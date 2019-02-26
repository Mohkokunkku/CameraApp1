using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CameraDataWebApp.Services;
using CameraDataWebApp.WordProcessing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CameraDataWebApp.Controllers
{
    [Route("photoapi/")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IPhotoUpload _services;
        private readonly ObservationContext _context;

        public UploadController(IPhotoUpload services, ObservationContext context)
        {
            _services = services;
            _context = context;
        }

        //public ActionResult UpdateDatabase()
        //{

        //}

        [HttpPost()]
        [Route("AddObservations")]
        public async Task<IActionResult> Post(List<IFormFile> items)
        {
            //var observations = _services.AddPhotos(items);
            long size = items.Sum(f => f.Length);
            var filePath = Environment.CurrentDirectory;
            string imagedatafolder = "\\imagedata\\";
            if (Directory.Exists($"{filePath}{imagedatafolder}") != true)
            {
                Directory.CreateDirectory($"{filePath}{imagedatafolder}");
            }
            //Täällä täytyy jotenkin käsitellä filepath mutta miksi Postmanissa ei tarvitse? 
            try
            {
                foreach (var formFile in items) //foreach (var formFile in items)
                {
                    //filePath = $"{filePath}{imagedatafolder}{formFile.FileName}";
                    //OTTAA FORM-FILESTA TIEDOSTONIMEN
                    var FileName = Path.GetFileName(formFile.FileName);
                    if (formFile.Length > 0)
                    {
                        using (var stream = new FileStream($"{filePath}{imagedatafolder}{FileName}", FileMode.Create))
                        {

                            await formFile.CopyToAsync(stream);
                            var entity = _context.Observations.FirstOrDefault(item => item.imageuri == formFile.FileName);
                            if (entity != null)
                            {
                                entity.imageuri = $"{filePath}{imagedatafolder}{FileName}";
                                _context.Observations.Update(entity);
                                await _context.SaveChangesAsync();
                            }
                        }
                    }
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

            //if (observations == null)
            //{
            //    return NotFound();
            //}
            return Ok(new { count = items.Count(), size, filePath });
        }

        [HttpGet()]
        [Route("GetImageFile/{id}")]
        public async Task<IActionResult> GetImageFile([FromRoute] int id)
        {
            Observation observation = await _context.Observations.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (System.IO.File.Exists(observation.imageuri))
            {
                return new PhysicalFileResult(observation.imageuri, "image/jpeg");
            }
            else
            {
                return NoContent();
            }
        }

        [HttpGet()]
        [Route("GetVisitDocument/{id}")]
        public async Task<IActionResult> GetVisitDocument([FromRoute] int id)
        {
            MonitoringVisit monitoringVisit = await _context.MonitoringVisits.Where(x => x.Id == id).Include(x => x.observations).FirstOrDefaultAsync();
            MonitoringVisitDocument monitoringVisitDocument = new MonitoringVisitDocument(monitoringVisit);
            string docxFile = monitoringVisitDocument.GetMonitoringVisitDocument();
            if (System.IO.File.Exists(docxFile))
            {
                return new PhysicalFileResult(docxFile, "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
            }
            else
            {
                return NoContent();
            }
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