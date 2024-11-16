using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatteDoie.Models.Platform;

namespace PatteDoie.Controllers.Platform
{
    public class PlatformLobbiesController : Controller
    {
        private readonly PatteDoieContext _context;

        public PlatformLobbiesController(PatteDoieContext context)
        {
            _context = context;
        }

        // GET: PlatformLobbies
        public async Task<IActionResult> Index()
        {
            return View(await _context.PlatformLobby.ToListAsync());
        }

        // GET: PlatformLobbies/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var platformLobby = await _context.PlatformLobby
                .FirstOrDefaultAsync(m => m.Id == id);
            if (platformLobby == null)
            {
                return NotFound();
            }

            return View(platformLobby);
        }

        // GET: PlatformLobbies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PlatformLobbies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,gameId,password,started")] PlatformLobby platformLobby)
        {
            if (ModelState.IsValid)
            {
                platformLobby.Id = Guid.NewGuid();
                _context.Add(platformLobby);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(platformLobby);
        }

        // GET: PlatformLobbies/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var platformLobby = await _context.PlatformLobby.FindAsync(id);
            if (platformLobby == null)
            {
                return NotFound();
            }
            return View(platformLobby);
        }

        // POST: PlatformLobbies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,gameId,password,started")] PlatformLobby platformLobby)
        {
            if (id != platformLobby.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(platformLobby);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlatformLobbyExists(platformLobby.Id))
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
            return View(platformLobby);
        }

        // GET: PlatformLobbies/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var platformLobby = await _context.PlatformLobby
                .FirstOrDefaultAsync(m => m.Id == id);
            if (platformLobby == null)
            {
                return NotFound();
            }

            return View(platformLobby);
        }

        // POST: PlatformLobbies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var platformLobby = await _context.PlatformLobby.FindAsync(id);
            if (platformLobby != null)
            {
                _context.PlatformLobby.Remove(platformLobby);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlatformLobbyExists(Guid id)
        {
            return _context.PlatformLobby.Any(e => e.Id == id);
        }
    }
}
