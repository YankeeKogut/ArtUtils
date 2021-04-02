using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using ArtUtils.Net.Exceptions;
using ArtUtils.Net.Interfaces;

namespace ArtUtils.Net
{
    // ReSharper disable once UnusedMember.Global
    public class BulkSql : IBulkSql
    {
        #region Inserts


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
            CheckForNullRows(dataTable);

            var dest = GetTargetName(targetTableName, targetSchemaName);

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
        #endregion

        #region Updates

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
            CheckForNullRows(tableSource);

            var tempTableName = GetTempTableName();
            var dest = GetTargetName(targetTableName, targetSchema);

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

        #endregion

        #region Merge


        public void Merge(DataTable tableSource, SqlConnection connection, string keyFieldName)
        {
            var tableName = tableSource.TableName;
            var schemaName = tableSource.Namespace;
            Merge(tableSource, connection, keyFieldName, tableName, schemaName);
        }

        public void Merge(DataTable tableSource, SqlConnection connection, string keyFieldName, string targetTableName,
            string targetSchema = "", List<string> fieldsToUpdate = null)
        {

            CheckForNullRows(tableSource);

            var tempTableName = GetTempTableName();
            var dest = GetTargetName(targetTableName, targetSchema);

            if (fieldsToUpdate == null)
            {
                fieldsToUpdate = GetFieldsToUpdate(tableSource, keyFieldName);
            }

            var updateColumnList = fieldsToUpdate.Select(f => $"TARGET.{f} = SOURCE.{f}");
            var updateColumnsString = string.Join("," + Environment.NewLine, updateColumnList);

            var insertColumnList = GetFieldsToInsert(tableSource, string.Empty);
            var insertSourceColumnList = GetFieldsToInsert(tableSource, "SOURCE.");

            var sqlCreateTable = $"SELECT * INTO {tempTableName} FROM {dest} WHERE 1=0";

            var sqlMerge = string.Join(Environment.NewLine,
                $"MERGE {dest} AS TARGET ",
                $"USING {tempTableName} AS SOURCE",
                $"ON (TARGET.{keyFieldName} = SOURCE.{keyFieldName})",
                $"WHEN MATCHED ",
                $"THEN UPDATE SET {updateColumnsString}",
                $"WHEN NOT MATCHED BY TARGET ",
                $"THEN INSERT ({string.Join("," + Environment.NewLine, insertColumnList)}) ",
                $"VALUES ({string.Join("," + Environment.NewLine, insertSourceColumnList)});"
                );

            var sqlDropTable = $"DROP TABLE {tempTableName}";

            var commandCreateTable = new SqlCommand(sqlCreateTable, connection);
            var commandMergeToTable = new SqlCommand(sqlMerge, connection);
            var commandDropTable = new SqlCommand(sqlDropTable, connection);

            var connectionStateInitial = connection.State;
            if (connectionStateInitial != ConnectionState.Open)
            {
                connection.Open();
            }

            commandCreateTable.ExecuteNonQuery();
            Insert(tableSource, connection, tempTableName);
            commandMergeToTable.ExecuteNonQuery();
            commandDropTable.ExecuteNonQuery();


            if (connectionStateInitial != ConnectionState.Open)
            {
                connection.Close();
            }
        }

        #endregion

        #region Private methods

        private static void CheckForNullRows(DataTable table)
        {
            if (table.Rows.Cast<DataRow>().Any(tableRow => tableRow == null))
            {
                throw new NullRecordInDataSetException(Constants.ErrorNullRecordInDataSet);
            }
        }

        private static string GetTargetName(string targetTableName, string targetSchema)
        {
            return targetSchema == string.Empty
                ? targetTableName
                : targetSchema + "." + targetTableName;
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

        private static List<string> GetFieldsToInsert(DataTable tableSource, string prefix)
        {
            var result = new List<string>();

            foreach (DataColumn tableSourceColumn in tableSource.Columns)
            {
                result.Add(prefix + tableSourceColumn.ColumnName);
            }

            return result;
        }

        private static string GetTempTableName()
        {
            // ReSharper disable once StringLiteralTypo
            return "#TempArtUtils" + DateTime.UtcNow.ToString("HHmmssfff");
        }

        #endregion
    }
}
