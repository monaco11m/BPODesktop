using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;

namespace BPOBackend
{
    internal class AutomateLabelsDao
    {
        private static AutomateLabelsDao instance = null;
        public static AutomateLabelsDao Instance
        {
            get
            {
                return instance ?? new AutomateLabelsDao();
            }
        }
        public List<AutomateLabel> GetIdsByUserIdAndDate(String userId,DateTime startDate,DateTime endDate)
        {
            List<AutomateLabel> result = new List<AutomateLabel>();
            using (var connection = ConnectionDao.Instance.GetConnection())
            {
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand("get_GroupIdsByUserAndDate", connection);
                command.Parameters.AddWithValue(new NpgsqlParameter("userId", DbType.String)).Value = userId;
                command.Parameters.AddWithValue(new NpgsqlParameter("startDate", NpgsqlDbType.Date)).Value = new NpgsqlDate(startDate);
                command.Parameters.AddWithValue(new NpgsqlParameter("endDate", NpgsqlDbType.Date)).Value = new NpgsqlDate(endDate);
                command.CommandType = CommandType.StoredProcedure;
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(new AutomateLabel
                    {
                        Id = Convert.ToInt64(reader["AutomateId"])
                    });
                }
            }
            return result;
        }
    }
}
