using Npgsql;

namespace BPOBackend
{
    internal class ConnectionDao
    {
        private static ConnectionDao instance = null;
        public static ConnectionDao Instance
        {
            get
            {
                return instance ?? new ConnectionDao();
            }
        }
        public NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection("Server=localhost;Port=5432;Database=newdb;User Id=postgres;Password=Pass!234");
        }
    }
}
