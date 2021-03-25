using System;

namespace ArtUtils.Net.Attributes
{
    public class FieldName : Attribute
    {
        public FieldName(string name)
        {
            Name = name;
        }
        public string Name { get; }
    }
}