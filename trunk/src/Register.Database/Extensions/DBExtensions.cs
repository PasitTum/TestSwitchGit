using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Dynamic;
using CSP.Lib.Models;
using CSP.Lib.Diagnostic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Collections;

namespace Register.Database
{
    public static class DBExtensions
    {
        private static System.Diagnostics.TraceSwitch tsw = new System.Diagnostics.TraceSwitch("mySwitch", "");
        static string _commandTimeout = "900";

        public static IEnumerable<dynamic> DynamicListFromSql(this DbContext db, string Sql, Dictionary<string, object> Params = null)
        {
            using (var cmd = db.Database.Connection.CreateCommand())
            {
                cmd.CommandText = Sql;
                if (cmd.Connection.State != ConnectionState.Open) { cmd.Connection.Open(); }

                if (Params != null)
                {
                    foreach (KeyValuePair<string, object> p in Params)
                    {
                        DbParameter dbParameter = cmd.CreateParameter();
                        dbParameter.ParameterName = p.Key;
                        if (p.Value is Guid)
                        {
                            dbParameter.DbType = DbType.Guid;
                        }
                        else if (p.Value is DateTime)
                        {
                            dbParameter.DbType = DbType.DateTime;
                        }
                        dbParameter.Value = (p.Value == null ? DBNull.Value : p.Value);
                        cmd.Parameters.Add(dbParameter);
                    }
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

   
        // Execute Stored
        public static DbRawSqlQuery<TResult> ExecuteStored<TResult>(this DbContext db, string sql, List<CommonParameter> parameter)
        {
            try
            {
                List<SqlParameter> prms = null;
                if (parameter != null)  // input parameter
                {
                    prms = new List<SqlParameter>();
                    parameter.ForEach(p =>
                    {
                        var dbParameter = new SqlParameter()
                        {
                            ParameterName = p.ParameterName,
                            Direction = ParameterDirection.Input,
                            Value = (p.Value == null ? DBNull.Value : p.Value)
                        };
                        if (p.Value is Guid)
                        {
                            dbParameter.DbType = DbType.Guid;
                        }
                        else if (p.Value is DateTime)
                        {
                            dbParameter.DbType = DbType.DateTime;
                        }
                        else
                        {
                            dbParameter.DbType = (p.DbType.HasValue ? p.DbType.Value : DbType.String);
                        }
                        if (p.Size.HasValue) dbParameter.Size = p.Size.Value;
                        prms.Add(dbParameter);
                    });
                }

                return db.Database.SqlQuery<TResult>(sql, prms.ToArray());
            }
            catch (Exception ex)
            {
                Log.WriteErrorLog(tsw.TraceError, "DB.ExecuteStored", ex);
                throw ex;
            }
        }


        /// <summary>
        /// Execute Stored NonQuery
        /// </summary>
        /// <param name="db"></param>
        /// <param name="storedProcedure"></param>
        /// <param name="parameter"></param>
        /// <param name="parameterOutput"></param>
        /// <returns></returns>
        public static ResultDataInfo ExecuteStored(this DbContext db, string storedProcedure, List<CommonParameter> parameter, List<CommonParameter> parameterOutput)
        {
            ResultDataInfo result = new ResultDataInfo();
            try
            {
                using (var cmd = db.Database.Connection.CreateCommand())
                {
                    cmd.CommandText = storedProcedure;
                    if (cmd.Connection.State != ConnectionState.Open) { cmd.Connection.Open(); }
                    if (parameter != null)
                    {
                        parameter.ForEach(p =>
                        {
                            var dbParameter = cmd.CreateParameter();
                            dbParameter.ParameterName = p.ParameterName;
                            if (p.Value is Guid)
                            {
                                dbParameter.DbType = DbType.Guid;
                            }
                            else if (p.Value is DateTime)
                            {
                                dbParameter.DbType = DbType.DateTime;
                            }
                            dbParameter.Direction = ParameterDirection.Input;
                            dbParameter.Value = (p.Value == null ? DBNull.Value : p.Value);
                            if (p.Size.HasValue) dbParameter.Size = p.Size.Value;
                            cmd.Parameters.Add(dbParameter);
                        });
                    }

                    if (parameterOutput != null && parameterOutput.Count > 0)
                    {
                        parameterOutput.ForEach(p =>
                        {
                            var dbParameter = cmd.CreateParameter();
                            dbParameter.ParameterName = p.ParameterName;
                            if (p.Value is Guid)
                            {
                                dbParameter.DbType = DbType.Guid;
                            }
                            else if (p.Value is DateTime)
                            {
                                dbParameter.DbType = DbType.DateTime;
                            }
                            dbParameter.Direction = ParameterDirection.Output;
                            dbParameter.Value = (p.Value == null ? DBNull.Value : p.Value);
                            if (p.Size.HasValue) dbParameter.Size = p.Size.Value;
                            cmd.Parameters.Add(dbParameter);
                        });
                    }

                    var rowAffected = cmd.ExecuteNonQuery();
                    result.Data = rowAffected;
                    result.Success = true;
                    result.ErrorMessage = string.Empty;
                    if (parameterOutput != null && parameterOutput.Count > 0)
                    {
                        parameterOutput.ForEach(p =>
                        {
                            p.Value = cmd.Parameters[p.ParameterName].Value;
                        });
                        }
                    }
            }
            catch (Exception ex)
            {
                Log.WriteErrorLog(tsw.TraceError, "DB.ExecuteStored", ex);
                result.ErrorCode = -999;
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }
        static public DataSet ExecStoredCrystal(this DbContext db, string storeName, string tableName, SqlParameter[] parameter)
        {
            var connStr = db.Database.Connection as SqlConnection;
            if (tsw.TraceInfo && parameter != null)
            {
                string paraValue = string.Empty;
                for (int iLog = 0; iLog < parameter.Length; iLog++)
                {
                    paraValue += string.Format(" Para[{0}] = {1}", iLog, parameter[iLog].Value.ToString());
                }
                Log.WriteInformationLog(tsw.TraceError, String.Format("ExecStored ConnStr : {0}\r\nstoreName :{1} \r\nParameter : {2}", connStr, storeName, paraValue.ToString()));
            }
            DataSet dsResult = new DataSet();
            if (tableName == null || tableName.Length == 0)
            {
                tableName = "tablename";
            }
            SqlConnection sqlconn = new SqlConnection(connStr.ConnectionString);
            try
            {
                SqlCommand cm = new SqlCommand();
                cm.Connection = sqlconn;
                cm.CommandText = storeName;
                cm.CommandType = CommandType.StoredProcedure;
                if (_commandTimeout != null && _commandTimeout != string.Empty)
                {
                    cm.CommandTimeout = Convert.ToInt32(_commandTimeout);
                }
                if (parameter != null && parameter.Length > 0)
                {
                    for (int iPara = 0; iPara < parameter.Length; iPara++)
                    {
                        cm.Parameters.Add(parameter[iPara]);
                    }
                }
                SqlDataAdapter da = new SqlDataAdapter(cm);
                da.Fill(dsResult, tableName);
            }
            catch (SqlException ex)
            {
                if (sqlconn != null)
                {
                    sqlconn.Close();
                }
                Log.WriteErrorLog(tsw.TraceError, ex);
                //WriteLog(tsw.TraceError, "ExecStored", String.Format("ConnStr : {0}\r\nStoredName :{1}\r\nError : {2}", connStr, storeName, ex.Message + "\r\n" + ex.StackTrace));
                throw new Exception(ex.Message);

            }
            catch (Exception ex)
            {
                Log.WriteErrorLog(tsw.TraceError, ex);
                //WriteLog(tsw.TraceError, "ExecStored", String.Format("ConnStr : {0}\r\nStoredName :{1}\r\nError : {2}", connStr, storeName, ex.Message + "\r\n" + ex.StackTrace));
                throw new Exception(ex.Message);
            }
            return dsResult;
        }
    }
}
