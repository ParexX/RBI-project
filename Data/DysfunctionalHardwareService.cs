using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using BlazorSupervisionRBI;

namespace BlazorSupervisionRBI.Data
{
    public class DysfunctionalHardwareService
    {
        public Task<List<DysfunctionalHardware>> GetDysfunctionalHardwareAsync()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "SRVJIRA\\SQLJIRA";
            builder.UserID = "Orion";
            builder.Password = "orionrbi";
            builder.InitialCatalog = "OrionSQL";


            string sql = $"SELECT HardwareInfoID, C.CategoryName,N.NodeName,HI.LastMessage, HI.DetailsUrl FROM Hardware RIGHT JOIN HardwareInfo HI ON HI.ID = HardwareInfoID";
            sql+=$" LEFT JOIN Node N ON NodeID = N.ID LEFT JOIN Category C ON CategoryID = C.ID WHERE HI.Status = 17 ORDER BY NodeName";
            try
            {
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            List<DysfunctionalHardware> items = new List<DysfunctionalHardware>();
                            while (reader.Read())
                            {
                                items.Add(new DysfunctionalHardware
                                {
                                    hardwareInfoID = reader.GetInt32(0),
                                    categoryName = reader.GetString(1),
                                    nodeName = reader.GetString(2),
                                    alertMessage = reader.GetString(3),
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