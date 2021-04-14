using System.Linq;
using ArtUtils.Net.Core.Attributes;
using ArtUtils.Net.Core.Classes;

namespace ArtUtils.Net.Core
{
    public static class Validator
    {
        public static ValidationResult VerifyTableName(object objectToCheck)
        {
            var result = new ValidationResult();

            if (objectToCheck.GetType().GetCustomAttributes(
                typeof(TableName), true).FirstOrDefault() is TableName dnAttribute && !string.IsNullOrEmpty(dnAttribute.Name))
            {
                result.Valid = true;
            }
            else
            {
                result.Errors.Add(Constants.ErrorTableNameAttributeMissing);
            }
            return result;
        }

        public static ValidationResult VerifyAttributeOnPropertiesPresent<T>(object objectToCheck, string errorMessage) where T: BaseNameAttribute
        {
            var result = new ValidationResult();

            var type = objectToCheck.GetType();
            var properties = type.GetProperties();

            foreach (var propertyInfo in properties)
            {
                if (propertyInfo.GetCustomAttributes(typeof(T), true)
                    .FirstOrDefault() is T dnAttribute && !string.IsNullOrEmpty(dnAttribute.Name))
                {
                    result.Valid = true;
                    break;
                }
            }

            if (!result.Valid)
            {
                result.Errors.Add(errorMessage);
            }
            return result;
        }
    }
}
