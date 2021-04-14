using System;

namespace ArtUtils.Net.Core.Attributes
{
    public class SchemaName : Attribute
    {
        public SchemaName(string name)
        {
            Name = name;
        }
        public string Name { get; }
    }
}