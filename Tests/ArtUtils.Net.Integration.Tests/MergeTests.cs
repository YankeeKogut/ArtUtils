using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using NUnit.Framework;

namespace ArtUtils.Net.Integration.Tests
{
    public class MergeTests
    {
        [Test]
        public void MergeTest()
        {
            const string connectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=MergeTest;Integrated Security=SSPI";

            var sampleDataList = new List<SampleDataClass>
            {
                new SampleDataClass { ProductID = 1, ProductNameDifferentFromDbColumnName = "One" },
                new SampleDataClass { ProductID = 2, ProductNameDifferentFromDbColumnName = "Two" }
            };

            var connection = new SqlConnection(connectionString);
            connection.Open();

            var clearCommand = new SqlCommand("DELETE FROM Products", connection);
            clearCommand.ExecuteNonQuery();

            new BulkSql().Insert(sampleDataList.ToDataTable("Products", "dbo"), connection);

            var mergeDataList = new List<SampleDataClass>
            {
                new SampleDataClass { ProductID = 2, ProductNameDifferentFromDbColumnName = "ModifiedTwo" },
                new SampleDataClass { ProductID = 3, ProductNameDifferentFromDbColumnName = "Three" }
            };

            new BulkSql().Merge(mergeDataList.ToDataTable("Products"), connection, "ProductId");

            var selectCommand = new SqlCommand("SELECT * FROM Products", connection);
            SqlDataReader dr = selectCommand.ExecuteReader();

            var sut = new List<SampleDataClass>();

            while (dr.Read())
            {
                sut.Add(new SampleDataClass
                {
                    ProductID = Convert.ToInt32(dr["ProductId"]),
                    ProductNameDifferentFromDbColumnName = dr["Product_Name_DifferentFromDBColumnName"].ToString()
                });

            }

            Assert.AreEqual(3, sut.Count);
        }

        [Test]
        public void MergeWrongSchemaTest()
        {
            const string connectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=MergeTest;Integrated Security=SSPI";

            var sampleDataList = new List<SampleDataClass>
            {
                new SampleDataClass { ProductID = 1, ProductNameDifferentFromDbColumnName = "One" },
                new SampleDataClass { ProductID = 2, ProductNameDifferentFromDbColumnName = "Two" }
            };

            var connection = new SqlConnection(connectionString);
            connection.Open();

            var clearCommand = new SqlCommand("DELETE FROM Products", connection);
            clearCommand.ExecuteNonQuery();

            new BulkSql().Insert(sampleDataList.ToDataTable("Products"), connection);

            var mergeDataList = new List<SampleDataClass>
            {
                new SampleDataClass { ProductID = 2, ProductNameDifferentFromDbColumnName = "ModifiedTwo" },
                new SampleDataClass { ProductID = 3, ProductNameDifferentFromDbColumnName = "Three" }
            };

            Assert.Throws<SqlException>(()=> new BulkSql().Merge(mergeDataList.ToDataTable("Products", "WromgSchema"), connection, "ProductId"));

            var selectCommand = new SqlCommand("SELECT * FROM Products", connection);
            var dr = selectCommand.ExecuteReader();

            var sut = new List<SampleDataClass>();

            while (dr.Read())
            {
                sut.Add(new SampleDataClass
                {
                    ProductID = Convert.ToInt32(dr["ProductId"]),
                    ProductNameDifferentFromDbColumnName = dr["Product_Name_DifferentFromDBColumnName"].ToString()
                });

            }

            Assert.AreNotEqual(3, sut.Count);
        }
    }
}
