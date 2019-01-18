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
                                Project project = new Project() { caseId = $"{reader.GetString(0)}", name = $"{reader.GetString(1)}" };
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
                var projectlist = projects.OrderBy(x => x.name).ToList();
                return projectlist;
            }
        }
    }
}
