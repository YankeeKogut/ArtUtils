# ArtUtils

- Bulk Insert for Sqlserver
- Conversion of any iEumerable to Data Table

## NuGet Gallery

- [NuGet Gallery: ArtUtils.Net](https://www.nuget.org/packages/ArtUtils.Net/)


## Usage

A quick example:

```C#
  var dut = new List<LineObj>
     {
      new LineObj{Sequence = 1},
      new LineObj{Sequence = 2},
     };

    //public static DataTable ToDataTable<T>(this IEnumerable<T> list, string tableName = "", string nameSpace = "")
     var sut = dut.ToDataTable(Constants.DBTableName, Constants.DBSchema);
     
    using (var connection = new SqlConnection("Data Source=.;Initial Catalog=SqlBulkTestDb;Integrated Security=True"))
    {
        BulkSql.Insert(sut, connection, TableName, Namespace);
    }
```

