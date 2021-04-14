using System.Collections.Generic;
using System.Data.SqlClient;

namespace ArtUtils.Net.Interfaces
{
    public interface IArtSql
    {
        void DeleteTree<T>(IEnumerable<T> listWithChildObjects, SqlConnection sqlConnection);
    }
}