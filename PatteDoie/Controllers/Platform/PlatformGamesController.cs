using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatteDoie.Models.Platform;

namespace PatteDoie.Controllers.Platform
{
    public class PlatformGamesController : Controller
    {
        private readonly PatteDoieContext _context;

        public PlatformGamesController(PatteDoieContext context)
        {
            _context = context;
        }

        // GET: PlatformGames
        public async Task<IActionResult> Index()
        {
            return View(await _context.PlatformGame.ToListAsync());
        }

        // GET: PlatformGames/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var platformGame = await _context.PlatformGame
                .FirstOrDefaultAsync(m => m.Id == id);
            if (platformGame == null)
            {
                return NotFound();
            }

            return View(platformGame);
        }

        // GET: PlatformGames/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PlatformGames/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Min_players,Max_players")] PlatformGame platformGame)
        {
            if (ModelState.IsValid)
            {
                platformGame.Id = Guid.NewGuid();
                _context.Add(platformGame);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(platformGame);
        }

        // GET: PlatformGames/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var platformGame = await _context.PlatformGame.FindAsync(id);
            if (platformGame == null)
            {
                return NotFound();
            }
            return View(platformGame);
        }

        // POST: PlatformGames/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Min_players,Max_players")] PlatformGame platformGame)
        {
            if (id != platformGame.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(platformGame);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlatformGameExists(platformGame.Id))
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
            return View(platformGame);
        }

        // GET: PlatformGames/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var platformGame = await _context.PlatformGame
                .FirstOrDefaultAsync(m => m.Id == id);
            if (platformGame == null)
            {
                return NotFound();
            }

            return View(platformGame);
        }

        // POST: PlatformGames/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var platformGame = await _context.PlatformGame.FindAsync(id);
            if (platformGame != null)
            {
                _context.PlatformGame.Remove(platformGame);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlatformGameExists(Guid id)
        {
            return _context.PlatformGame.Any(e => e.Id == id);
        }
    }
}
