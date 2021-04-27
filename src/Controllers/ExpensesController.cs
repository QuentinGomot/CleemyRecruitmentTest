using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cleemy.Data;
using Cleemy.Models;

namespace Cleemy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        private readonly PurchaseContext _context;

        public ExpensesController(PurchaseContext context)
        {
            _context = context;
        }

        // GET: api/Expenses
        /// <summary>
        /// Returns a filtered and sorted list of expenses with their attributes
        /// </summary>
        /// <param name="userFullName">The full name of the user results are filtered on : {firstName} {lastName} (optional)</param>
        /// <param name="sortingType">The sorting type for results : {"amount","date"} (optional)</param>
        /// <param name="sortingOrder">The sorting order for results : {"asc","desc"} (optional)</param>
        /// <returns>The list of expenses</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExpenseDTO>>> GetExpenses([FromQuery] string userFullName, [FromQuery] string sortingType, [FromQuery] string sortingOrder)
        {
            IQueryable<Expense> query = _context.Expenses
                .Include(e => e.Nature)
                .Include(e => e.Amount)
                    .ThenInclude(a => a.Currency)
                .Include(e => e.User)
                    .ThenInclude(u => u.Currency);

            // Filtering on user if indicated
            if (userFullName != null)
            {
                query = query.Where(e => e.User.FirstName + " " + e.User.LastName == userFullName);
            }

            // Sorting on amount if indicated
            if (sortingType == "amount")
            {
                if (sortingOrder == "desc")
                {
                    query = query.OrderByDescending(e => e.Amount.Value);
                } else
                {
                    query = query.OrderBy(e => e.Amount.Value);
                }
            }

            // Sorting on date if indicated
            if (sortingType == "date")
            {
                if (sortingOrder == "desc")
                {
                    query = query.OrderByDescending(e => e.Date);
                }
                else
                {
                    query = query.OrderBy(e => e.Date);
                }
            }

            return await query
                .Select(e => ExpenseToDTO(e))
                .ToListAsync();
        }

        // GET: api/Expenses/id
        /// <summary>
        /// Returns a specific expense
        /// </summary>
        /// <param name="id">The id of the expense</param>
        /// <returns>The expense with its attributes</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ExpenseDTO>> GetExpense(int id)
        {
            var expense = await _context.Expenses
                .Include(e => e.Nature)
                .Include(e => e.Amount)
                    .ThenInclude(a => a.Currency)
                .Include(e => e.User)
                    .ThenInclude(u => u.Currency)
                .SingleOrDefaultAsync(e => e.ID == id);

            // No matching expense in DB
            if (expense == null)
            {
                return NotFound();
            }

            return ExpenseToDTO(expense);
        }

        // POST: api/Expenses
        /// <summary>
        /// Create a new expense
        /// </summary>
        /// <param name="expenseDTO">The entity that represents the expense to create
        /// {
        ///     "date" : string
        ///     "userFullName" : string
        ///     "amount" : float
        ///     "currency" : string
        ///     "nature" : string
        ///     "comment" : string
        /// }
        /// </param>
        /// <returns>The created expense with its attributes</returns>
        [HttpPost]
        public async Task<ActionResult<ExpenseDTO>> PostExpense(ExpenseDTO expenseDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Getting expense nature
            var nature = _context.Natures.Where(n => n.Label == expenseDTO.Nature).SingleOrDefault();
            // No matching nature in DB
            if (nature == null)
            {
                return BadRequest("No nature is matching : " + expenseDTO.Nature);
            }

            // Getting expense currency
            var currency = _context.Currencies.Where(c => c.Code == expenseDTO.Currency).SingleOrDefault();
            // No matching currency in DB
            if (currency == null)
            {
                return BadRequest("No currency is matching : " + expenseDTO.Nature);
            }

            // Getting expense user
            var user = _context.Users.Where(u => u.FirstName + " " + u.LastName == expenseDTO.UserFullName).SingleOrDefault();
            // No matching user in DB
            if (user == null)
            {
                return BadRequest("No user is matching : " + expenseDTO.UserFullName);
            }

            // Getting expense date
            DateTime date;
            if (!DateTime.TryParse(expenseDTO.Date, out date))
            {
                return BadRequest("Incorrect date format");
            }

            // Creating a new amount
            var amount = new Amount
            {
                Value = expenseDTO.Amount,
                Currency = currency
            };

            // Creating a new expense
            var expense = new Expense
            {
                ID = expenseDTO.ID,
                User = user,
                Nature = nature,
                Amount = amount,
                Comment = expenseDTO.Comment,
                Date = date
            };

            // Date is older than three months or in the futur
            if (!expense.IsValidDate())
            {
                return BadRequest("The date must not be more than three months old or in the futur");
            }

            // No matching between user currency and expense currency
            if (!expense.IsValidCurrency())
            {
                return BadRequest("The expense currency must be the same as the currency chosen by the user");
            }

            // Testing duplicate expense in DB
            var duplicateExpense = _context.Expenses.Any(e => (e.Date == date && e.Amount.Value == expenseDTO.Amount));
            // Duplicate expense in DB
            if (duplicateExpense)
            {
                return BadRequest("An identical expense already exists in data, two expenses cannot have the same amount and the same date");
            }

            _context.Amounts.Add(amount);
            _context.Expenses.Add(expense);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetExpense", new { id = expense.ID }, ExpenseToDTO(expense));
        }

        // DELETE: api/Expenses/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Expense>> DeleteExpense(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null)
            {
                return NotFound();
            }

            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();

            return expense;
        }

        // Converts a Expense entity in an ExpenseDTO entity
        private static ExpenseDTO ExpenseToDTO(Expense expense)
        {
            return new ExpenseDTO
            {
                ID = expense.ID,
                Date = expense.Date.ToString("d"),
                Comment = expense.Comment,
                UserFullName = expense.User.FirstName + " " + expense.User.LastName,
                Amount = expense.Amount.Value,
                Currency = expense.Amount.Currency.Code,
                Nature = expense.Nature.Label
            };
        }

        private bool ExpenseExists(int id)
        {
            return _context.Expenses.Any(e => e.ID == id);
        }
    }
}
