using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PORTFOLIO.api.Data;
using PORTFOLIO.api.Models;

namespace PORTFOLIO.api.Controllers
{
    public class StuffController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StuffController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Stuff
        public async Task<IActionResult> Index()
        {
            return View(await _context.Stuff.ToListAsync());
        }

        // GET: Stuff/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stuff = await _context.Stuff
                .SingleOrDefaultAsync(m => m.Id == id);
            if (stuff == null)
            {
                return NotFound();
            }

            return View(stuff);
        }

        // GET: Stuff/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Stuff/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ImagePath,Categories,Link,Year,Order,RepoLink,Role")] Stuff stuff)
        {
            if (ModelState.IsValid)
            {
                _context.Add(stuff);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(stuff);
        }

        // GET: Stuff/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stuff = await _context.Stuff.SingleOrDefaultAsync(m => m.Id == id);
            if (stuff == null)
            {
                return NotFound();
            }
            return View(stuff);
        }

        // POST: Stuff/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ImagePath,Categories,Link,Year,Order,RepoLink,Role")] Stuff stuff)
        {
            if (id != stuff.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stuff);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StuffExists(stuff.Id))
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
            return View(stuff);
        }

        // GET: Stuff/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stuff = await _context.Stuff
                .SingleOrDefaultAsync(m => m.Id == id);
            if (stuff == null)
            {
                return NotFound();
            }

            return View(stuff);
        }

        // POST: Stuff/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var stuff = await _context.Stuff.SingleOrDefaultAsync(m => m.Id == id);
            _context.Stuff.Remove(stuff);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool StuffExists(int id)
        {
            return _context.Stuff.Any(e => e.Id == id);
        }
    }
}
