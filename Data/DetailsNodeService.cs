using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using BlazorSupervisionRBI;

namespace BlazorSupervisionRBI.Data
{
    public class DetailsNodeService
    {
        public Task<List<DetailsNode>> GetDetailsNodeAsync(int nodeStatus)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "SRVJIRA\\SQLJIRA";
            builder.UserID = "Orion";
            builder.Password = "orionrbi";
            builder.InitialCatalog = "OrionSQL";


            string sql = $"SELECT NodeName, CodeClient, CodeCS, Status FROM Node  WHERE Status ={nodeStatus};";
            try
            {
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            List<DetailsNode> items = new List<DetailsNode>();
                            while (reader.Read())
                            {
                                items.Add(new DetailsNode
                                {
                                    nodeName = reader.GetString(0),
                                    codeClient = reader.GetString(1),
                                    codeCS = reader.GetString(2),
                                    nodeStatus = reader.GetInt32(3),

                                });
                            }
                            connection.Close();
                            return Task.FromResult(items);
                        }
                    }

                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}");
                return null;
            }
        }
    }
}