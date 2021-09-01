using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using BlazorSupervisionRBI;

namespace BlazorSupervisionRBI.Data
{
    public class OverviewService
    {
        public Task<List<Overview>> GetAppBySeverityAsync(string software)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "SRVJIRA\\SQLJIRA";
            builder.UserID = "Orion";
            builder.Password = "orionrbi";
            builder.InitialCatalog = "OrionSQL";


            string sql = $"SELECT  Status, COUNT(Status) FROM Application A LEFT JOIN ApplicationTemplate AT ON A.ApplicationTemplateID = AT.ID LEFT JOIN Tag T ON AT.ID = T.TemplateID WHERE T.TagName = '{software}' AND Status<> 1 GROUP BY Status ORDER BY Status DESC; ";
            try
            {
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            List<Overview> items = new List<Overview>();
                            while (reader.Read())
                            {
                                items.Add(new Overview
                                {
                                    severity = reader.GetInt32(0),
                                    countSeverity = reader.GetInt32(1)
                                });
                            }
                            return Task.FromResult(items);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}");
                return null;
            }
        }

        public Task<List<Overview>> GetNodeBySeverityAsync()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "SRVJIRA\\SQLJIRA";
            builder.UserID = "Orion";
            builder.Password = "orionrbi";
            builder.InitialCatalog = "OrionSQL";


            string sql = $"SELECT Status,COUNT(Status) FROM Node WHERE Status <> 1 GROUP BY Status ORDER BY Status DESC;";
            try
            {
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            List<Overview> items = new List<Overview>();
                            while (reader.Read())
                            {
                                items.Add(new Overview
                                {
                                    severity = reader.GetInt32(0),
                                    countSeverity = reader.GetInt32(1)
                                });
                            }
                            return Task.FromResult(items);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}");
                return null;
            }
        }
    }
}






