using System;
using System.Collections.Generic;
using System.Text;

namespace OleDbReplicator
{
    /// <summary>
    /// 
    /// </summary>
    public class FieldInfo
    {
        private int length = -1;
        private string dbTypeName = "";
        private string name = "";
        private string sourceName = "";
        private Type sourceType = null;

        public Type SourceType
        {
            get { return sourceType; }
            set { sourceType = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string SourceName
        {
            get { return sourceName; }
            set { sourceName = value; }
        }

        public string DBTypeName
        {
            get { return dbTypeName; }
            set { dbTypeName = value; }
        }

        public int Length
        {
            get { return length; }
            set { length = value; }
        }

        public string FieldValueToString(object obj)
        {
            string result = "";
            switch (DBTypeName)
            {
                case "BIT":
                    result = ((bool)obj) ? "1" : "0";
                    break;
                case "DATETIME":
                    DateTime dateTime = (DateTime)obj;
                    if (dateTime.Year < 1800)
                        obj = DateTime.Parse("01.01.2000");
                    result = string.Format("'{0:yyyy-MM-dd}'", obj);
                    break;
                case "VARCHAR":
                    string stringValue = UnknownEncoding.Unknown.GetString(Encoding.Unicode.GetBytes((string)obj)).Trim();
                    result = string.Format("'{0}'", stringValue);
                    break;
                case "DECIMAL":
                    result = ((decimal)obj).ToString(System.Globalization.CultureInfo.InvariantCulture);
                    break;
                case "FLOAT":
                    result = ((float)obj).ToString(System.Globalization.CultureInfo.InvariantCulture);
                    break;
                case "DOUBLE":
                    result = ((double)obj).ToString(System.Globalization.CultureInfo.InvariantCulture);
                    break;
                default:
                    result = obj.ToString();
                    break;
            }
            return result;
        }

        public override string ToString()
        {
            string ret = string.Format("[{0}] {1}", Name, DBTypeName);
            if (Length != -1)
                ret += string.Format("({0})", Length);
            return ret;
        }
    }

}
