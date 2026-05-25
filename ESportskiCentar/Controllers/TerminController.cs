using ESportskiCentar.Data;
using ESportskiCentar.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ESportskiCentar.Controllers
{
    [Authorize]
    public class TerminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TerminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(DateTime? datumOd, DateTime? datumDo, Sport? sport)
        {
            var termini = _context.Termini
                .Include(t => t.Teren)
                .AsQueryable();

            // Ako je obični korisnik, vidi samo slobodne buduće termine
            if (!User.IsInRole("Administrator") && !User.IsInRole("Radnik"))
            {
                termini = termini.Where(t => !t.rezervisan && t.datum > DateTime.Now);
            }

            if (datumOd.HasValue)
            {
                termini = termini.Where(t => t.datum.Date >= datumOd.Value.Date);
            }

            if (datumDo.HasValue)
            {
                termini = termini.Where(t => t.datum.Date <= datumDo.Value.Date);
            }

            if (sport.HasValue)
            {
                termini = termini.Where(t => t.Teren.sport == sport.Value);
            }

            ViewBag.DatumOd = datumOd?.ToString("yyyy-MM-dd");
            ViewBag.DatumDo = datumDo?.ToString("yyyy-MM-dd");
            ViewBag.Sport = sport;

            var lista = await termini
                .OrderBy(t => t.datum)
                .ToListAsync();

            return View(lista);
        }

        // DETALJI TERMINA
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var termin = await _context.Termini
                .Include(t => t.Teren)
                .FirstOrDefaultAsync(t => t.id == id);

            if (termin == null)
                return NotFound();

            if (User.IsInRole("Administrator") || User.IsInRole("Radnik"))
            {
                var rezervacija = await _context.Rezervacije
                    .Include(r => r.Korisnik)
                    .FirstOrDefaultAsync(r => r.terminID == termin.id);

                ViewBag.EmailKorisnika = rezervacija?.Korisnik?.Email;
            }

            return View(termin);
        }

        // SAMO ADMIN I RADNIK MOGU DODATI TERMIN
        [Authorize(Roles = "Administrator,Radnik")]
        public IActionResult Create()
        {
            ViewData["terenID"] = new SelectList(_context.Tereni, "id", "naziv");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,Radnik")]
        public async Task<IActionResult> Create([Bind("datum,cijena,terenID,rezervisan")] Termin termin)
        {
            termin.rezervisan = false;

            if (termin.datum <= DateTime.Now)
            {
                ModelState.AddModelError("datum", "Ne možete dodati termin u prošlosti.");
            }

            bool postojiTermin = await _context.Termini.AnyAsync(t =>
                t.terenID == termin.terenID &&
                t.datum == termin.datum);

            if (postojiTermin)
            {
                ModelState.AddModelError("", "Termin za ovaj teren i vrijeme već postoji.");
            }

            if (ModelState.IsValid)
            {
                _context.Termini.Add(termin);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewData["terenID"] = new SelectList(_context.Tereni, "id", "naziv", termin.terenID);
            return View(termin);
        }

        [Authorize(Roles = "Administrator,Radnik")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var termin = await _context.Termini.FindAsync(id);

            if (termin == null)
                return NotFound();
            if (termin.rezervisan)
            {
                TempData["Greska"] = "Ne možete uređivati termin koji je već rezervisan.";
                return RedirectToAction(nameof(Index));
            }

            ViewData["terenID"] = new SelectList(_context.Tereni, "id", "naziv", termin.terenID);
            return View(termin);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,Radnik")]
        public async Task<IActionResult> Edit(int id, [Bind("id,datum,cijena,terenID")] Termin termin)
        {
            if (id != termin.id)
                return NotFound();

            var postojeciTermin = await _context.Termini.AsNoTracking()
                .FirstOrDefaultAsync(t => t.id == id);

            if (postojeciTermin == null)
                return NotFound();

            if (postojeciTermin.rezervisan)
            {
                TempData["Greska"] = "Ne možete uređivati termin koji je već rezervisan.";
                return RedirectToAction(nameof(Index));
            }

            termin.rezervisan = false;

            if (termin.datum <= DateTime.Now)
            {
                ModelState.AddModelError("datum", "Ne možete postaviti termin u prošlosti.");
            }

            if (ModelState.IsValid)
            {
                _context.Update(termin);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewData["terenID"] = new SelectList(_context.Tereni, "id", "naziv", termin.terenID);
            return View(termin);
        }

        [Authorize(Roles = "Administrator,Radnik")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var termin = await _context.Termini
                .Include(t => t.Teren)
                .FirstOrDefaultAsync(t => t.id == id);

            if (termin == null)
                return NotFound();

            return View(termin);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,Radnik")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var termin = await _context.Termini.FindAsync(id);

            if (termin == null)
                return NotFound();

            if (termin.rezervisan)
            {
                ModelState.AddModelError("", "Ne možete obrisati rezervisan termin.");

                termin = await _context.Termini
                    .Include(t => t.Teren)
                    .FirstOrDefaultAsync(t => t.id == id);

                return View("Delete", termin);
            }

            _context.Termini.Remove(termin);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}