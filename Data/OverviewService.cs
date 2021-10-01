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
    public class OverviewService
    {
        /*
        Récupère le nombre d'applications par seuil de danger (critique, avertissement, non operationnel)
        */
        public Task<List<Overview>> GetAppBySeverityAsync(string software)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            //Specifie les informations de connexion à la base de données SQL.
            builder.DataSource = "SRVJIRA\\SQLJIRA";
            builder.UserID = "Orion";
            builder.Password = "orionrbi092021";
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
                            //Instancie des objets du modele DysfunctionalHardware à partir des données recuperées par la requête SQL
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
        Récupère le nombre de serveurs par seuil de danger (critique, avertissement, non operationnel)
        */
        public Task<List<Overview>> GetNodeBySeverityAsync()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = CommonClass.credentials["server"];
            builder.UserID = CommonClass.credentials["user"];
            builder.Password = CommonClass.credentials["pwd"];
            builder.InitialCatalog = CommonClass.credentials["database"];


            string sql = "SELECT Status,COUNT(Status) FROM Node WHERE Status <> 1 GROUP BY Status ORDER BY Status DESC;";
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

        public Task<List<Overview>> GetSeverityByCustomerAsync(string CodeClient)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "SRVJIRA\\SQLJIRA";
            builder.UserID = "Orion";
            builder.Password = "orionrbi092021";
            builder.InitialCatalog = "OrionSQL";


            string sql = $"SELECT Status FROM Node WHERE CodeClient = '{CodeClient}' GROUP BY Status ORDER BY Status DESC;";
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
                                });
                            }
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






