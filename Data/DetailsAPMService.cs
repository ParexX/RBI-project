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
    public class DetailsAPMService
    {
        public Task<List<DetailsAPM>> GetDetailsAPMAsync(int appStatus, string software)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = CommonClass.credentials["server"];
            builder.UserID = CommonClass.credentials["user"];
            builder.Password = CommonClass.credentials["pwd"];
            builder.InitialCatalog = CommonClass.credentials["database"];


            string sql = "SELECT N.CodeClient,N.CodeCS,N.NodeName, N.Status, AT.ApplicationTemplateName, A.ID, A.DetailsUrl";
            sql+=" FROM Application A INNER JOIN Node N ON A.NodeID = N.ID LEFT JOIN ApplicationTemplate AT ON ";
            sql+="A.ApplicationTemplateID = AT.ID LEFT JOIN Tag T ON AT.ID = T.TemplateID ";
            sql+=$"WHERE A.Status = {appStatus} AND T.TagName = '{software}';" ;
            try
            {
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                       
                        using (SqlDataReader reader = command.ExecuteReader())
                        { 
                            List<DetailsAPM> items = new List<DetailsAPM>();
                            while (reader.Read())
                            {
                                items.Add(new DetailsAPM
                                {
                                    clientName = reader.GetString(0),
                                    csCode = reader.GetString(1),
                                    nodeName = reader.GetString(2),
                                    nodeStatus = reader.GetInt32(3),
                                    applicationName = reader.GetString(4),
                                    applicationID = reader.GetInt32(5),
                                    detailsUrl = reader.GetString(6)
                                });
                            }
                            connection.Close();
                            return Task.FromResult(items);
                        }
                    }
                   
                }
            }
            catch(SqlException e){
                Console.WriteLine($"{e.Message}\n{e.StackTrace}");
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}");
                return null;
            }
        }

        public Task<List<DetailsComponent>> GetDetailsComponentAsync(int applicationID)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "SRVJIRA\\SQLJIRA";
            builder.UserID = "Orion";
            builder.Password = "orionrbi092021";
            builder.InitialCatalog = "OrionSQL";


            string sql = $"SELECT ComponentName, SeverityStatus, ApplicationID, Component.DetailsUrl FROM Component LEFT JOIN Application A ON A.ID = ApplicationID WHERE ApplicationID={applicationID} AND SeverityStatus <>1 ORDER BY SeverityStatus DESC ";
            try
            {
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                       
                        using (SqlDataReader reader = command.ExecuteReader())
                        { 
                            List<DetailsComponent> items = new List<DetailsComponent>();
                            while (reader.Read())
                            {
                                items.Add(new DetailsComponent
                                {
                                    componentName = reader.GetString(0),
                                    componentStatus = reader.GetInt32(1),
                                    applicationId = reader.GetInt32(2),
                                    detailsUrl = reader.GetString(3)
                                });
                            }
                            connection.Close();
                            return Task.FromResult(items);
                        }
                    }
                   
                }
            }
            catch(SqlException e){
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






