using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;
using ArtUtils.Net.Core.Attributes;
using ArtUtils.Net.Core.Classes;
using ArtUtils.Net.Core.Exceptions;
using ArtUtils.Net.Interfaces;

namespace ArtUtils.Net
{
    public class ArtSql : IArtSql
    {
        public void DeleteTree<T>(IEnumerable<T> listWithChildObjects, SqlConnection sqlConnection)
        {
            var validationResult = VerifyTablesAttributes(listWithChildObjects);
            if (!validationResult.Valid)
            {
                throw new TableAttributesException(Constants.ErrorTableAttributesMissing,
                    new Exception(string.Join(Environment.NewLine, validationResult.Errors)));
            }

            throw new NotImplementedException();
        }

        private ValidationResult VerifyTablesAttributes<T>(IEnumerable<T> listWithChildObjects)
        {
            throw new NotImplementedException();
        }

        internal static string GetBaseNameAttributeName<T>(PropertyInfo info) where T : BaseNameAttribute
        {
            string result = null;
            foreach (var attr in Attribute.GetCustomAttributes(info))
            {
                if (!(attr is T fn)) continue;
                result = fn.Name;
                break;
            }

            return result;
        }
    }
}
