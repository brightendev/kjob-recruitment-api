﻿using Newtonsoft.Json;
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
        public static async Task<string> NoQuery(string sql)
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
                Console.WriteLine(ex.Message);
                return $"ERROR:{ex.Message}";
            }
        }

        public static async Task<string> Query(string sql)
        {
            string connectionString = Environment.GetEnvironmentVariable("sql_read");
            try
            {
                if (sql == null || sql.Equals(""))
                {
                    throw new Exception("ERROR");
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
                Console.WriteLine(ex.Message);
                return "ERROR";
            }
        }

        public static async Task<string> QueryOne(string sql)
        {
            string connectionString = Environment.GetEnvironmentVariable("sql_read");
            try
            {
                if (sql == null || sql.Equals(""))
                {
                    throw new Exception("ERROR");
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
                            reader.Close();
                            conn.Close();
                            return sw.ToString();
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "ERROR";
            }
        }

        public static async Task<string> GetCount(string sql)
        {
            string result = "";
            try
            {
                string connectionString = Environment.GetEnvironmentVariable("sql_read");
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(sql, conn))
                    {
                        await conn.OpenAsync();
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            result = reader[0].ToString();
                        }
                        reader.Close();
                        conn.Close();
                        return result;
                    }                  
                }    
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "ERROR";
            }
        }

    }
}
