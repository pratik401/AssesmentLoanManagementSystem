using System.Data.SqlClient;

namespace dao
{
    public static class DBUtil
    {
        public static string GetDBConn()
        {
            // Replace this with your actual connection string.
            return "Data Source=.\\SQLEXPRESS;Initial Catalog=LoanManagementDB;Integrated Security=True;TrustServerCertificate=True";
        }
    }
}

