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
    public class DysfunctionalHardwareService
    {
        /*
        Description : Récupere le detail du materiel(s) defectueu(x) et le detail du serveur.
        */
        public Task<List<DysfunctionalHardware>> GetDysfunctionalHardwareAsync()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            //Specifie les informations de connexion à la base de données SQL.
            builder.DataSource = CommonClass.credentials["server"];
            builder.UserID = CommonClass.credentials["user"];
            builder.Password = CommonClass.credentials["pwd"];
            builder.InitialCatalog = CommonClass.credentials["database"];


            string sql = $"SELECT HardwareInfoID, C.CategoryName, N.NodeName, N.CodeClient, N.CodeCS, HI.LastMessage, HI.DetailsUrl FROM Hardware RIGHT JOIN HardwareInfo HI ON HI.ID = HardwareInfoID";
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
                            //Instancie des objets du modele DysfunctionalHardware à partir des données recuperées par la requête SQL
                            while (reader.Read())
                            {
                                items.Add(new DysfunctionalHardware
                                {
                                    hardwareInfoID = reader.GetInt32(0),
                                    categoryName = reader.GetString(1),
                                    nodeName = reader.GetString(2),
                                    cdiValdo = reader.GetString(3),
                                    csCode = reader.GetString(4),
                                    alertMessage = reader.GetString(5),
                                    detailsUrl = reader.GetString(6)
                                });
                            }
                            connection.Close();
                            return Task.FromResult(items);
                        }
                    }

                }
            }
            catch(SqlException e){//Affiche une erreur generée par la requête SQL
                Console.WriteLine($"{e.Message}\n{e.StackTrace}");
                return null;
            }
            catch(InvalidOperationException e){//Affiche une erreur de connexion
                Console.WriteLine("La connection est deja ouverte");
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}