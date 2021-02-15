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
    public class IOUnitsController : Controller
    {
        private readonly HomeSurveillanceDBContext _context;

        public IOUnitsController(HomeSurveillanceDBContext context)
        {
            _context = context;
        }

        // GET: IOUnits
        public async Task<IActionResult> Index()
        {
            var homeSurveillanceDBContext = _context.IOUnits.Include(i => i.Device);
            return View(await homeSurveillanceDBContext.ToListAsync());
        }

        // GET: IOUnits/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var iOUnit = await _context.IOUnits
                .Include(i => i.Device)
                .FirstOrDefaultAsync(m => m.IOUnitId == id);
            if (iOUnit == null)
            {
                return NotFound();
            }

            return View(iOUnit);
        }

        // GET: IOUnits/Create
        public IActionResult Create()
        {
            ViewData["DeviceId"] = new SelectList(_context.Devices, "DeviceId", "MACAddress");
            return View();
        }

        // POST: IOUnits/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IOUnitId,Enabled,Name,DeviceId")] IOUnit iOUnit)
        {
            if (ModelState.IsValid)
            {
                _context.Add(iOUnit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DeviceId"] = new SelectList(_context.Devices, "DeviceId", "MACAddress", iOUnit.DeviceId);
            return View(iOUnit);
        }

        // GET: IOUnits/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var iOUnit = await _context.IOUnits.FindAsync(id);
            if (iOUnit == null)
            {
                return NotFound();
            }
            ViewData["DeviceId"] = new SelectList(_context.Devices, "DeviceId", "MACAddress", iOUnit.DeviceId);
            return View(iOUnit);
        }

        // POST: IOUnits/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IOUnitId,Enabled,Name,DeviceId")] IOUnit iOUnit)
        {
            if (id != iOUnit.IOUnitId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(iOUnit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IOUnitExists(iOUnit.IOUnitId))
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
            ViewData["DeviceId"] = new SelectList(_context.Devices, "DeviceId", "MACAddress", iOUnit.DeviceId);
            return View(iOUnit);
        }

        // GET: IOUnits/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var iOUnit = await _context.IOUnits
                .Include(i => i.Device)
                .FirstOrDefaultAsync(m => m.IOUnitId == id);
            if (iOUnit == null)
            {
                return NotFound();
            }

            return View(iOUnit);
        }

        // POST: IOUnits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var iOUnit = await _context.IOUnits.FindAsync(id);
            _context.IOUnits.Remove(iOUnit);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IOUnitExists(int id)
        {
            return _context.IOUnits.Any(e => e.IOUnitId == id);
        }
    }
}
