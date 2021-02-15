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
    public class IOUnitController : ControllerBase
    {
        private readonly HomeSurveillanceDBContext _context;

        public IOUnitController(HomeSurveillanceDBContext context)
        {
            _context = context;
        }

        // GET: api/IOUnit
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IOUnit>>> GetIOUnits()
        {
            return await _context.IOUnits.ToListAsync();
        }

        // GET: api/IOUnit/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IOUnit>> GetIOUnit(int id)
        {
            var iOUnit = await _context.IOUnits.FindAsync(id);

            if (iOUnit == null)
            {
                return NotFound();
            }

            return iOUnit;
        }

        // PUT: api/IOUnit/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIOUnit(int id, IOUnit iOUnit)
        {
            if (id != iOUnit.IOUnitId)
            {
                return BadRequest();
            }

            _context.Entry(iOUnit).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IOUnitExists(id))
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

        // POST: api/IOUnit
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<IOUnit>> PostIOUnit(IOUnit iOUnit)
        {
            _context.IOUnits.Add(iOUnit);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetIOUnit", new { id = iOUnit.IOUnitId }, iOUnit);
        }

        // DELETE: api/IOUnit/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIOUnit(int id)
        {
            var iOUnit = await _context.IOUnits.FindAsync(id);
            if (iOUnit == null)
            {
                return NotFound();
            }

            _context.IOUnits.Remove(iOUnit);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool IOUnitExists(int id)
        {
            return _context.IOUnits.Any(e => e.IOUnitId == id);
        }
    }
}
