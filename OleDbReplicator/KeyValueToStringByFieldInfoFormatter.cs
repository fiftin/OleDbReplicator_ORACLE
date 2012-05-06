using System;
using System.Collections.Generic;
using System.Text;

namespace OleDbReplicator
{
    /// <summary>
    /// 
    /// </summary>
    class KeyValueToStringByFieldInfoFormatter : IValueFormatter
    {
        public KeyValueToStringByFieldInfoFormatter(List<FieldInfo> fieldInfos)
        {
            FieldInfos = fieldInfos;
        }
        public object Format(object value, string arg)
        {
            KeyValuePair<string, object> keyValue = (KeyValuePair<string, object>)value;
            FieldInfo info = FieldInfos.Find(delegate(FieldInfo x) { return x.Name == keyValue.Key; });
            if (info == null)
                return keyValue.Value.ToString();
            return info.FieldValueToString(keyValue.Value);
        }

        private List<FieldInfo> fieldInfos;

        public List<FieldInfo> FieldInfos
        {
            get { return fieldInfos; }
            set { fieldInfos = value; }
        }

    }
}
