using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;

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
        public List<string> GetUrls()
        {
            List<string> result = new List<string>();
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
        public List<LabelStorageUrl> GetUrlsByParameters(string userId,int groupId,DateTime startDate)
        {
            List<LabelStorageUrl> result = new List<LabelStorageUrl>();
            using (var connection = ConnectionDao.Instance.GetConnection())
            {
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand("get_LabelStorageUrlsByUserAndGroupId", connection);
                command.Parameters.AddWithValue(new NpgsqlParameter("userId", DbType.String)).Value = userId;
                command.Parameters.AddWithValue(new NpgsqlParameter("groupId", DbType.Int32)).Value = groupId;
                command.Parameters.AddWithValue(new NpgsqlParameter("startDate", NpgsqlDbType.Date)).Value = new NpgsqlDate(startDate);
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
                        ItemQuantity= Convert.ToInt32(reader["itemQuantity"]),
                        ItemSku= reader["itemSku"].ToString(),
                    });
                }
            }
            return result;
        }

    }
}
