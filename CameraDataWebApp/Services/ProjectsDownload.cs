using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace CameraDataWebApp.Services
{
    public class ProjectsDownload : IProjectsDownload
    {

        public List<Project> GetProjectList(string database = "DocOverride")
        {
            
            string connString = @"Data Source=192.168.100.226\docstarter,1433;Initial Catalog=DocOverride;Persist Security Info=True;User ID=sa;Password=goC0p1ala;Pooling=False";
            List<Project> projects = new List<Project>();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand("SELECT CaseNo, ProjectName FROM [dbo].[Projects]", conn);
                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read()) 
                            {
                                Project project = new Project() { CaseId = $"{reader.GetString(0)}", Name = $"{reader.GetString(1)}" };
                                projects.Add(project);
                            } 
                        }
                        reader.Close();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                var projectlist = projects.OrderBy(x => x.Name).ToList();
                return projectlist;
            }
        }
    }
}
