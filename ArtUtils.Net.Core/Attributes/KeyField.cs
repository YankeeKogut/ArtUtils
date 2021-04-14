using System;

namespace ArtUtils.Net.Core.Attributes
{
    public class KeyField : Attribute
    {
        public KeyField(string name)
        {
            Name = name;
        }
        public string Name { get; }
    }
}