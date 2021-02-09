using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HomeSurveillanceAPI;
using HomeSurveillanceAPI.Models;
using Microsoft.AspNetCore.Authorization;

namespace HomeSurveillanceAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class MicroControllerController : ControllerBase
    {
        private readonly HomeSurveillanceDBContext _context;

        public MicroControllerController(HomeSurveillanceDBContext context)
        {
            _context = context;
        }

        // GET: api/MicroControllers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MicroController>>> GetMicroControllers()
        {
            return await _context.MicroControllers.ToListAsync();
        }

        // GET: api/MicroControllers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MicroController>> GetMicroController(int id)
        {
            var microController = await _context.MicroControllers.FindAsync(id);

            if (microController == null)
            {
                return NotFound();
            }

            return microController;
        }

        // PUT: api/MicroControllers/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMicroController(int id, MicroController microController)
        {
            if (id != microController.MicroControllerId)
            {
                return BadRequest();
            }

            _context.Entry(microController).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MicroControllerExists(id))
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

        // POST: api/MicroControllers
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<MicroController>> PostMicroController(MicroController microController)
        {
            _context.MicroControllers.Add(microController);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMicroController", new { id = microController.MicroControllerId }, microController);
        }

        // DELETE: api/MicroControllers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<MicroController>> DeleteMicroController(int id)
        {
            var microController = await _context.MicroControllers.FindAsync(id);
            if (microController == null)
            {
                return NotFound();
            }

            _context.MicroControllers.Remove(microController);
            await _context.SaveChangesAsync();

            return microController;
        }

        private bool MicroControllerExists(int id)
        {
            return _context.MicroControllers.Any(e => e.MicroControllerId == id);
        }
    }
}
