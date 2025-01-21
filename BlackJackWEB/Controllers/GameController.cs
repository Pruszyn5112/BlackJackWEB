using Microsoft.AspNetCore.Mvc;
using BlackJackWEB.Models;

namespace BlackJackWEB.Controllers
{
    public class GameController : Controller
    {
        private static Game _game;

        [HttpGet]
        public IActionResult Index()
        {
            if (_game == null)
            {
                _game = new Game(initialBalance: 1000);
            }
            return View(_game);
        }

        [HttpPost]
        public IActionResult StartGame(decimal betAmount)
        {
            if (_game.Player.PlaceBet(betAmount))
            {
                _game.StartGame(betAmount);
            }
            else
            {
                TempData["Error"] = "Insufficient balance to place the bet.";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Hit()
        {
            _game.PlayerHit();
            if (_game.IsGameOver())
            {
                TempData["Winner"] = _game.GetWinner();
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Stand()
        {
            _game.PlayerStand();
            if (_game.IsGameOver())
            {
                _game.UpdateBalance();
                TempData["Winner"] = _game.GetWinner();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DoubleDown()
        {
            if (_game.Player.CanDoubleDown())
            {
                _game.DoubleDown();
            }
            else
            {
                TempData["Error"] = "Cannot double down.";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Split()
        {
            if (_game.Player.CanSplit())
            {
                _game.SplitHand();
            }
            else
            {
                TempData["Error"] = "Cannot split hands.";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Restart()
        {
            _game = new Game(initialBalance: _game.Player.Balance);
            return RedirectToAction("Index");
        }
    }
}
