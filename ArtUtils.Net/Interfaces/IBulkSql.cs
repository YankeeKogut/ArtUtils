using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ArtUtils.Net.Interfaces
{
    public interface IBulkSql
    {
        void Insert(DataTable dataTable, string sqlConnectionString, string targetTableName,
            string targetSchemaName = "");
        void Insert(DataTable dataTable, SqlConnection sqlConnection);
        void Insert(DataTable dataTable, SqlConnection sqlConnection, string targetTableName,
            string targetSchemaName = "");

        void Update(DataTable tableSource, string sqlConnectionString, string keyFieldName,
            string targetTableName,
            string targetSchema = "", List<string> fieldsToUpdate = null);

        void Update(DataTable tableSource, SqlConnection connection, string keyFieldName,
            string targetTableName,
            string targetSchema = "", List<string> fieldsToUpdate = null);

        void Merge(DataTable tableSource, SqlConnection connection, string keyFieldName,
            string targetTableName,
            string targetSchema = "", List<string> fieldsToUpdate = null);

        void Merge(DataTable tableSource, SqlConnection connection, string keyFieldName);
    }
}