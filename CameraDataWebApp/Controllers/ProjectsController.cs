using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CameraDataWebApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CameraDataWebApp.Controllers
{
    //Tämän tarkoituksena olisi palauttaa projektilista DocStarterin kannasta --> on vielä testausvaiheessa
    [Route("projectsapi/")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectsDownload projects;

        public ProjectsController(IProjectsDownload projects)
        {
            this.projects = projects;
        }


        [HttpGet]
        [Route("GetProjects")]
        public ActionResult<List<Project>> GetProjectList()
        {
            List<Project> projectlist = projects.GetProjectList();
           // var projectlist2 = projectlist.GroupBy(x => x.name).Select(grp => grp.ToList()).ToList();
            if (projectlist == null)
            {
                return NotFound("Ei löydy");
            }
            return projectlist;
        }
    }
}