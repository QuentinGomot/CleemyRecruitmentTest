using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cleemy.Models
{
    public class ExpenseDTO
    {
        public int ID { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public string Date { get; set; }
        [Required]
        public string Comment { get; set; }
        [Required]
        public string UserFullName { get; set; }
        [Required]
        public float Amount { get; set; }
        [Required]
        public string Currency { get; set; }
        [Required]
        public string Nature { get; set; }
    }
}
