using System;

namespace ArtUtils.Net.Core.Attributes
{
    public class ParentKeyField : Attribute
    {
        public ParentKeyField(string name)
        {
            Name = name;
        }
        public string Name { get; }
    }
}