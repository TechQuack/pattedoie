using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatteDoie.Models.Scattergories;

namespace PatteDoie.Controllers.Scattergories
{
    public class ScattergoriesPlayersController : Controller
    {
        private readonly PatteDoieContext _context;

        public ScattergoriesPlayersController(PatteDoieContext context)
        {
            _context = context;
        }

        // GET: ScattergoriesPlayers
        public async Task<IActionResult> Index()
        {
            return View(await _context.ScattergoriesPlayer.ToListAsync());
        }

        // GET: ScattergoriesPlayers/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scattergoriesPlayer = await _context.ScattergoriesPlayer
                .FirstOrDefaultAsync(m => m.Id == id);
            if (scattergoriesPlayer == null)
            {
                return NotFound();
            }

            return View(scattergoriesPlayer);
        }

        // GET: ScattergoriesPlayers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ScattergoriesPlayers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Score")] ScattergoriesPlayer scattergoriesPlayer)
        {
            if (ModelState.IsValid)
            {
                scattergoriesPlayer.Id = Guid.NewGuid();
                _context.Add(scattergoriesPlayer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(scattergoriesPlayer);
        }

        // GET: ScattergoriesPlayers/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scattergoriesPlayer = await _context.ScattergoriesPlayer.FindAsync(id);
            if (scattergoriesPlayer == null)
            {
                return NotFound();
            }
            return View(scattergoriesPlayer);
        }

        // POST: ScattergoriesPlayers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Score")] ScattergoriesPlayer scattergoriesPlayer)
        {
            if (id != scattergoriesPlayer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(scattergoriesPlayer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScattergoriesPlayerExists(scattergoriesPlayer.Id))
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
            return View(scattergoriesPlayer);
        }

        // GET: ScattergoriesPlayers/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scattergoriesPlayer = await _context.ScattergoriesPlayer
                .FirstOrDefaultAsync(m => m.Id == id);
            if (scattergoriesPlayer == null)
            {
                return NotFound();
            }

            return View(scattergoriesPlayer);
        }

        // POST: ScattergoriesPlayers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var scattergoriesPlayer = await _context.ScattergoriesPlayer.FindAsync(id);
            if (scattergoriesPlayer != null)
            {
                _context.ScattergoriesPlayer.Remove(scattergoriesPlayer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScattergoriesPlayerExists(Guid id)
        {
            return _context.ScattergoriesPlayer.Any(e => e.Id == id);
        }
    }
}
