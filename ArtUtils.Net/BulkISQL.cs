using System;
using System.Data;
using System.Data.SqlClient;

namespace ArtUtils.Net
{
    public static class BulkSql
    {
        public static void Insert(DataTable dataTable, string sqlConnectionString, string targetTableName, string targetSchemaName = "")
        {
            var connection = new SqlConnection(sqlConnectionString);

            Insert(dataTable, connection, targetTableName, targetSchemaName);
        }

        public static void Insert(DataTable dataTable, SqlConnection sqlConnection)
        {
            var tableName = dataTable.TableName;
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentException("DataTable must have TableName property populated, Namespace property is optional.");
            }

            var schema = dataTable.Namespace;

            Insert(dataTable, sqlConnection, tableName, schema);
        }

        public static void Insert(DataTable dataTable, SqlConnection sqlConnection, string targetTableName, string targetSchemaName = "")
        {
            var dest = targetSchemaName == string.Empty
                ? targetTableName
                : targetSchemaName + "." + targetTableName;

            var bulkCopy =
                new SqlBulkCopy
                (
                    sqlConnection,
                    SqlBulkCopyOptions.TableLock |
                    SqlBulkCopyOptions.FireTriggers |
                    SqlBulkCopyOptions.UseInternalTransaction,
                    null
                )
                {
                    DestinationTableName = dest
                };

            sqlConnection.Open();
            try
            {
                bulkCopy.WriteToServer(dataTable);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                if (e.Message == Constants.ErrWrongDataType)
                {
                    throw new Exception(Constants.ErrorMessageFixColumnsOrder, e);
                }

                throw;
            }
            
            sqlConnection.Close();
        }
    }
}
