using System.Collections.Generic;
using System.Threading.Tasks;

namespace CameraDataWebApp.Services
{
    public interface IProjectsDownload
    {
        List<Project> GetProjectList(string database = "DocOverride");
    }
}