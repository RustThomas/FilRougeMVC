using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FilRougeMVC.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace FilRougeMVC.Controllers
{
    public class LivresController : Controller
    {
        private readonly BibliothequeDbContext _context;

        public LivresController(BibliothequeDbContext context)
        {
            _context = context;
        }

        // GET: Livres
        public async Task<IActionResult> Index(string searchString)
        {
            var _bibliothequeDbContext =  _context.Livres.Include(l => l.Auteur).Include(l => l.Domaine).AsQueryable();
            if (!string.IsNullOrEmpty(searchString))
            {
                _bibliothequeDbContext = _bibliothequeDbContext.Where(s =>
               s.Titre.Contains(searchString) ||
               s.Auteur.Nom.Contains(searchString));
            }
            ViewData["CurrentFilter"] = searchString;
            return View(await _bibliothequeDbContext.ToListAsync());
        }

        // GET: Livres/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var livre = await _context.Livres
                .Include(l => l.Auteur)
                .Include(l => l.Domaine)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (livre == null)
            {
                return NotFound();
            }

            return View(livre);
        }
   


        // GET: Livres/Create
        public IActionResult Create()
        {
            // Nom et Prenom de l'Id Associée 
            ViewData["AuteurId"] = new SelectList(_context.Auteurs, "Id", "FullName");
            ViewData["DomaineId"] = new SelectList(_context.Domaines, "Id", "Description");
            return View();
        }

        // POST: Livres/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LivreViewModel livre)
        {

            if (ModelState.IsValid)
            {
                Livre _livre = new Livre()
                {
                    Titre = livre.Titre,
                    NombrePages = livre.NombrePages,
                    StatutDuLivre = livre.StatutDuLivre,
                    EtatDuLivre = livre.EtatDuLivre,
                    AuteurId = livre.AuteurId,
                    DomaineId = livre.DomaineId,
                    Auteur = _context.Auteurs.FirstOrDefault(a => a.Id == livre.AuteurId),
                    Domaine = _context.Domaines.FirstOrDefault(a => a.Id == livre.DomaineId)

                };
                _context.Add(_livre);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuteurId"] = new SelectList(_context.Auteurs, "Id", "Email", livre.AuteurId);
            ViewData["DomaineId"] = new SelectList(_context.Domaines, "Id", "Description", livre.DomaineId);
            return View(livre);
        }

        // GET: Livres/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var livre = await _context.Livres.FindAsync(id);
            if (livre == null)
            {
                return NotFound();
            }
            ViewData["AuteurId"] = new SelectList(_context.Auteurs, "Id", "Email", livre.AuteurId);
            ViewData["DomaineId"] = new SelectList(_context.Domaines, "Id", "Description", livre.DomaineId);
            return View(livre);
        }

        // POST: Livres/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titre,NombrePages,StatutDuLivre,EtatDuLivre,AuteurId,DomaineId")] Livre livre)
        {
            if (id != livre.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(livre);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LivreExists(livre.Id))
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
            ViewData["AuteurId"] = new SelectList(_context.Auteurs, "Id", "Email", livre.AuteurId);
            ViewData["DomaineId"] = new SelectList(_context.Domaines, "Id", "Description", livre.DomaineId);
            return View(livre);
        }

        // GET: Livres/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var livre = await _context.Livres
                .Include(l => l.Auteur)
                .Include(l => l.Domaine)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (livre == null)
            {
                return NotFound();
            }

            return View(livre);
        }

        // POST: Livres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var livre = await _context.Livres.FindAsync(id);
            if (livre != null)
            {
                _context.Livres.Remove(livre);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LivreExists(int id)
        {
            return _context.Livres.Any(e => e.Id == id);
        }
    }
}
