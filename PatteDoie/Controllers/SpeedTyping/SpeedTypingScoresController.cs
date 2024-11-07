using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatteDoie.Models.SpeedTyping;

namespace PatteDoie.Controllers.SpeedTyping
{
    public class SpeedTypingScoresController : Controller
    {
        private readonly PatteDoieContext _context;

        public SpeedTypingScoresController(PatteDoieContext context)
        {
            _context = context;
        }

        // GET: SpeedTypingScores
        public async Task<IActionResult> Index()
        {
            return View(await _context.SpeedTypingScore.ToListAsync());
        }

        // GET: SpeedTypingScores/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var speedTypingScore = await _context.SpeedTypingScore
                .FirstOrDefaultAsync(m => m.Id == id);
            if (speedTypingScore == null)
            {
                return NotFound();
            }

            return View(speedTypingScore);
        }

        // GET: SpeedTypingScores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SpeedTypingScores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,Score")] SpeedTypingScore speedTypingScore)
        {
            if (ModelState.IsValid)
            {
                speedTypingScore.Id = Guid.NewGuid();
                _context.Add(speedTypingScore);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(speedTypingScore);
        }

        // GET: SpeedTypingScores/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var speedTypingScore = await _context.SpeedTypingScore.FindAsync(id);
            if (speedTypingScore == null)
            {
                return NotFound();
            }
            return View(speedTypingScore);
        }

        // POST: SpeedTypingScores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,UserId,Score")] SpeedTypingScore speedTypingScore)
        {
            if (id != speedTypingScore.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(speedTypingScore);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpeedTypingScoreExists(speedTypingScore.Id))
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
            return View(speedTypingScore);
        }

        // GET: SpeedTypingScores/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var speedTypingScore = await _context.SpeedTypingScore
                .FirstOrDefaultAsync(m => m.Id == id);
            if (speedTypingScore == null)
            {
                return NotFound();
            }

            return View(speedTypingScore);
        }

        // POST: SpeedTypingScores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var speedTypingScore = await _context.SpeedTypingScore.FindAsync(id);
            if (speedTypingScore != null)
            {
                _context.SpeedTypingScore.Remove(speedTypingScore);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SpeedTypingScoreExists(Guid id)
        {
            return _context.SpeedTypingScore.Any(e => e.Id == id);
        }
    }
}
