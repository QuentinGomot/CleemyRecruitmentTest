using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cleemy.Models
{
    public class Expense
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public DateTime Date { get; set; }
        public int AmountID { get; set; }
        public int NatureID { get; set; }
        public string Comment { get; set; }

        public User User { get; set; }
        public Amount Amount { get; set; }
        public Nature Nature { get; set; }

        public bool IsValidDate()
        {
            return Date >= DateTime.Now.AddMonths(-3) && Date <= DateTime.Now;
        }

        public bool IsValidCurrency()
        {
            return Amount.Currency.Equals(User.Currency);
        }

        //public bool IsNotDuplicate()
        //{
        //    return Amount.Currency.Equals(User.Currency);
        //}
    }
}
