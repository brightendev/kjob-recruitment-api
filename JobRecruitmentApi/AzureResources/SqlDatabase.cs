using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace JobRecruitmentApi.AzureResources
{
    static class SqlDatabase
    {
        public static async Task<string> Insert(string sql) => await NoQuery(sql);
        public static async Task<string> Update(string sql) => await NoQuery(sql);
        public static async Task<string> Delete(string sql) => await NoQuery(sql);
        public static async Task<string> Select(string sql) => await Query(sql);

        private static async Task<string> NoQuery(string sql)
        {
            string connectionString = Environment.GetEnvironmentVariable("sql_write");
            try
            {
                if (sql == null || sql.Equals(""))
                {
                    throw new Exception("sql query is empty");
                }
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(sql, conn))
                    {
                        await conn.OpenAsync();
                        command.ExecuteNonQuery();
                        conn.Close();
                        return "OK";

                    }
                }
            }
            catch (Exception ex)
            {
                return $"ERROR:{ex.Message}";
            }
        }

        private static async Task<string> Query(string sql)
        {
            string connectionString = Environment.GetEnvironmentVariable("sql_read");
            try
            {
                if (sql == null || sql.Equals(""))
                {
                    throw new Exception("sql query is empty");
                }
                StringBuilder sb = new StringBuilder();
                StringWriter sw = new StringWriter(sb);
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(sql, conn))
                    {
                        await conn.OpenAsync();
                        SqlDataReader reader = command.ExecuteReader();
                        using (JsonWriter jsonWriter = new JsonTextWriter(sw))
                        {
                            jsonWriter.WriteStartArray();
                            while (reader.Read())
                            {
                                jsonWriter.WriteStartObject();
                                int fields = reader.FieldCount;
                                for (int i = 0; i < fields; i++)
                                {
                                    jsonWriter.WritePropertyName(reader.GetName(i));
                                    jsonWriter.WriteValue(reader[i]);
                                }
                                jsonWriter.WriteEndObject();
                            }
                            jsonWriter.WriteEndArray();
                            reader.Close();
                            conn.Close();
                            return sw.ToString();
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                return $"ERROR:{ex.Message}";
            }
        }
    }
}
