using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace DotNetLab8.Controllers
{
    public class GameController : Controller
    {
        private readonly ILogger<GameController> _logger;
        private static readonly Random random = new Random();
        private static int upperRange = 30;
        private static int randValue = -1;
        private static int nOfProbes = 0;

        public GameController(ILogger<GameController> logger)
        {
            _logger = logger;
        }

        public IActionResult Set(int n)
        {
            if (n > 0)
            {
                reset(n);
                ViewBag.Message = $"Set upper range to {n}";
                ViewBag.Style = "correct";
            } else
            {
                reset();
                ViewBag.Message = $"Please provide integer value greater than 0";
                ViewBag.Style = "incorrect";
            }
            
            return View();
        }

        private void reset(int n = 30)
        {
            upperRange = n;
            nOfProbes = 0;
        }

        public IActionResult Draw()
        {
            randValue = random.Next(upperRange);
            nOfProbes = 0;
            _logger.LogDebug($"Draw {randValue}");
            return View();
        }

        public IActionResult Guess(int userValue)
        {
            ViewBag.Guess = userValue;
            ViewBag.Info = $"Guessing from range 0 to {upperRange - 1} ({randValue})";
            if (userValue <= 0 || userValue >= upperRange){
                ViewBag.Message = "Out Of Range!";
                ViewBag.Style = "out_of_range";
                ViewBag.TextStyle = "out_of_range_text";
            }
            else if (userValue < randValue)
            {
                ViewBag.Message = "Too Low!";
                ViewBag.Style = "too_low";
                ViewBag.TextStyle = "too_low_text";
            } else if (userValue > randValue)
            {
                ViewBag.Message = "Too High!";
                ViewBag.Style = "too_high";
                ViewBag.TextStyle = "too_high_text";
            } else
            {
                ViewBag.Message = "Bingo!";
                ViewBag.Style = "guess";
                ViewBag.TextStyle = "guess_text";
            }
            nOfProbes += 1;
            ViewBag.NProbes = nOfProbes;
            return View();
        }
    }
}
