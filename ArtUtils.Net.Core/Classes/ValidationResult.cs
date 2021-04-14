using System.Collections.Generic;

namespace ArtUtils.Net.Core.Classes
{
    public class ValidationResult
    {
        public bool Valid { set; get; } = false;

        public List<string> Errors { set; get; } = new List<string>();
    }
}
