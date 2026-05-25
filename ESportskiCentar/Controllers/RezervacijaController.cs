using System.Security.Claims;
using ESportskiCentar.Data;
using ESportskiCentar.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ESportskiCentar.Controllers
{
    [Authorize]
    public class RezervacijaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RezervacijaController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var korisnikId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var rezervacije = _context.Rezervacije
                .Include(r => r.Korisnik)
                .Include(r => r.Termin)
                .ThenInclude(t => t.Teren)
                .AsQueryable();

            // Obični korisnik vidi samo svoje rezervacije.
            if (User.IsInRole(RoleNames.Korisnik))
            {
                rezervacije = rezervacije.Where(r => r.korisnikID == korisnikId);
            }

            return View(await rezervacije.ToListAsync());
        }

        public async Task<IActionResult> Create(int? terminId)
        {
            if (terminId == null)
                return RedirectToAction("Index", "Termin");

            var termin = await _context.Termini
                .Include(t => t.Teren)
                .FirstOrDefaultAsync(t => t.id == terminId && !t.rezervisan);

            if (termin == null)
                return NotFound();

            var rezervacija = new Rezervacija
            {
                terminID = termin.id,
                Termin = termin
            };

            return View(rezervacija);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("terminID")] Rezervacija rezervacija)
        {
            var korisnikId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var termin = await _context.Termini
                .Include(t => t.Teren)
                .FirstOrDefaultAsync(t => t.id == rezervacija.terminID);

            if (termin == null) return NotFound();

            if (termin.rezervisan)
            {
                ModelState.AddModelError("", "Termin je u međuvremenu zauzet.");
                return View(rezervacija);
            }

            double cijena = termin.cijena;
            bool imaPopust = false;

            // Automatska dodjela popusta.
            var popust = await _context.Popusti.FirstOrDefaultAsync();

            if (popust != null)
            {
                int brojRezervacija = await _context.Rezervacije.CountAsync(r =>
                                                                             r.korisnikID == korisnikId);

                if (brojRezervacija >= popust.potrebanBrojRezervacija)
                {
                    cijena = cijena - (cijena * popust.procenat / 100);
                    imaPopust = true;
                }
            }

            rezervacija.korisnikID = korisnikId;
            rezervacija.status = Status.NA_CEKANJU;
            rezervacija.vrijemeRezervacije = DateTime.Now;
            rezervacija.primjenjenPopust = imaPopust;
            rezervacija.konacnaCijena = cijena;

            termin.rezervisan = true;

            _context.Rezervacije.Add(rezervacija);
            await _context.SaveChangesAsync();

            // Kreira se podsjetnik 24h prije termina.
            var notifikacija = new Notifikacija
            {
                rezervacijaID = rezervacija.id,
                poslana = false,
                vrijemeSlanja = termin.datum.AddHours(-24)
            };

            _context.Notifikacije.Add(notifikacija);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var korisnikId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var rezervacija = await _context.Rezervacije
                .Include(r => r.Termin)
                .ThenInclude(t => t.Teren)
                .FirstOrDefaultAsync(r => r.id == id);

            if (rezervacija == null) return NotFound();

            if (User.IsInRole(RoleNames.Korisnik) && rezervacija.korisnikID != korisnikId)
                return Forbid();

            return View(rezervacija);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rezervacija = await _context.Rezervacije
                .Include(r => r.Termin)
                .FirstOrDefaultAsync(r => r.id == id);

            if (rezervacija == null) return NotFound();

            //Ne dozvoljavamo otkazivanje ako je manje od 2h do termina.
            if (rezervacija.Termin.datum <= DateTime.Now.AddHours(2))
            {
                ModelState.AddModelError("", "Otkazivanje više nije moguće.");
                return View("Delete", rezervacija);
            }

            var notifikacije = _context.Notifikacije
                .Where(n => n.rezervacijaID == rezervacija.id);

            _context.Notifikacije.RemoveRange(notifikacije);

            rezervacija.Termin.rezervisan = false;

            _context.Rezervacije.Remove(rezervacija);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}