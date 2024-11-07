using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatteDoie.Models.SpeedTyping;

namespace PatteDoie.Controllers.SpeedTyping
{
    public class SpeedTypingTimeProgressesController : Controller
    {
        private readonly PatteDoieContext _context;

        public SpeedTypingTimeProgressesController(PatteDoieContext context)
        {
            _context = context;
        }

        // GET: SpeedTypingTimeProgresses
        public async Task<IActionResult> Index()
        {
            return View(await _context.SpeedTypingTimeProgress.ToListAsync());
        }

        // GET: SpeedTypingTimeProgresses/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var speedTypingTimeProgress = await _context.SpeedTypingTimeProgress
                .FirstOrDefaultAsync(m => m.Id == id);
            if (speedTypingTimeProgress == null)
            {
                return NotFound();
            }

            return View(speedTypingTimeProgress);
        }

        // GET: SpeedTypingTimeProgresses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SpeedTypingTimeProgresses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,TimeProgress")] SpeedTypingTimeProgress speedTypingTimeProgress)
        {
            if (ModelState.IsValid)
            {
                speedTypingTimeProgress.Id = Guid.NewGuid();
                _context.Add(speedTypingTimeProgress);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(speedTypingTimeProgress);
        }

        // GET: SpeedTypingTimeProgresses/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var speedTypingTimeProgress = await _context.SpeedTypingTimeProgress.FindAsync(id);
            if (speedTypingTimeProgress == null)
            {
                return NotFound();
            }
            return View(speedTypingTimeProgress);
        }

        // POST: SpeedTypingTimeProgresses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,UserId,TimeProgress")] SpeedTypingTimeProgress speedTypingTimeProgress)
        {
            if (id != speedTypingTimeProgress.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(speedTypingTimeProgress);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpeedTypingTimeProgressExists(speedTypingTimeProgress.Id))
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
            return View(speedTypingTimeProgress);
        }

        // GET: SpeedTypingTimeProgresses/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var speedTypingTimeProgress = await _context.SpeedTypingTimeProgress
                .FirstOrDefaultAsync(m => m.Id == id);
            if (speedTypingTimeProgress == null)
            {
                return NotFound();
            }

            return View(speedTypingTimeProgress);
        }

        // POST: SpeedTypingTimeProgresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var speedTypingTimeProgress = await _context.SpeedTypingTimeProgress.FindAsync(id);
            if (speedTypingTimeProgress != null)
            {
                _context.SpeedTypingTimeProgress.Remove(speedTypingTimeProgress);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SpeedTypingTimeProgressExists(Guid id)
        {
            return _context.SpeedTypingTimeProgress.Any(e => e.Id == id);
        }
    }
}
