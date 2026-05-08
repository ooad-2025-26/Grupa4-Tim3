using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ESportskiCentar.Data;
using ESportskiCentar.Models;

namespace ESportskiCentar.Controllers
{
    public class TerminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TerminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Termin
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Termini.Include(t => t.Teren);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Termin/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var termin = await _context.Termini
                .Include(t => t.Teren)
                .FirstOrDefaultAsync(m => m.id == id);
            if (termin == null)
            {
                return NotFound();
            }

            return View(termin);
        }

        // GET: Termin/Create
        public IActionResult Create()
        {
            ViewData["terenID"] = new SelectList(_context.Tereni, "id", "id");
            return View();
        }

        // POST: Termin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,datum,cijena,rezervisan,terenID")] Termin termin)
        {
            if (ModelState.IsValid)
            {
                _context.Add(termin);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["terenID"] = new SelectList(_context.Tereni, "id", "id", termin.terenID);
            return View(termin);
        }

        // GET: Termin/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var termin = await _context.Termini.FindAsync(id);
            if (termin == null)
            {
                return NotFound();
            }
            ViewData["terenID"] = new SelectList(_context.Tereni, "id", "id", termin.terenID);
            return View(termin);
        }

        // POST: Termin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,datum,cijena,rezervisan,terenID")] Termin termin)
        {
            if (id != termin.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(termin);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TerminExists(termin.id))
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
            ViewData["terenID"] = new SelectList(_context.Tereni, "id", "id", termin.terenID);
            return View(termin);
        }

        // GET: Termin/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var termin = await _context.Termini
                .Include(t => t.Teren)
                .FirstOrDefaultAsync(m => m.id == id);
            if (termin == null)
            {
                return NotFound();
            }

            return View(termin);
        }

        // POST: Termin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var termin = await _context.Termini.FindAsync(id);
            if (termin != null)
            {
                _context.Termini.Remove(termin);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TerminExists(int id)
        {
            return _context.Termini.Any(e => e.id == id);
        }
    }
}
