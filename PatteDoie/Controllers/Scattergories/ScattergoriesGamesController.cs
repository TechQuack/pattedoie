using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatteDoie.Models.Scattergories;

namespace PatteDoie.Controllers.Scattergories
{
    public class ScattergoriesGamesController : Controller
    {
        private readonly PatteDoieContext _context;

        public ScattergoriesGamesController(PatteDoieContext context)
        {
            _context = context;
        }

        // GET: ScattergoriesGames
        public async Task<IActionResult> Index()
        {
            return View(await _context.ScattergoriesGame.ToListAsync());
        }

        // GET: ScattergoriesGames/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scattergoriesGame = await _context.ScattergoriesGame
                .FirstOrDefaultAsync(m => m.Id == id);
            if (scattergoriesGame == null)
            {
                return NotFound();
            }

            return View(scattergoriesGame);
        }

        // GET: ScattergoriesGames/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ScattergoriesGames/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MaxRound,CurrentRound,CurrentLetter")] ScattergoriesGame scattergoriesGame)
        {
            if (ModelState.IsValid)
            {
                scattergoriesGame.Id = Guid.NewGuid();
                _context.Add(scattergoriesGame);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(scattergoriesGame);
        }

        // GET: ScattergoriesGames/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scattergoriesGame = await _context.ScattergoriesGame.FindAsync(id);
            if (scattergoriesGame == null)
            {
                return NotFound();
            }
            return View(scattergoriesGame);
        }

        // POST: ScattergoriesGames/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,MaxRound,CurrentRound,CurrentLetter")] ScattergoriesGame scattergoriesGame)
        {
            if (id != scattergoriesGame.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(scattergoriesGame);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScattergoriesGameExists(scattergoriesGame.Id))
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
            return View(scattergoriesGame);
        }

        // GET: ScattergoriesGames/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scattergoriesGame = await _context.ScattergoriesGame
                .FirstOrDefaultAsync(m => m.Id == id);
            if (scattergoriesGame == null)
            {
                return NotFound();
            }

            return View(scattergoriesGame);
        }

        // POST: ScattergoriesGames/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var scattergoriesGame = await _context.ScattergoriesGame.FindAsync(id);
            if (scattergoriesGame != null)
            {
                _context.ScattergoriesGame.Remove(scattergoriesGame);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScattergoriesGameExists(Guid id)
        {
            return _context.ScattergoriesGame.Any(e => e.Id == id);
        }
    }
}
