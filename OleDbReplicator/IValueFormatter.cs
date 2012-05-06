using System;
using System.Collections.Generic;
using System.Text;

namespace OleDbReplicator
{
    /// <summary>
    /// 
    /// </summary>
    interface IValueFormatter
    {
        object Format(object value, string arg);
    }
}
