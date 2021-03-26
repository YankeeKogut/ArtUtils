using ArtUtils.Net.Attributes;

namespace ArtUtils.Net.Unit.Tests
{
    class SampleDataClass
    {
        public const string TestFieldAttribute = "42";
        
        public int ProductID { get; set; }
        
        [FieldName(TestFieldAttribute)]
        public string ProductNameDifferentFromDbColumnName { get; set; }
    }
}
