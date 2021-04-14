using ArtUtils.Net.Core.Attributes;

namespace ArtUtils.Net.Unit.Tests
{
    class SampleDataClass
    {
        public const string TestFieldAttribute = "42";
        
        public int ProductId { get; set; }
        
        [FieldName(TestFieldAttribute)]
        public string ProductNameDifferentFromDbColumnName { get; set; }
    }
}
