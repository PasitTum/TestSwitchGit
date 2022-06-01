using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Register.Database
{
    public class CommonParameter
    {
        public string ParameterName { get; set; }
        public DbType? DbType { get; set; }
        public int? Size { get; set; }
        public object Value { get; set; }

        public CommonParameter()
        {
            this.DbType = System.Data.DbType.AnsiString;
        }

        public CommonParameter(string name, object value)
        {
            ParameterName = name;
            Value = value;
        }

        public CommonParameter(string name, object value, DbType dbType, int size)
        {
            ParameterName = name;
            Value = value;
            this.DbType = dbType;
            this.Size = size;
        }
    }
}
