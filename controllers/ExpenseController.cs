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
        public IActionResult GetAllExpenses()
        {
            var expenses = _context.Expenses.ToList();
            if (expenses == null || !expenses.Any())
            {
                return NotFound();
            }
            return Ok(expenses);
        }

        [HttpPost]
        public IActionResult CreateExpense([FromBody] Expense expense)
        {
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