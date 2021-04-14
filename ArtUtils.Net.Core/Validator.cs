using System.Linq;
using ArtUtils.Net.Core.Attributes;
using ArtUtils.Net.Core.Classes;

namespace ArtUtils.Net.Core
{
    public static class Validator
    {
        public static ValidationResult VerifyTableName(object ObjectToCheck)
        {
            var result = new ValidationResult();
            var dnAttribute = ObjectToCheck.GetType().GetCustomAttributes(
                typeof(TableName), true).FirstOrDefault() as TableName;

            if (dnAttribute != null && !string.IsNullOrEmpty(dnAttribute.Name))
            {
                result.Valid = true;
            }
            else
            {
                result.Errors.Add(Constants.ErrorTableAttributesMissing);
            }
            return result;

        }
    }
}
