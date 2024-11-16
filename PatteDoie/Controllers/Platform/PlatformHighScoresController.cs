using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatteDoie.Models.Platform;

namespace PatteDoie.Controllers.Platform
{
    public class PlatformHighScoresController : Controller
    {
        private readonly PatteDoieContext _context;

        public PlatformHighScoresController(PatteDoieContext context)
        {
            _context = context;
        }

        // GET: PlatformHighScores
        public async Task<IActionResult> Index()
        {
            return View(await _context.PlatformHighScore.ToListAsync());
        }

        // GET: PlatformHighScores/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var platformHighScore = await _context.PlatformHighScore
                .FirstOrDefaultAsync(m => m.Id == id);
            if (platformHighScore == null)
            {
                return NotFound();
            }

            return View(platformHighScore);
        }

        // GET: PlatformHighScores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PlatformHighScores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Player,Name")] PlatformHighScore platformHighScore)
        {
            if (ModelState.IsValid)
            {
                platformHighScore.Id = Guid.NewGuid();
                _context.Add(platformHighScore);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(platformHighScore);
        }

        // GET: PlatformHighScores/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var platformHighScore = await _context.PlatformHighScore.FindAsync(id);
            if (platformHighScore == null)
            {
                return NotFound();
            }
            return View(platformHighScore);
        }

        // POST: PlatformHighScores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Player,Name")] PlatformHighScore platformHighScore)
        {
            if (id != platformHighScore.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(platformHighScore);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlatformHighScoreExists(platformHighScore.Id))
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
            return View(platformHighScore);
        }

        // GET: PlatformHighScores/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var platformHighScore = await _context.PlatformHighScore
                .FirstOrDefaultAsync(m => m.Id == id);
            if (platformHighScore == null)
            {
                return NotFound();
            }

            return View(platformHighScore);
        }

        // POST: PlatformHighScores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var platformHighScore = await _context.PlatformHighScore.FindAsync(id);
            if (platformHighScore != null)
            {
                _context.PlatformHighScore.Remove(platformHighScore);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlatformHighScoreExists(Guid id)
        {
            return _context.PlatformHighScore.Any(e => e.Id == id);
        }
    }
}
