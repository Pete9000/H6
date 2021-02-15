using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HomeSurveillanceApp;
using HomeSurveillanceApp.Models;

namespace HomeSurveillanceApp.Controllers
{
    public class TelemetriesController : Controller
    {
        private readonly HomeSurveillanceDBContext _context;

        public TelemetriesController(HomeSurveillanceDBContext context)
        {
            _context = context;
        }

        // GET: Telemetries
        public async Task<IActionResult> Index()
        {
            var homeSurveillanceDBContext = _context.Telemetrys.Include(t => t.IOUnit);
            return View(await homeSurveillanceDBContext.ToListAsync());
        }

        // GET: Telemetries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var telemetry = await _context.Telemetrys
                .Include(t => t.IOUnit)
                .FirstOrDefaultAsync(m => m.TelemetryId == id);
            if (telemetry == null)
            {
                return NotFound();
            }

            return View(telemetry);
        }

        // GET: Telemetries/Create
        public IActionResult Create()
        {
            ViewData["IOUnitId"] = new SelectList(_context.IOUnits, "IOUnitId", "IOUnitId");
            return View();
        }

        // POST: Telemetries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TelemetryId,ActivityTimeStamp,IOUnitId")] Telemetry telemetry)
        {
            if (ModelState.IsValid)
            {
                _context.Add(telemetry);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IOUnitId"] = new SelectList(_context.IOUnits, "IOUnitId", "IOUnitId", telemetry.IOUnitId);
            return View(telemetry);
        }

        // GET: Telemetries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var telemetry = await _context.Telemetrys.FindAsync(id);
            if (telemetry == null)
            {
                return NotFound();
            }
            ViewData["IOUnitId"] = new SelectList(_context.IOUnits, "IOUnitId", "IOUnitId", telemetry.IOUnitId);
            return View(telemetry);
        }

        // POST: Telemetries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TelemetryId,ActivityTimeStamp,IOUnitId")] Telemetry telemetry)
        {
            if (id != telemetry.TelemetryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(telemetry);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TelemetryExists(telemetry.TelemetryId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IOUnitId"] = new SelectList(_context.IOUnits, "IOUnitId", "IOUnitId", telemetry.IOUnitId);
            return View(telemetry);
        }

        // GET: Telemetries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var telemetry = await _context.Telemetrys
                .Include(t => t.IOUnit)
                .FirstOrDefaultAsync(m => m.TelemetryId == id);
            if (telemetry == null)
            {
                return NotFound();
            }

            return View(telemetry);
        }

        // POST: Telemetries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var telemetry = await _context.Telemetrys.FindAsync(id);
            _context.Telemetrys.Remove(telemetry);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TelemetryExists(int id)
        {
            return _context.Telemetrys.Any(e => e.TelemetryId == id);
        }
    }
}
