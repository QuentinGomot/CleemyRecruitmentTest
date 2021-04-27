using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cleemy.Models
{
    public class Amount
    {
        public int ID { get; set; }
        public float Value { get; set; }
        public int CurrencyID { get; set; }

        public Currency Currency { get; set; }
    }
}
