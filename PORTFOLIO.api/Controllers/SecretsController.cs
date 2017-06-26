using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PORTFOLIO.api.Data;
using PORTFOLIO.api.Models;
using Microsoft.AspNetCore.Authorization;

namespace PORTFOLIO.api.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class SecretsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SecretsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Secrets
        public async Task<IActionResult> Index()
        {
            return View(await _context.Secrets.ToListAsync());
        }

        // GET: Secrets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var secret = await _context.Secrets
                .SingleOrDefaultAsync(m => m.Id == id);
            if (secret == null)
            {
                return NotFound();
            }

            return View(secret);
        }

        // GET: Secrets/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Secrets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Value")] Secret secret)
        {
            if (ModelState.IsValid)
            {
                _context.Add(secret);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(secret);
        }

        // GET: Secrets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var secret = await _context.Secrets.SingleOrDefaultAsync(m => m.Id == id);
            if (secret == null)
            {
                return NotFound();
            }
            return View(secret);
        }

        // POST: Secrets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Value")] Secret secret)
        {
            if (id != secret.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(secret);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SecretExists(secret.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(secret);
        }

        // GET: Secrets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var secret = await _context.Secrets
                .SingleOrDefaultAsync(m => m.Id == id);
            if (secret == null)
            {
                return NotFound();
            }

            return View(secret);
        }

        // POST: Secrets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var secret = await _context.Secrets.SingleOrDefaultAsync(m => m.Id == id);
            _context.Secrets.Remove(secret);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool SecretExists(int id)
        {
            return _context.Secrets.Any(e => e.Id == id);
        }
    }
}
