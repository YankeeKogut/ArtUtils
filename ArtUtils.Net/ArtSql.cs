using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    }
}
