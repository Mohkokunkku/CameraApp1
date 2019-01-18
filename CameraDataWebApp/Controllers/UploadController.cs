using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CameraDataWebApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace CameraDataWebApp.Controllers
{
    [Route("photoapi/")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IPhotoUpload _services;

        public UploadController(IPhotoUpload services)
        {
            _services = services;
        }

        [HttpPost]
        [Route("AddObservation")]
        public ActionResult<Observation> AddPhotos(Observation items)
        {
            var observations = _services.AddPhotos(items);
            
            if (observations == null)
            {
                return NotFound();
            }
            return observations;
        }

        [HttpGet]
        [Route("GetObservations")]
        public ActionResult<Dictionary<string, Observation>> GetObservations()
        {
            var observations = _services.GetObservations();

            if (observations.Count() == 0)
            {
                return NotFound();
            }
            return observations;
        }
    }
}