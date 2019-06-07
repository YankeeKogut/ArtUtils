using System.Data;
using System.Data.SqlClient;

namespace ArtUtils.Net
{
    public static class BulkSql
    {
        public static void Insert(DataTable dataTable, string sqlConnectionString, string targetTableName, string targetSchemaName)
        { 
            var connection = new SqlConnection(sqlConnectionString);
            
            Insert(dataTable, connection, targetTableName, targetSchemaName);
        }

        public static void Insert(DataTable dataTable, SqlConnection sqlConnection, string targetTableName, string targetSchemaName="")
        {
            var  dest =targetSchemaName == string.Empty 
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
            bulkCopy.WriteToServer(dataTable);
            sqlConnection.Close();
        }
    }
}
