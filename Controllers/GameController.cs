using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace DotNetLab8.Controllers
{
    public class GameController : Controller
    {
        private readonly ILogger<GameController> _logger;
        private static readonly Random random = new Random(42);
        private static readonly int DEFAULT_UPPER_RANGE = 30;

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
            HttpContext.Session.SetInt32("upperRange", n);
            HttpContext.Session.SetInt32("nOfProbes", 0);
            HttpContext.Session.Remove("randValue");

        }

        public IActionResult Draw()
        {
            int upperRange = HttpContext.Session.GetInt32("upperRange").GetValueOrDefault(DEFAULT_UPPER_RANGE);
            int randValue = random.Next(upperRange);
            HttpContext.Session.SetInt32("randValue", randValue);
            HttpContext.Session.SetInt32("nOfProbes", 0);

            return View();
        }

        public IActionResult Guess(int userValue)
        {

            int upperRange = HttpContext.Session.GetInt32("upperRange").GetValueOrDefault(DEFAULT_UPPER_RANGE);
            int randValue = HttpContext.Session.GetInt32("randValue").GetValueOrDefault(-1);
            int nOfProbes = HttpContext.Session.GetInt32("nOfProbes").GetValueOrDefault(0);


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
            HttpContext.Session.SetInt32("nOfProbes", nOfProbes);
            ViewBag.NProbes = nOfProbes;
            return View();
        }
    }
}
