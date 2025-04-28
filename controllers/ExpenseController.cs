using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ExpenseTrackerApi.Data;
using ExpenseTrackerApi.Models;


namespace ExpenseTrackerApi.Controllers
{
    [Authorize]
    [Route("/api")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        private static readonly string[] _categories = new[]
        {
            "Food",
            "Transport",
            "Entertainment",
            "Eletronics",
            "Clothing",
            "Utilities",
            "Groceries",
            "Health",
            "Other"
        };

        public ExpenseController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;

        }

        [HttpGet]
        public IActionResult TestController()
        {
            if (!_env.IsDevelopment())
            {
                return NotFound();
            }
            return Ok("Hello from ExpenseController!");
        }

        [HttpGet("{id}")]
        public IActionResult GetExpenseById(int id)
        {
            var expense = _context.Expenses.Find(id);
            if (expense == null)
            {
                return NotFound();
            }
            return Ok(expense);
        }

        [HttpGet("all")]
        public IActionResult GetAllExpenses(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] string? range)
        {

            IQueryable<Expense> expenses = _context.Expenses;

            if (startDate.HasValue && endDate.HasValue)
            {
                expenses = expenses.Where(e => e.Date >= startDate.Value && e.Date <= endDate.Value);
            }
            else if (!string.IsNullOrEmpty(range))
            {
                DateTime today = DateTime.Today;
                switch (range.ToLower())
                {
                    case "week":
                        DateTime startOfWeek = today.AddDays(-(int)today.DayOfWeek);
                        expenses = expenses.Where(e => e.Date >= startOfWeek && e.Date <= today);
                        break;
                    case "month":
                        DateTime MonthAgo = today.AddMonths(-1);
                        expenses = expenses.Where(e => e.Date >= MonthAgo && e.Date <= today);
                        break;
                    case "3 months":
                        DateTime threeMonthsAgo = today.AddMonths(-3);
                        expenses = expenses.Where(e => e.Date >= threeMonthsAgo && e.Date <= today);
                        break;
                    case "6 months":
                        DateTime sixMonthsAgo = today.AddMonths(-6);
                        expenses = expenses.Where(e => e.Date >= sixMonthsAgo && e.Date <= today);
                        break;
                    case "year":
                            DateTime yearAgo = today.AddYears(-1);
                        expenses = expenses.Where(e => e.Date >= yearAgo && e.Date <= today);
                        break;
                    default:
                        return BadRequest("Invalid range. Valid ranges are: week, 3months.");
                }
            }

            var results = expenses.ToList();

            if (expenses == null || !expenses.Any())
            {
                return NotFound();
            }
            return Ok(results);
        }

        [HttpPost]
        public IActionResult CreateExpense([FromBody] Expense expense)
        {

            if (expense == null)
            {
                return BadRequest("Expense cannot be null");
            }

            if (!_categories.Contains(expense.Category))
            {
                return BadRequest($"Invalid category. Valid categories are: {string.Join(", ", _categories)}");
            }

            _context.Expenses.Add((Expense)expense);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetExpenseById), new { id = expense.Id }, expense);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateExpenseById([FromBody] Expense updatedExpense, int id)
        {
            var expense = _context.Expenses.Find(id);
            if (expense == null)
            {
                return NotFound();
            }

            expense.Name = updatedExpense.Name ?? expense.Name;
            expense.Amount = updatedExpense.Amount != 0 ? updatedExpense.Amount : expense.Amount;
            expense.Date = updatedExpense.Date;
            expense.Category = updatedExpense.Category ?? expense.Category;

            _context.SaveChanges();
            return NoContent();
        }


        [HttpDelete("all")]
        public IActionResult DeleteAllExpenses()
        {
            if (!_env.IsDevelopment())
            {
                return Forbid();
            }
            var expenses = _context.Expenses.ToList();
            _context.RemoveRange(_context.Expenses);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteExpenseById(int id)
        {
            var expense = _context.Expenses.Find(id);
            if (expense == null)
            {
                return NoContent();
            }
            _context.Expenses.Remove(expense);
            _context.SaveChanges();

            return NoContent();
        
    }
}
}