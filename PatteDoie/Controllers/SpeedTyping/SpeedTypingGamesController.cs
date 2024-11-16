﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatteDoie.Models.SpeedTyping;
using PatteDoie.Queries.SpeedTyping;
using PatteDoie.Rows.SpeedTypingGame;
using PatteDoie.Services.SpeedTyping;

namespace PatteDoie.Controllers.SpeedTyping
{
    public class SpeedTypingGamesController : Controller
    {
        private readonly PatteDoieContext _context;

        private readonly ISpeedTypingService _service;
        public SpeedTypingGamesController(PatteDoieContext context, ISpeedTypingService speedTypingService)
        {
            _context = context;
            _service = speedTypingService;
        }

        // GET: SpeedTypingGames
        public async Task<IActionResult> Index()
        {
            return View(await _context.SpeedTypingGame.ToListAsync());
        }

        // GET: SpeedTypingGames/GetGame/5
        public async Task<ActionResult<SpeedTypingGameRow>> GetGame(Guid id)
        {
            var game = await _service.GetGame(id);


            if (game == null)
            {
                return NotFound();
            }

            return game;
        }

        // GET: SpeedTypingGames/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SpeedTypingGames/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<SpeedTypingGameRow>> Create(CreateSpeedTypingGameCommand command)
        {
            var speeedTypingGame_created = await _service.CreateGame(command, []);

            return CreatedAtAction("GetGame", new { id = speeedTypingGame_created.Id }, speeedTypingGame_created);
        }

        // GET: SpeedTypingGames/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var speedTypingGame = await _context.SpeedTypingGame.FindAsync(id);
            if (speedTypingGame == null)
            {
                return NotFound();
            }
            return View(speedTypingGame);
        }

        // POST: SpeedTypingGames/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,LaunchTime")] SpeedTypingGame speedTypingGame)
        {
            if (id != speedTypingGame.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(speedTypingGame);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpeedTypingGameExists(speedTypingGame.Id))
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
            return View(speedTypingGame);
        }

        // GET: SpeedTypingGames/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var speedTypingGame = await _context.SpeedTypingGame
                .FirstOrDefaultAsync(m => m.Id == id);
            if (speedTypingGame == null)
            {
                return NotFound();
            }

            return View(speedTypingGame);
        }

        // POST: SpeedTypingGames/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var speedTypingGame = await _context.SpeedTypingGame.FindAsync(id);
            if (speedTypingGame != null)
            {
                _context.SpeedTypingGame.Remove(speedTypingGame);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SpeedTypingGameExists(Guid id)
        {
            return _context.SpeedTypingGame.Any(e => e.Id == id);
        }
    }
}
