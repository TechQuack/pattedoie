using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PatteDoie.Models.Platform;

namespace PatteDoie.Controllers.Platform
{
    public class PlatformUsersController : Controller
    {
        private readonly PatteDoieContext _context;

        public PlatformUsersController(PatteDoieContext context)
        {
            _context = context;
        }

        // GET: PlatformUsers
        public async Task<IActionResult> Index()
        {
            return View(await _context.PlatformUser.ToListAsync());
        }

        // GET: PlatformUsers/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var platformUser = await _context.PlatformUser
                .FirstOrDefaultAsync(m => m.Id == id);
            if (platformUser == null)
            {
                return NotFound();
            }

            return View(platformUser);
        }

        // GET: PlatformUsers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PlatformUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nickname")] PlatformUser platformUser)
        {
            if (ModelState.IsValid)
            {
                platformUser.Id = Guid.NewGuid();
                _context.Add(platformUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(platformUser);
        }

        // GET: PlatformUsers/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var platformUser = await _context.PlatformUser.FindAsync(id);
            if (platformUser == null)
            {
                return NotFound();
            }
            return View(platformUser);
        }

        // POST: PlatformUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Nickname")] PlatformUser platformUser)
        {
            if (id != platformUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(platformUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlatformUserExists(platformUser.Id))
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
            return View(platformUser);
        }

        // GET: PlatformUsers/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var platformUser = await _context.PlatformUser
                .FirstOrDefaultAsync(m => m.Id == id);
            if (platformUser == null)
            {
                return NotFound();
            }

            return View(platformUser);
        }

        // POST: PlatformUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var platformUser = await _context.PlatformUser.FindAsync(id);
            if (platformUser != null)
            {
                _context.PlatformUser.Remove(platformUser);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlatformUserExists(Guid id)
        {
            return _context.PlatformUser.Any(e => e.Id == id);
        }
    }
}
