using System.Collections.Generic;
using System.Data.SqlClient;

namespace ArtUtils.Net.Interfaces
{
    public interface IArtSql
    {
        void DeleteTree<T>(List<T> listWithChildObjects, SqlConnection sqlConnection);
    }
}