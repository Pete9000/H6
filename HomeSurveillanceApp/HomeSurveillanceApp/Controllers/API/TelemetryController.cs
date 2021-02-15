using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HomeSurveillanceApp;
using HomeSurveillanceApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace HomeSurveillanceApp.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class TelemetryController : ControllerBase
    {
        private readonly HomeSurveillanceDBContext _context;

        public TelemetryController(HomeSurveillanceDBContext context)
        {
            _context = context;
        }

        // GET: api/Telemetry
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Telemetry>>> GetTelemetrys()
        {
            return await _context.Telemetrys.ToListAsync();
        }

        // GET: api/Telemetry/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Telemetry>> GetTelemetry(int id)
        {
            var telemetry = await _context.Telemetrys.FindAsync(id);

            if (telemetry == null)
            {
                return NotFound();
            }

            return telemetry;
        }

        // PUT: api/Telemetry/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTelemetry(int id, Telemetry telemetry)
        {
            if (id != telemetry.TelemetryId)
            {
                return BadRequest();
            }

            _context.Entry(telemetry).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TelemetryExists(id))
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

        // POST: api/Telemetry
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Telemetry>> PostTelemetry(Telemetry telemetry)
        {
            _context.Telemetrys.Add(telemetry);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTelemetry", new { id = telemetry.TelemetryId }, telemetry);
        }

        // DELETE: api/Telemetry/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTelemetry(int id)
        {
            var telemetry = await _context.Telemetrys.FindAsync(id);
            if (telemetry == null)
            {
                return NotFound();
            }

            _context.Telemetrys.Remove(telemetry);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TelemetryExists(int id)
        {
            return _context.Telemetrys.Any(e => e.TelemetryId == id);
        }
    }
}
