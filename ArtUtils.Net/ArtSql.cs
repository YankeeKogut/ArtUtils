using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using ArtUtils.Net.Core;
using ArtUtils.Net.Core.Attributes;
using ArtUtils.Net.Core.Classes;
using ArtUtils.Net.Core.Exceptions;
using ArtUtils.Net.Interfaces;

namespace ArtUtils.Net
{
    public class ArtSql : IArtSql
    {
        public void DeleteTree<T>(List<T> listWithChildObjects, SqlConnection sqlConnection)
        {
            var validationResult = VerifyTablesAttributes(listWithChildObjects);
            if (!validationResult.Valid)
            {
                throw new TableAttributesException(Constants.ErrorTableAttributesMissing,
                    new Exception(string.Join(Environment.NewLine, validationResult.Errors)));
            }

            throw new NotImplementedException();
        }

        private ValidationResult VerifyTablesAttributes<T>(List<T> listWithChildObjects)
        {

            var result = new ValidationResult();

            var tableNameValidation = Validator.VerifyTableName(listWithChildObjects);
            MergeValidations(tableNameValidation, result);

            var keyFieldValidation = Validator.VerifyAttributeOnPropertiesPresent<KeyField>(listWithChildObjects, Constants.ErrorKeyFieldAttributeMissing);
            MergeValidations(keyFieldValidation, result);


            throw new NotImplementedException();
        }

        private static void MergeValidations(ValidationResult newValidation, ValidationResult mergeToValidation)
        {
            if (newValidation.Valid) return;
            
            mergeToValidation.Valid = false;
            mergeToValidation.Errors.AddRange(newValidation.Errors);
        }
    }
}
