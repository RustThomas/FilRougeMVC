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
    public class EmpruntsController : Controller
    {
        private readonly BibliothequeDbContext _context;

        public EmpruntsController(BibliothequeDbContext context)
        {
            _context = context;
        }

        // GET: Emprunts
        public async Task<IActionResult> Index()
        {
            var bibliothequeDbContext = _context.Emprunts.Include(e => e.Lecteur).Include(e => e.Livre);
            return View(await bibliothequeDbContext.ToListAsync());
        }

        // GET: Emprunts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emprunt = await _context.Emprunts
                .Include(e => e.Lecteur)
                .Include(e => e.Livre)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (emprunt == null)
            {
                return NotFound();
            }

            return View(emprunt);
        }

        // GET: Emprunts/Create
        public IActionResult Create()
        {
            ViewData["LecteurId"] = new SelectList(_context.Lecteurs, "Id", "Email");
            ViewData["LivreId"] = new SelectList(_context.Livres, "Id", "Titre");
            return View();
        }

        // POST: Emprunts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmpruntViewModel emprunt)
        {
            if (ModelState.IsValid)
            {
                Emprunt _emprunt = new Emprunt()
                {
                    DateEmprunt = emprunt.DateEmprunt,
                    DateRetour = emprunt.DateRetour, 
                    LecteurId = emprunt.LecteurId, 
                    LivreId = emprunt.LivreId, 
                    Lecteur = _context.Lecteurs.FirstOrDefault(l => l.Id == emprunt.LecteurId),
                    Livre = _context.Livres.FirstOrDefault(l1 => l1.Id == emprunt.LivreId)
                }; 
                _context.Add(_emprunt);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LecteurId"] = new SelectList(_context.Lecteurs, "Id", "Email", emprunt.LecteurId);
            ViewData["LivreId"] = new SelectList(_context.Livres, "Id", "Titre", emprunt.LivreId);
            return View(emprunt);
        }

        // GET: Emprunts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emprunt = await _context.Emprunts.FindAsync(id);
            if (emprunt == null)
            {
                return NotFound();
            }
            ViewData["LecteurId"] = new SelectList(_context.Lecteurs, "Id", "Email", emprunt.LecteurId);
            ViewData["LivreId"] = new SelectList(_context.Livres, "Id", "Titre", emprunt.LivreId);
            return View(emprunt);
        }

        // POST: Emprunts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DateEmprunt,DateRetour,LecteurId,LivreId")] Emprunt emprunt)
        {
            if (id != emprunt.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(emprunt);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmpruntExists(emprunt.Id))
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
            ViewData["LecteurId"] = new SelectList(_context.Lecteurs, "Id", "Email", emprunt.LecteurId);
            ViewData["LivreId"] = new SelectList(_context.Livres, "Id", "Titre", emprunt.LivreId);
            return View(emprunt);
        }

        // GET: Emprunts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emprunt = await _context.Emprunts
                .Include(e => e.Lecteur)
                .Include(e => e.Livre)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (emprunt == null)
            {
                return NotFound();
            }

            return View(emprunt);
        }

        // POST: Emprunts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var emprunt = await _context.Emprunts.FindAsync(id);
            if (emprunt != null)
            {
                _context.Emprunts.Remove(emprunt);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmpruntExists(int id)
        {
            return _context.Emprunts.Any(e => e.Id == id);
        }
    }
}
