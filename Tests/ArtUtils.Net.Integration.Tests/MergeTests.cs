using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using NUnit.Framework;

namespace ArtUtils.Net.Integration.Tests
{
    public class MergeTests
    {
        private const string ConnectionString = "Data Source=localhost;Initial Catalog=MergeTest;Integrated Security=SSPI";

        [Test]
        public void MergeTest()
        {
            
            var sampleDataList = new List<SampleDataClass>
            {
                new SampleDataClass { ProductId = 1, ProductNameDifferentFromDbColumnName = "One" },
                new SampleDataClass { ProductId = 2, ProductNameDifferentFromDbColumnName = "Two" }
            };

            var connection = new SqlConnection(ConnectionString);
            connection.Open();

            var clearCommand = new SqlCommand("DELETE FROM Products", connection);
            clearCommand.ExecuteNonQuery();

            new BulkSql().Insert(sampleDataList.ToDataTable("Products", "dbo"), connection);

            var mergeDataList = new List<SampleDataClass>
            {
                new SampleDataClass { ProductId = 2, ProductNameDifferentFromDbColumnName = "ModifiedTwo" },
                new SampleDataClass { ProductId = 3, ProductNameDifferentFromDbColumnName = "Three" }
            };

            new BulkSql().Merge(mergeDataList.ToDataTable("Products"), connection, "ProductId");

            var selectCommand = new SqlCommand("SELECT * FROM Products", connection);
            SqlDataReader dr = selectCommand.ExecuteReader();

            var sut = new List<SampleDataClass>();

            while (dr.Read())
            {
                sut.Add(new SampleDataClass
                {
                    ProductId = Convert.ToInt32(dr["ProductId"]),
                    ProductNameDifferentFromDbColumnName = dr["ProductName"].ToString()
                });

            }

            Assert.AreEqual(3, sut.Count);
        }

        [Test]
        public void MergeWrongSchemaTest()
        {
            var sampleDataList = new List<SampleDataClass>
            {
                new SampleDataClass { ProductId = 1, ProductNameDifferentFromDbColumnName = "One" },
                new SampleDataClass { ProductId = 2, ProductNameDifferentFromDbColumnName = "Two" }
            };

            var connection = new SqlConnection(ConnectionString);
            connection.Open();

            var clearCommand = new SqlCommand("DELETE FROM Products", connection);
            clearCommand.ExecuteNonQuery();

            new BulkSql().Insert(sampleDataList.ToDataTable("Products"), connection);

            var mergeDataList = new List<SampleDataClass>
            {
                new SampleDataClass { ProductId = 2, ProductNameDifferentFromDbColumnName = "ModifiedTwo" },
                new SampleDataClass { ProductId = 3, ProductNameDifferentFromDbColumnName = "Three" }
            };

            Assert.Throws<SqlException>(()=> new BulkSql().Merge(mergeDataList.ToDataTable("Products", "WrongSchema"), connection, "ProductId"));

            var selectCommand = new SqlCommand("SELECT * FROM Products", connection);
            var dr = selectCommand.ExecuteReader();

            var sut = new List<SampleDataClass>();

            while (dr.Read())
            {
                sut.Add(new SampleDataClass
                {
                    ProductId = Convert.ToInt32(dr["ProductId"]),
                    ProductNameDifferentFromDbColumnName = dr["ProductName"].ToString()
                });

            }

            Assert.AreNotEqual(3, sut.Count);
        }
    }
}
