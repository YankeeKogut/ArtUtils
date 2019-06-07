using System;
using System.Collections.Generic;
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
            var rut = dut.ToDataTable(String.Empty, nameSpace);

            Assert.IsInstanceOf<DataTable>(rut);
            Assert.AreEqual(nameSpace, rut.Namespace);
        }
    }
}
