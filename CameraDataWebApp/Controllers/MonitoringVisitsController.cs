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
    public class MonitoringVisitsController : ControllerBase
    {
        private readonly ObservationContext _context;

        public MonitoringVisitsController(ObservationContext context)
        {
            _context = context;
        }

        // GET: api/MonitoringVisits
        [HttpGet]
        public IEnumerable<MonitoringVisit> GetMonitoringVisits()
        {
            return _context.MonitoringVisits;
        }

        // GET: api/MonitoringVisits/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMonitoringVisit([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var monitoringVisit = await _context.MonitoringVisits.FindAsync(id);

            if (monitoringVisit == null)
            {
                return NotFound();
            }

            return Ok(monitoringVisit);
        }

        // PUT: api/MonitoringVisits/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMonitoringVisit([FromRoute] int id, [FromBody] MonitoringVisit monitoringVisit)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != monitoringVisit.Id)
            {
                return BadRequest();
            }

            _context.Entry(monitoringVisit).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MonitoringVisitExists(id))
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

        // POST: api/MonitoringVisits
        [HttpPost]
        public async Task<IActionResult> PostMonitoringVisit([FromBody] MonitoringVisit monitoringVisit)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.MonitoringVisits.Add(monitoringVisit);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMonitoringVisit", new { id = monitoringVisit.Id }, monitoringVisit);
        }

        // DELETE: api/MonitoringVisits/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMonitoringVisit([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var monitoringVisit = await _context.MonitoringVisits.FindAsync(id);
            if (monitoringVisit == null)
            {
                return NotFound();
            }

            _context.MonitoringVisits.Remove(monitoringVisit);
            await _context.SaveChangesAsync();

            return Ok(monitoringVisit);
        }

        private bool MonitoringVisitExists(int id)
        {
            return _context.MonitoringVisits.Any(e => e.Id == id);
        }
    }
}