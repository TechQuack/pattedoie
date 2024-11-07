using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatteDoie.Models.SpeedTyping;

namespace PatteDoie
{
    public class SpeedTypingWordsController : Controller
    {
        private readonly PatteDoieContext _context;

        public SpeedTypingWordsController(PatteDoieContext context)
        {
            _context = context;
        }

        // GET: SpeedTypingWords
        public async Task<IActionResult> Index()
        {
            return View(await _context.SpeedTypingWord.ToListAsync());
        }

        // GET: SpeedTypingWords/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var speedTypingWord = await _context.SpeedTypingWord
                .FirstOrDefaultAsync(m => m.Id == id);
            if (speedTypingWord == null)
            {
                return NotFound();
            }

            return View(speedTypingWord);
        }

        // GET: SpeedTypingWords/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SpeedTypingWords/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Value")] SpeedTypingWord speedTypingWord)
        {
            if (ModelState.IsValid)
            {
                speedTypingWord.Id = Guid.NewGuid();
                _context.Add(speedTypingWord);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(speedTypingWord);
        }

        // GET: SpeedTypingWords/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var speedTypingWord = await _context.SpeedTypingWord.FindAsync(id);
            if (speedTypingWord == null)
            {
                return NotFound();
            }
            return View(speedTypingWord);
        }

        // POST: SpeedTypingWords/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Value")] SpeedTypingWord speedTypingWord)
        {
            if (id != speedTypingWord.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(speedTypingWord);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpeedTypingWordExists(speedTypingWord.Id))
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
            return View(speedTypingWord);
        }

        // GET: SpeedTypingWords/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var speedTypingWord = await _context.SpeedTypingWord
                .FirstOrDefaultAsync(m => m.Id == id);
            if (speedTypingWord == null)
            {
                return NotFound();
            }

            return View(speedTypingWord);
        }

        // POST: SpeedTypingWords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var speedTypingWord = await _context.SpeedTypingWord.FindAsync(id);
            if (speedTypingWord != null)
            {
                _context.SpeedTypingWord.Remove(speedTypingWord);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SpeedTypingWordExists(Guid id)
        {
            return _context.SpeedTypingWord.Any(e => e.Id == id);
        }
    }
}
