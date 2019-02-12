using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CameraDataWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ObservationsApiController : ControllerBase
    {
        private readonly ObservationContext _context;

        public ObservationsApiController(ObservationContext context)
        {
            _context = context;
        }

        // GET: api/ObservationsApi
        [HttpGet]
        public ActionResult<IEnumerable<Observation>> GetObservations()
        {
            if (_context.Observations.Count() == 0)
            {
                return NotFound("No data in database");
            }
            return _context.Observations;
        }

        // GET: api/ObservationsApi/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetObservation([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var observation = await _context.Observations.FindAsync(id);

            if (observation == null)
            {
                return NotFound();
            }

            return Ok(observation);
        }

        // PUT: api/ObservationsApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutObservation([FromRoute] int id, [FromBody] Observation observation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != observation.Id)
            {
                return BadRequest();
            }

            _context.Entry(observation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ObservationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ObservationsApi
        [HttpPost]
        public async Task<IActionResult> PostObservation([FromBody] List<Observation> observations) //
        {

            try
            {
                List<string> images = new List<string>();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                Observation observ = observations.FirstOrDefault();
                MonitoringVisit monitoringVisit = _context.MonitoringVisits.Where(x => x.visitguid == observ.visitguid).FirstOrDefault();
                List<Observation> lstObservations = new List<Observation>();
                foreach (var observation in observations)
                {
                    if (_context.Observations.Contains(observation) == false)
                    {
                        lstObservations.Add(observation);
                        //observation.imageuri = string.Empty;
                        //_context.Observations.Add(observation);
                        images.Add(observation.imageuri);
                    }
                }
                if (lstObservations.Count > 0)
                {
                    monitoringVisit.observations = lstObservations;
                    _context.MonitoringVisits.Update(monitoringVisit);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    return NotFound("Tietoja ei saatu päivitettyä");
                }



                return Ok(images); //PALAUTTAA LISTAN TIETOKANTAAN TALLENNETUISTA KUVISTA
                                   //return CreatedAtAction("GetObservation", observations.ToString());

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }



        // DELETE: api/ObservationsApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteObservation([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var observation = await _context.Observations.FindAsync(id);
            if (observation == null)
            {
                return NotFound();
            }

            _context.Observations.Remove(observation);
            await _context.SaveChangesAsync();

            return Ok(observation);
        }

        private bool ObservationExists(int id)
        {
            return _context.Observations.Any(e => e.Id == id);
        }
    }
}