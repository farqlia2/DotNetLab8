using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace DotNetLab8.Controllers
{
    enum Solution
    {
        TwoSolutions, OneSolution, NoSolution, InfiniteSolutions
    }

 

    public class ToolController : Controller
    {

        static Dictionary<Solution, string> CssStyle = new Dictionary<Solution, string>()
        {
            {Solution.TwoSolutions, "two_solutions" },
            {Solution.OneSolution, "one_solution" },
            {Solution.NoSolution, "no_solution" },
            {Solution.InfiniteSolutions, "infinite_solutions" }
        };

        public IActionResult SolutionPage(double a = 0, double b = 0, double c = 0)
        {
            double[] solution = SolveQuadraticEq(a, b, c);
            Solution solType = GetSolutionType(solution);
            ViewBag.Equation = $"{a} * x <sup>2</sup> + {b} * x + {c}";
            ViewBag.Message = FormatSolutionMessage(solType, solution);
            ViewBag.Style = CssStyle.GetValueOrDefault(solType, "");
            return View();
        }

   

        private static double[] SolveQuadraticEq(double a = 0, double b = 0, double c = 0)
        {
            double delta = Math.Pow(b, 2) - 4 * a * c;
            double[] solution;

            // infinite solutions
            if (a == 0 && b == 0 && c == 0)
            {
                solution = new double[1] { Double.NaN };
            }
            // Case for linear equation 
            else if (a == 0 && b != 0)
            {
                solution = new double[1] { (-c / b) };
            }
            else if (delta > 0)
            {
                double sol1 = (-b + Math.Sqrt(delta)) / (2 * a);
                double sol2 = (-b - Math.Sqrt(delta)) / (2 * a);
                solution = new double[2] { sol1, sol2 };
            }
            else if (delta == 0 && a != 0)
            {
                solution = new double[1] { ((-b) / (2 * a)) };
            }
            else
            {
                solution = Array.Empty<double>();
            }

            return solution;

        }

        private static Solution GetSolutionType(double[] solution)
        {
            switch (solution.Length)
            {
                case 1 when double.IsNaN(solution[0]):
                    return Solution.InfiniteSolutions;
                case 1:
                    return Solution.OneSolution;
                case 2:
                    return Solution.TwoSolutions;
                default:
                    return Solution.NoSolution;
            }
        }

        private static string FormatSolutionMessage(Solution solutionType, double[] solution)
        {
            string message = string.Empty;
            switch (solutionType)
            {
                case Solution.InfiniteSolutions:
                    message= "Infinite solutions possible";
                    break;
                case Solution.OneSolution:
                    message = $"Found solution x = {solution[0]}";
                    break;
                case Solution.TwoSolutions:
                    message = $"Found solutions x = {solution[0]}, x = {solution[1]}";
                    break;
                default:
                    message = "No Solutions";
                    break;
            }
            return message;
        }

    }
}
