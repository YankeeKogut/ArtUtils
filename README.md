
# ArtUtils

- Bulk Insert, Update and Merge for SQL Server 
- Conversion of any iEumerable to Data Table (as ToDataTable() extension method)

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
     
    using (var connection = 
                new SqlConnection("Data Source=.;Initial Catalog=SqlBulkTestDb;Integrated Security=True")  
    {
        new BulkSql().Insert(sut, connection);
    }
```

## Release history

**1.0.8.0** - Added field name attributes for DTO classes
    Now you can define your DTO classes with field names different from database column names using [FieldName("...")] attribute
    
   for example:
```C#
    class SampleDataClass
    {
        
        public int ProductID { get; set; }
        
        [FieldName("ProductName")]  // column name in the database
        public string ProductNameDifferentFromDbColumnName { get; set; }
    }
```
**1.0.7.2** - Changed versioning to allow bug fix references
	Fixed Merge method that was ignoring datatable schema name

**1.0.0.7** - Added Merge

**1.0.0.6** - Added Update