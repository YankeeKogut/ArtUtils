using System.Collections.Generic;
using System.Linq;
using ArtUtils.Net.Core.Exceptions;
using NUnit.Framework;
using DataTable = System.Data.DataTable;
// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace ArtUtils.Net.Unit.Tests
{
    public class TablesTests
    {
        private class SampleTestDataClass
        {
            public string StringData { get; set; }
            public int IntData { get; set; }
        }

        private List<SampleTestDataClass> GenerateSampleData()
        {
            return new List<SampleTestDataClass>
            {
                new SampleTestDataClass {StringData = "one", IntData = 1},
                new SampleTestDataClass {StringData = "two", IntData = 2}
            };
        }

        [Test]
        public void ToDataTableTest()
        {
            var dut = GenerateSampleData();

            var rut = dut.ToDataTable();

            Assert.IsInstanceOf<DataTable>(rut);
            Assert.AreEqual(2, rut.Rows.Count);
        }

        [Test]
        public void ToDataTableNameTest()
        {
            var dut = GenerateSampleData();
            const string tableName = "string name";
            var rut = dut.ToDataTable(tableName);

            Assert.IsInstanceOf<DataTable>(rut);
            Assert.AreEqual(tableName, rut.TableName);
        }

        [Test]
        public void ToDataTableNamespaceTest()
        {
            var dut = GenerateSampleData();
            const string nameSpace = "string name space";
            var rut = dut.ToDataTable(string.Empty, nameSpace);

            Assert.IsInstanceOf<DataTable>(rut);
            Assert.AreEqual(nameSpace, rut.Namespace);
        }

        [Test]
        public void GetColumnNameWithoutAttributes()
        {
            var type = typeof(SampleDataClass);
            var properties = type.GetProperties();

            var sutFound = properties.Any(ts => Tables.GetColumnName(ts) == "ProductId");

            Assert.IsTrue(sutFound, "Failed to find column name for the field without attributes");
        }

        [Test]
        public void GetColumnNameWithAttributes()
        {
            var type = typeof(SampleDataClass);
            var properties = type.GetProperties();

            var sutFound = properties.Any(ts => Tables.GetColumnName(ts) == SampleDataClass.TestFieldAttribute);

            Assert.IsTrue(sutFound, "Failed to find column name for the field with attributes");
        }

        [Test]
        public void AssureNullExceptionThrown()
        {
            var dut = GenerateSampleData();
            dut.Add(null);

            Assert.Catch<NullRecordInDataSetException>(() => dut.ToDataTable(string.Empty, string.Empty));
        }
    }
}
