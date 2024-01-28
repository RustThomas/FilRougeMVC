using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FilRougeMVC.Data;

namespace FilRougeMVC.Controllers
{
    public class LecteursController : Controller
    {
        private readonly BibliothequeDbContext _context;

        public LecteursController(BibliothequeDbContext context)
        {
            _context = context;
        }

        // GET: Lecteurs
        public async Task<IActionResult> Index()
        {
            var bibliothequeDbContext = _context.Lecteurs.Include(l => l.Adresse);
            return View(await bibliothequeDbContext.ToListAsync());
        }

        // GET: Lecteurs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lecteur = await _context.Lecteurs
                .Include(l => l.Adresse)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lecteur == null)
            {
                return NotFound();
            }

            return View(lecteur);
        }

        // GET: Lecteurs/Create
        public IActionResult Create()
        {
            ViewData["AdresseId"] = new SelectList(_context.Adresses, "Id", "Appartement");
            return View();
        }

        // POST: Lecteurs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LecteurViewModel lecteur)
        {
            if (ModelState.IsValid)
            {
                Lecteur _lecteur = new Lecteur()
                {
                    Nom = lecteur.Nom,
                    Prenom = lecteur.Prenom,
                    Email = lecteur.Email,
                    Telephone = lecteur.Telephone,
                    MotDePasse = lecteur.MotDePasse,
                    AdresseId = lecteur.AdresseId,
                    Adresse = _context.Adresses.FirstOrDefault(ad => ad.Id == lecteur.AdresseId)
                };
                _context.Add(_lecteur);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AdresseId"] = new SelectList(_context.Adresses, "Id", "Appartement", lecteur.AdresseId);
            return View(lecteur);
        }

        // GET: Lecteurs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lecteur = await _context.Lecteurs.FindAsync(id);
            if (lecteur == null)
            {
                return NotFound();
            }
            ViewData["AdresseId"] = new SelectList(_context.Adresses, "Id", "Appartement", lecteur.AdresseId);
            return View(lecteur);
        }

        // POST: Lecteurs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nom,Prenom,Email,Telephone,MotDePasse,AdresseId")] Lecteur lecteur)
        {
            if (id != lecteur.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lecteur);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LecteurExists(lecteur.Id))
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
            ViewData["AdresseId"] = new SelectList(_context.Adresses, "Id", "Appartement", lecteur.AdresseId);
            return View(lecteur);
        }

        // GET: Lecteurs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lecteur = await _context.Lecteurs
                .Include(l => l.Adresse)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lecteur == null)
            {
                return NotFound();
            }

            return View(lecteur);
        }

        // POST: Lecteurs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lecteur = await _context.Lecteurs.FindAsync(id);
            if (lecteur != null)
            {
                _context.Lecteurs.Remove(lecteur);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LecteurExists(int id)
        {
            return _context.Lecteurs.Any(e => e.Id == id);
        }
    }
}
