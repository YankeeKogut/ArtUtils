using ArtUtils.Net.Core.Attributes;

namespace ArtUtils.Net.Integration.Tests
{
    class SampleDataClass
    {
        
        public int ProductId { get; set; }
        
        [FieldName("ProductName")]
        public string ProductNameDifferentFromDbColumnName { get; set; }
    }
}
