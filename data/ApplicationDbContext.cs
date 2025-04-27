using Microsoft.EntityFrameworkCore;
using ExpenseTrackerApi.Models;

namespace ExpenseTrackerApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Expense> Expenses { get; set; }
    }
}