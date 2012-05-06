using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Data.OleDb;

namespace OleDbReplicator
{

    class Program
    {

        static void WriteError(string error)
        {
            ConsoleColor temp = Console.BackgroundColor;
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine(error);
            Console.BackgroundColor = temp;

            System.IO.File.AppendAllText("OleDbReplicator.error.log\r\n",
                string.Format("{0} {1}", DateTime.Now, error));
        }

        static void WriteError(Exception ex)
        {
            WriteError(ex.Message);
        }

        static void WriteTableError(string tableName, Exception ex)
        {
            WriteError(string.Format("table {0}: {1}", tableName, ex.Message));
        }
        static void WriteSourceError(string source, Exception ex)
        {
            WriteError(string.Format("source {0}: {1}", source, ex.Message));
        }

        static string Join<T>(IEnumerable<T> values, string separator, IValueFormatter fromatter)
        {
            string result = "";
            foreach (object obj in values)
            {
                if (result != "")
                    result += separator;
                if (fromatter != null)
                    result += fromatter.Format(obj, null).ToString();
                else
                    result += obj.ToString();
            }
            return result;
        }

        static void Func(OleDbCommand command, XmlElement srcElem)
        {
            //
            //ПОДКЛЮЧАЕТСЯ К БД ИЗ КОТОРОЙ НУЖНО ЧИТАТЬ ДАННЫЕ
            //

            OleDbConnection sourceDbConn = new OleDbConnection(srcElem.Attributes["connectionString"].Value);
            //OleDbConnection sourceDbConn = new OleDbConnection("Provider=vfpoledb.1;Data Source=C:/temp/234old/SHTAT.DBF");
            sourceDbConn.Open();
            OleDbCommand selectCommand = sourceDbConn.CreateCommand();
            try
            {
                //
                //ЧИТАЕТ КОНФИГ И ЧИТАЕТ ДАННЫН ИЗ БД
                //
                XmlElement tables = (XmlElement)srcElem["tables"];
                foreach (XmlNode tablesNode in tables)
                {
                    XmlElement tableElem = tablesNode as XmlElement;
                    if (tableElem != null && tableElem.Name == "table")
                    {
                        //ИМЯ ТПБЛИЦЫ
                        string tableName = tableElem.GetAttribute("name");
                        try
                        {
                            //ПОЛЯ ТАБЛИЦЫ
                            List<FieldInfo> fieldInfos = new List<FieldInfo>();
                            //СЧИТЫВАТЬ ВСЕ ПОЛЯ КОТОВЫЕ ЕСТЬ В ЗАПРОСЕ
                            bool fieldsFromQuery = false;
                            //СЧИТЫВАЕТ ИЗ КОНФИГА ПОЛЯ ТАБЛИЦЫ
                            XmlElement fieldsElem = (XmlElement)tableElem["fields"];
                            if (fieldsElem != null)
                            {
                                foreach (XmlNode fieldNode in fieldsElem)
                                {
                                    XmlElement fieldElem = fieldNode as XmlElement;
                                    if (fieldElem != null && fieldElem.Name == "field")
                                    {
                                        FieldInfo fieldInfo = new FieldInfo();
                                        fieldInfo.Name = fieldElem.GetAttribute("name").ToLower();
                                        fieldInfo.SourceName = fieldElem.GetAttribute("sourceField").ToLower();
                                        fieldInfo.DBTypeName = fieldElem.GetAttribute("type").ToUpper();
                                        fieldInfos.Add(fieldInfo);
                                    }
                                }
                            }
                            else
                            {
                                fieldsFromQuery = true;
                            }

                            //ВЫПОЛНЕНИЕ ЗАПРОСА ДАННЫХ ИЗ БД
                            string selectQuery = tableElem["select"].InnerText;
                            selectCommand.CommandText = selectQuery;
                            OleDbDataReader reader = selectCommand.ExecuteReader();
                            //ЧТЕНИЕ ДАННЫХ ИЗ БД
                            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                            while (reader.Read())
                            {
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    string sourceFieldName = reader.GetName(i).ToLower();
                                    FieldInfo info = fieldInfos.Find(delegate(FieldInfo x)
                                    {
                                        return x.SourceName == sourceFieldName;
                                    });

                                    if (info == null && fieldsFromQuery)
                                    {
                                        info = new FieldInfo();
                                        info.SourceName = sourceFieldName;
                                        info.Name = info.SourceName;
                                        fieldInfos.Add(info);
                                    }

                                    if (info != null)
                                    {
                                        info.SourceType = reader.GetFieldType(i);
                                        if (string.IsNullOrEmpty(info.DBTypeName))
                                        {
                                            switch (info.SourceType.FullName)
                                            {
                                                case "System.Boolean":
                                                    info.DBTypeName = "NUMBER(1)";
                                                    break;
                                                case "System.Float":
                                                    info.DBTypeName = "FLOAT";
                                                    break;
                                                case "System.Double":
                                                    info.DBTypeName = "DOUBLE";
                                                    break;
                                                case "System.Byte":
                                                case "System.Int16":
                                                    info.DBTypeName = "SMALLINT";
                                                    break;
                                                case "System.Int32":
                                                    info.DBTypeName = "INT";
                                                    break;
                                                case "System.Decimal":
                                                    info.DBTypeName = "DECIMAL";
                                                    break;
                                                case "System.DateTime":
                                                    info.DBTypeName = "DATE";
                                                    break;
                                                case "System.String":
                                                    info.DBTypeName = "VARCHAR";
                                                    info.Length = reader.GetString(i).Length;
                                                    break;
                                                default:
                                                    throw new Exception("incorrent type");
                                            }
                                        }
                                        else
                                        {
                                            if (info.SourceType.FullName == "System.String")
                                            {
                                                int len = reader.GetString(i).Length;
                                                if (len > info.Length)
                                                    info.Length = len;
                                            }
                                        }
                                    }
                                }

                                Dictionary<string, object> srcFieldValues = new Dictionary<string, object>();
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    string fieldName = reader.GetName(i).ToLower();
                                    FieldInfo info = fieldInfos.Find(delegate(FieldInfo x) { return x.SourceName == fieldName; });
                                    if (info != null)
                                    {
                                        object fieldValue = reader.GetValue(i);
                                        srcFieldValues.Add(info.Name, fieldValue);
                                    }
                                }
                                rows.Add(srcFieldValues);
                            }

                            //
                            // Создаение таблицы, вставка в нее данных
                            //


                            //Удаляем старую таблицу
                            try
                            {
                                command.CommandText = string.Format("DROP TABLE \"{0}\"", tableName);

                                command.ExecuteNonQuery();
                            }
                            catch
                            {
                            }

                            //Заново создаем таблицу
                            string createTableParamsString = Join(fieldInfos, ", \n", null);
                            if (createTableParamsString.Trim() != "")
                            {
                                command.CommandText = string.Format("CREATE TABLE \"{0}\"({1})", tableName, createTableParamsString);
                                command.ExecuteNonQuery();

                                //Заполняем таблицу данными
                                foreach (Dictionary<string, object> row in rows)
                                {
                                    string fieldNamesString = string.Format("\"{0}\"", Join(row.Keys, "\", \"", null));
                                    string fieldValuesString = Join(row, ", ", new KeyValueToStringByFieldInfoFormatter(fieldInfos));
                                    //Запрос на вставку текущей строки
                                    string insertQuery = string.Format("INSERT INTO \"{0}\"({1}) VALUES ({2})",
                                        tableName, fieldNamesString, fieldValuesString);
                                    Console.WriteLine(fieldValuesString);
                                    command.CommandText = insertQuery;
                                    command.ExecuteNonQuery();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            WriteTableError(tableName, ex);
                        }
                    }
                }
            }
            finally
            {
                sourceDbConn.Close();
            }
        }

        static void Main(string[] args)
        {

            //Чтение конфигурационного файла
            XmlDocument doc = new XmlDocument();
            doc.Load("OleDbReplicator.config");
            XmlElement root = doc.DocumentElement;

            //
            // Подключение к БД в которую пишем
            //
            string connectionString = root["destDbConnectionString"].InnerText;
            OleDbConnection destDbConn = new OleDbConnection(connectionString);
            destDbConn.Open();
            XmlElement sourcesElem = root["sources"];
            try
            {
                foreach (XmlNode sourceNode in sourcesElem)
                {
                    XmlElement sourceElem = sourceNode as XmlElement;

                    if (sourceElem != null && sourceElem.Name == "source")
                    {
                        XmlAttribute connStringAttr = sourceElem.Attributes["connectionString"];
                        string source = "<empty>";
                        if (connStringAttr != null)
                        {
                            source = connStringAttr.Value;
                        }

                        using (OleDbCommand insertCommand = new OleDbCommand())
                        {
                            insertCommand.Connection = destDbConn;
                            try
                            {
                                Func(insertCommand, sourceElem);
                            }
                            catch (Exception ex)
                            {
                                WriteSourceError(source, ex);
                            }
                        }
                    }
                }
            }
            finally
            {
                destDbConn.Close();
            }
        }
    }
}