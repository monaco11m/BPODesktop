using Npgsql;
using System.Collections.Generic;
using System.Data;

namespace BPOBackend
{
    internal class AspNetUserDao
    {
        private static AspNetUserDao instance = null;
        public static AspNetUserDao Instance
        {
            get
            {
                return instance ?? new AspNetUserDao();
            }
        }
        public List<AspNetUser> GetAspNetUser()
        {
            List<AspNetUser> result = new List<AspNetUser>();
            using (var connection = ConnectionDao.Instance.GetConnection())
            {
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand("get_Users", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(new AspNetUser { 
                        Id=reader["Id"].ToString(), 
                        UserName=reader["UserName"].ToString() 
                    });
                }
            }
            return result;
        }
    }
}
