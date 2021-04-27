using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cleemy.Models
{
    public class User
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int CurrencyID { get; set; }

        public Currency Currency { get; set; }

        public ICollection<Expense> Expenses { get; set; }
    }
}
