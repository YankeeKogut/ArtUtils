using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using ArtUtils.Net.Interfaces;

namespace ArtUtils.Net
{
    public class BulkSql : IBulkSql
    {
        public void Insert(DataTable dataTable, string sqlConnectionString, string targetTableName, string targetSchemaName = "")
        {
            var connection = new SqlConnection(sqlConnectionString);

            Insert(dataTable, connection, targetTableName, targetSchemaName);
        }

        public void Insert(DataTable dataTable, SqlConnection sqlConnection)
        {
            var tableName = dataTable.TableName;
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentException("DataTable must have TableName property populated, Namespace property is optional.");
            }

            var schema = dataTable.Namespace;

            Insert(dataTable, sqlConnection, tableName, schema);
        }

        public void Insert(DataTable dataTable, SqlConnection sqlConnection, string targetTableName, string targetSchemaName = "")
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

            var connectionStateInitial = sqlConnection.State;
            if (connectionStateInitial != ConnectionState.Open)
            {
                sqlConnection.Open();
            }

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

            if (connectionStateInitial != ConnectionState.Open)
            {
                sqlConnection.Close();
            }
        }

        public void Update(DataTable tableSource, string sqlConnectionString, string keyFieldName,
            string targetTableName,
            string targetSchema = "", List<string> fieldsToUpdate = null)
        {
            var connection = new SqlConnection(sqlConnectionString);

            Update(tableSource, connection, keyFieldName, targetTableName, targetSchema, fieldsToUpdate);
        }

        public void Update(DataTable tableSource, SqlConnection connection, string keyFieldName, string targetTableName,
            string targetSchema = "", List<string> fieldsToUpdate = null)
        {
            var tempTableName = GetTempTableName();
            var dest = targetSchema == string.Empty
                ? targetTableName
                : targetSchema + "." + targetTableName;

            if (fieldsToUpdate == null)
            {
                fieldsToUpdate = GetFieldsToUpdate(tableSource, keyFieldName);
            }

            var updateColumnList = fieldsToUpdate.Select(f => $"TARGET.{f} = SOURCE.{f}");

            var updateColumnsString = string.Join("," + Environment.NewLine, updateColumnList);

            var sqlUpdate = "UPDATE TARGET SET " + Environment.NewLine +
                            updateColumnsString + Environment.NewLine +
                            $" FROM {dest} TARGET " + Environment.NewLine +
                            $" INNER JOIN {tempTableName} SOURCE ON TARGET.{keyFieldName}=SOURCE.{keyFieldName}" +
                            Environment.NewLine;

            var sqlCreateTable = $"SELECT * INTO {tempTableName} FROM {dest} WHERE 1=0";

            var sqlDropTable = $"DROP TABLE {tempTableName}";

            var commandCreateTable = new SqlCommand(sqlCreateTable, connection);
            var commandUpdateTable = new SqlCommand(sqlUpdate, connection);
            var commandDropTable = new SqlCommand(sqlDropTable, connection);

            var connectionStateInitial = connection.State;
            if (connectionStateInitial != ConnectionState.Open)
            {
                connection.Open();
            }

            commandCreateTable.ExecuteNonQuery();
            Insert(tableSource, connection, tempTableName);
            commandUpdateTable.ExecuteNonQuery();
            commandDropTable.ExecuteNonQuery();

            if (connectionStateInitial != ConnectionState.Open)
            {
                connection.Close();
            }
        }

        private static List<string> GetFieldsToUpdate(DataTable tableSource, string keyFieldName)
        {
            var fieldsToUpdate = new List<string>();

            foreach (DataColumn tableSourceColumn in tableSource.Columns)
            {
                if (!tableSourceColumn.ColumnName.Equals(keyFieldName, StringComparison.InvariantCultureIgnoreCase))
                {
                    fieldsToUpdate.Add(tableSourceColumn.ColumnName);
                }
            }

            return fieldsToUpdate;
        }

        private static string GetTempTableName()
        {
            return "#TempArtUtils" + DateTime.UtcNow.ToString("HHmmssfff");
        }
    }
}
