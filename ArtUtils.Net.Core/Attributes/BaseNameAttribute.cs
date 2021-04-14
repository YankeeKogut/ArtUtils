using System;

namespace ArtUtils.Net.Core.Attributes
{
    public class BaseNameAttribute : Attribute
    {
        public BaseNameAttribute(string name)
        {
            Name = name;
        }
        public string Name { get; }
    }
}