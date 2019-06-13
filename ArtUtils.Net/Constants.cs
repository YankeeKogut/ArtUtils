using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtUtils.Net
{
    public class Constants
    {
        public const string ErrWrongDataType =
            "The given value of type String from the data source cannot be converted to type int of the specified target column";

        public const string ErrorMessageFixColumnsOrder =
            "Underlying data type error. Can be caused by mismatch in column order between table and entity object";
    }
}
