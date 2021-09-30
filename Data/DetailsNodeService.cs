using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using BlazorSupervisionRBI;
using BlazorSupervisionRBI.Shared;
namespace BlazorSupervisionRBI.Data
{
    public class DetailsNodeService
    {
        public Task<List<DetailsNode>> GetDetailsNodeBySeverityAsync(int nodeStatus)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "SRVJIRA\\SQLJIRA";
            builder.UserID = "Orion";
            builder.Password = "orionrbi092021";
            builder.InitialCatalog = "OrionSQL";


            string sql = $"SELECT NodeName, CodeClient, CodeCS, Status, DetailsUrl FROM Node  WHERE Status ={nodeStatus} ORDER BY NodeName;";
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
                                    detailsUrl = reader.GetString(4)

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

        public Task<List<DetailsNode>> GetDetailsNodeAsync()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = CommonClass.credentials["server"];
            builder.UserID = CommonClass.credentials["user"];
            builder.Password = CommonClass.credentials["pwd"];
            builder.InitialCatalog = CommonClass.credentials["database"];


            string sql = $"SELECT CodeClient, NodeName, CodeCS, Status, DetailsUrl FROM Node ORDER BY CodeClient, Status DESC, NodeName;";
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
                                    codeClient = reader.GetString(0),
                                    nodeName = reader.GetString(1),
                                    codeCS = reader.GetString(2),
                                    nodeStatus = reader.GetInt32(3),
                                    detailsUrl = reader.GetString(4)

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