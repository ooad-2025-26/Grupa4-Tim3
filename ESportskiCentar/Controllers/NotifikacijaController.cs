using ESportskiCentar.Data;
using ESportskiCentar.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESportskiCentar.Controllers
{
    [Authorize]
    public class NotifikacijaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NotifikacijaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Notifikacija
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Notifikacije.Include(n => n.Rezervacija);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Notifikacija/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notifikacija = await _context.Notifikacije
                .Include(n => n.Rezervacija)
                .FirstOrDefaultAsync(m => m.id == id);
            if (notifikacija == null)
            {
                return NotFound();
            }

            return View(notifikacija);
        }

        // GET: Notifikacija/Create
        public IActionResult Create()
        {
            ViewData["rezervacijaID"] = new SelectList(_context.Rezervacije, "id", "id");
            return View();
        }

        // POST: Notifikacija/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,poslana,vrijemeSlanja,rezervacijaID")] Notifikacija notifikacija)
        {
            if (ModelState.IsValid)
            {
                _context.Add(notifikacija);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["rezervacijaID"] = new SelectList(_context.Rezervacije, "id", "id", notifikacija.rezervacijaID);
            return View(notifikacija);
        }

        // GET: Notifikacija/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notifikacija = await _context.Notifikacije.FindAsync(id);
            if (notifikacija == null)
            {
                return NotFound();
            }
            ViewData["rezervacijaID"] = new SelectList(_context.Rezervacije, "id", "id", notifikacija.rezervacijaID);
            return View(notifikacija);
        }

        // POST: Notifikacija/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,poslana,vrijemeSlanja,rezervacijaID")] Notifikacija notifikacija)
        {
            if (id != notifikacija.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(notifikacija);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NotifikacijaExists(notifikacija.id))
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
            ViewData["rezervacijaID"] = new SelectList(_context.Rezervacije, "id", "id", notifikacija.rezervacijaID);
            return View(notifikacija);
        }

        // GET: Notifikacija/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notifikacija = await _context.Notifikacije
                .Include(n => n.Rezervacija)
                .FirstOrDefaultAsync(m => m.id == id);
            if (notifikacija == null)
            {
                return NotFound();
            }

            return View(notifikacija);
        }

        // POST: Notifikacija/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var notifikacija = await _context.Notifikacije.FindAsync(id);
            if (notifikacija != null)
            {
                _context.Notifikacije.Remove(notifikacija);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NotifikacijaExists(int id)
        {
            return _context.Notifikacije.Any(e => e.id == id);
        }
    }
}
