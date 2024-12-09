using Microsoft.AspNetCore.Mvc;
using BlackJackWEB.Models;

namespace BlackJackWEB.Controllers
{
    public class GameController : Controller
    {
        private static Game _game;

        public GameController()
        {
            if (_game == null)
            {
                _game = new Game(1000m);
            }
        }

        public IActionResult Index()
        {
            return View(_game);
        }

        [HttpPost]
        public IActionResult StartGame(decimal betAmount)
        {
            if (betAmount > 0 && betAmount <= _game.Player.Balance)
            {
                if (_game.IsGameOver())
                {
                    _game.UpdateBalance();
                }
                _game.StartGame(betAmount);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult PlayerHit()
        {
            if (!_game.IsGameOver())
            {
                _game.PlayerHit();
                if (_game.IsGameOver())
                {
                    _game.ResolveGame();
                    _game.UpdateBalance();
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult PlayerStay()
        {
            if (!_game.IsGameOver())
            {
                _game.DealerTurn();
                _game.ResolveGame();
                _game.UpdateBalance();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult PlayerDoubleDown()
        {
            if (!_game.IsGameOver() && _game.Player.CanDoubleDown())
            {
                _game.DoubleDown();
                _game.DealerTurn();
                _game.ResolveGame();
                _game.UpdateBalance();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult PlayerSplit()
        {
            if (!_game.IsGameOver() && _game.Player.CanSplit())
            {
                _game.SplitHand();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult PlayAgain()
        {
            if (_game.IsGameOver())
            {
                _game.UpdateBalance();
                _game.Player.ClearHands();
                _game.Dealer.ClearHand();
                // Reset current bet to 0 to show betting form
                _game = new Game(_game.Player.Balance);
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult PlaceInsuranceBet()
        {
            if (!_game.IsGameOver() && _game.Dealer.Hand.GetCards()[0].Value == 11)
            {
                _game.PlaceInsuranceBet();
            }
            return RedirectToAction("Index");
        }
    }
}