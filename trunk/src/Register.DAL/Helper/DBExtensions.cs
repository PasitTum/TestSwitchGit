﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCS.Register.DAL
{
    public static class DBExtensions
    {
        public static IEnumerable<dynamic> DynamicListFromSql(this DbContext db, string Sql, Dictionary<string, object> Params)
        {
            using (var cmd = db.Database.Connection.CreateCommand())
            {
                cmd.CommandText = Sql;
                if (cmd.Connection.State != ConnectionState.Open) { cmd.Connection.Open(); }

                foreach (KeyValuePair<string, object> p in Params)
                {
                    DbParameter dbParameter = cmd.CreateParameter();
                    dbParameter.ParameterName = p.Key;
                    if (p.Value is Guid)
                    {
                        dbParameter.DbType = DbType.Guid;
                    } else if (p.Value is DateTime)
                    {
                        dbParameter.DbType = DbType.DateTime;
                    }
                    dbParameter.Value = (p.Value == null ? DBNull.Value : p.Value);
                    cmd.Parameters.Add(dbParameter);
                }

                using (var dataReader = cmd.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        var row = new ExpandoObject() as IDictionary<string, object>;
                        for (var fieldCount = 0; fieldCount < dataReader.FieldCount; fieldCount++)
                        {
                            row.Add(dataReader.GetName(fieldCount), dataReader[fieldCount]);
                        }
                        yield return row;
                    }
                }
            }
        }
    }
}
