using System;

namespace ArtUtils.Net.Core.Attributes
{
    public class TableName : Attribute
    {
        public TableName(string name)
        {
            Name = name;
        }
        public string Name { get; }
    }
}