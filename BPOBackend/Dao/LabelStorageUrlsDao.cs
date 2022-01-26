using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace BPOBackend
{
    internal class LabelStorageUrlsDao
    {
        private static LabelStorageUrlsDao instance = null;
        public static LabelStorageUrlsDao Instance
        {
            get
            {
                return instance ?? new LabelStorageUrlsDao();
            }
        }
        public List<String> GetUrls()
        {
            List<String> result = new List<String>();
            using (var connection = GetConnection())
            {
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand("get_LabelStorageUrls", connection);
                command.Parameters.AddWithValue(new NpgsqlParameter("id", DbType.Int32)).Value = 403479;
                command.CommandType = CommandType.StoredProcedure;
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(reader["Url"].ToString());
                }
            }
            return result;
        }
        private static NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection("Server=localhost;Port=5432;Database=newdb;User Id=postgres;Password=Pass!234");
        }
    }
}
