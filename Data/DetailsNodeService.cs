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
        /*
            Description : Recupere le detail des serveurs en fonction du statut
            Entree : Le statut
        */
        public Task<List<DetailsNode>> GetDetailsNodeBySeverityAsync(int nodeStatus)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            //Specifie les informations de connexion à la base de données SQL.
            builder.DataSource = CommonClass.credentials["server"];
            builder.UserID = CommonClass.credentials["user"];
            builder.Password = CommonClass.credentials["pwd"];
            builder.InitialCatalog = CommonClass.credentials["database"];


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
                            //Instancie des objets du modele DetailsAPM à partir des données recuperées par la requête SQL
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
        /*
            Descrption : Recupere l'etat 
        */
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