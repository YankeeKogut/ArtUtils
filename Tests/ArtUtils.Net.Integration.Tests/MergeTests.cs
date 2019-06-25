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
                new SampleDataClass { ProductID = 1, ProductName = "One" },
                new SampleDataClass { ProductID = 2, ProductName = "Two" }
            };

            var connection = new SqlConnection(connectionString);
            connection.Open();

            var clearCommand = new SqlCommand("DELETE FROM Products", connection);
            clearCommand.ExecuteNonQuery();

            new BulkSql().Insert(sampleDataList.ToDataTable("Products"), connection);

            var mergeDataList = new List<SampleDataClass>
            {
                new SampleDataClass { ProductID = 2, ProductName = "ModifiedTwo" },
                new SampleDataClass { ProductID = 3, ProductName = "Three" }
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
                    ProductName = dr["ProductName"].ToString()
                });

            }

            Assert.AreEqual(3, sut.Count);
        }
    }
}
