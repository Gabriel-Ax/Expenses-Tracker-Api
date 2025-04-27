using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ExpenseTrackerApi.Data;
using ExpenseTrackerApi.Models;


namespace ExpenseTrackerApi.Controllers
{
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
        public IActionResult GetExpenses()
        {
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

        [HttpPost("new/no-async")]
        public IActionResult CreateExpenseNoAsync([FromBody] Expense expense)
        {
            _context.Expenses.Add((Expense)expense);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetExpenseById), new { id = expense.Id }, expense);
        }

        [HttpPost("new/deprecated")]
        public IActionResult CreateExpenseAsync([FromBody] object expense)
        {
            var expenseCast = (Expense)expense;
            _context.Expenses.Add(expenseCast);
            //await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetExpenseById), new { id = expenseCast.Id }, expense);
        }

        [HttpPost]
        public IActionResult CreateExpenseWithId(int id, [FromBody] object expense)
        {
            return CreatedAtAction(nameof(GetExpenseById), new { id }, expense);
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
        public IActionResult DeleteExpense(int id)
        {
            var expense = _context.Expenses.Find(id);
            if (expense == null)
            {
                return NotFound();
            }

            _context.Expenses.Remove(expense);
            _context.SaveChanges();

            return NoContent();
        
    }
}
}