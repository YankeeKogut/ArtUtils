using ArtUtils.Net.Attributes;

namespace ArtUtils.Net.Integration.Tests
{
    class SampleDataClass
    {
        
        public int ProductID { get; set; }
        
        [FieldName("ProductName")]
        public string ProductNameDifferentFromDbColumnName { get; set; }
    }
}
