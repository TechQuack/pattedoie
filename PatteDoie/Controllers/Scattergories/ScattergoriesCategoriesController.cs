using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatteDoie.Models.Scattergories;

namespace PatteDoie.Controllers.Scattergories
{
    public class ScattergoriesCategoriesController : Controller
    {
        private readonly PatteDoieContext _context;

        public ScattergoriesCategoriesController(PatteDoieContext context)
        {
            _context = context;
        }

        // GET: ScattergoriesCategories
        public async Task<IActionResult> Index()
        {
            return View(await _context.ScattergoriesCategory.ToListAsync());
        }

        // GET: ScattergoriesCategories/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scattergoriesCategory = await _context.ScattergoriesCategory
                .FirstOrDefaultAsync(m => m.Id == id);
            if (scattergoriesCategory == null)
            {
                return NotFound();
            }

            return View(scattergoriesCategory);
        }

        // GET: ScattergoriesCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ScattergoriesCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] ScattergoriesCategory scattergoriesCategory)
        {
            if (ModelState.IsValid)
            {
                scattergoriesCategory.Id = Guid.NewGuid();
                _context.Add(scattergoriesCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(scattergoriesCategory);
        }

        // GET: ScattergoriesCategories/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scattergoriesCategory = await _context.ScattergoriesCategory.FindAsync(id);
            if (scattergoriesCategory == null)
            {
                return NotFound();
            }
            return View(scattergoriesCategory);
        }

        // POST: ScattergoriesCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name")] ScattergoriesCategory scattergoriesCategory)
        {
            if (id != scattergoriesCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(scattergoriesCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScattergoriesCategoryExists(scattergoriesCategory.Id))
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
            return View(scattergoriesCategory);
        }

        // GET: ScattergoriesCategories/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scattergoriesCategory = await _context.ScattergoriesCategory
                .FirstOrDefaultAsync(m => m.Id == id);
            if (scattergoriesCategory == null)
            {
                return NotFound();
            }

            return View(scattergoriesCategory);
        }

        // POST: ScattergoriesCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var scattergoriesCategory = await _context.ScattergoriesCategory.FindAsync(id);
            if (scattergoriesCategory != null)
            {
                _context.ScattergoriesCategory.Remove(scattergoriesCategory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScattergoriesCategoryExists(Guid id)
        {
            return _context.ScattergoriesCategory.Any(e => e.Id == id);
        }
    }
}
