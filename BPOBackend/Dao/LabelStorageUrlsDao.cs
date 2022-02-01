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
            using (var connection = ConnectionDao.Instance.GetConnection())
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
        public List<LabelStorageUrl> GetUrlsByParameters(String userId,Int32 groupId)
        {
            List<LabelStorageUrl> result = new List<LabelStorageUrl>();
            using (var connection = ConnectionDao.Instance.GetConnection())
            {
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand("get_LabelStorageUrlsByUserAndGroupId", connection);
                command.Parameters.AddWithValue(new NpgsqlParameter("userId", DbType.String)).Value = userId;
                command.Parameters.AddWithValue(new NpgsqlParameter("groupId", DbType.Int32)).Value = groupId;
                command.CommandType = CommandType.StoredProcedure;
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(new LabelStorageUrl
                    {
                        Url = reader["url"].ToString(),
                        TrackingNumber= reader["trackingNumber"].ToString(),
                        Format= reader["format"].ToString(),
                        BatchNumber= Convert.ToInt64(reader["batchNumber"]),
                    });
                }
            }
            return result;
        }

    }
}
