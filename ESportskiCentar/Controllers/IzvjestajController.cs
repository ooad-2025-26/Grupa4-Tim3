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
    public class IzvjestajController : Controller
    {
        private readonly ApplicationDbContext _context;

        public IzvjestajController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Izvjestaj
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Izvjestaji.Include(i => i.Korisnik);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Izvjestaj/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var izvjestaj = await _context.Izvjestaji
                .Include(i => i.Korisnik)
                .FirstOrDefaultAsync(m => m.id == id);
            if (izvjestaj == null)
            {
                return NotFound();
            }

            return View(izvjestaj);
        }

        // GET: Izvjestaj/Create
        public IActionResult Create()
        {
            ViewData["korisnikID"] = new SelectList(_context.Korisnici, "id", "id");
            return View();
        }

        // POST: Izvjestaj/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,datumGenerisanja,korisnikID,datumOd,datumDo")] Izvjestaj izvjestaj)
        {
            if (ModelState.IsValid)
            {
                _context.Add(izvjestaj);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["korisnikID"] = new SelectList(_context.Korisnici, "id", "id", izvjestaj.korisnikID);
            return View(izvjestaj);
        }

        // GET: Izvjestaj/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var izvjestaj = await _context.Izvjestaji.FindAsync(id);
            if (izvjestaj == null)
            {
                return NotFound();
            }
            ViewData["korisnikID"] = new SelectList(_context.Korisnici, "id", "id", izvjestaj.korisnikID);
            return View(izvjestaj);
        }

        // POST: Izvjestaj/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,datumGenerisanja,korisnikID,datumOd,datumDo")] Izvjestaj izvjestaj)
        {
            if (id != izvjestaj.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(izvjestaj);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IzvjestajExists(izvjestaj.id))
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
            ViewData["korisnikID"] = new SelectList(_context.Korisnici, "id", "id", izvjestaj.korisnikID);
            return View(izvjestaj);
        }

        // GET: Izvjestaj/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var izvjestaj = await _context.Izvjestaji
                .Include(i => i.Korisnik)
                .FirstOrDefaultAsync(m => m.id == id);
            if (izvjestaj == null)
            {
                return NotFound();
            }

            return View(izvjestaj);
        }

        // POST: Izvjestaj/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var izvjestaj = await _context.Izvjestaji.FindAsync(id);
            if (izvjestaj != null)
            {
                _context.Izvjestaji.Remove(izvjestaj);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IzvjestajExists(int id)
        {
            return _context.Izvjestaji.Any(e => e.id == id);
        }
    }
}
