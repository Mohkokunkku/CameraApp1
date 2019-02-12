using System;
using System.Collections.Generic;
using System.IO;
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