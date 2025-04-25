using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTrackerApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        // GET: api/expense
        [HttpGet]
        public IActionResult GetExpenses()
        {
            // Logic to get expenses
            return Ok("Hello from ExpenseController!");
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            return Ok($"Getting expense with ID: {id}");
        }

        // POST: api/expense
        [HttpPost]
        public IActionResult CreateExpense([FromBody] object expense)
        {
            // Logic to create an expense
            return CreatedAtAction(nameof(GetExpenses), new { id = 1 }, expense);
        }

        [HttpPost]
        public IActionResult CreateExpenseWithId(int id, [FromBody] object expense)
        {
            // Logic to create an expense with a specific ID
            return CreatedAtAction(nameof(GetById), new { id }, expense);
        }
    }
}