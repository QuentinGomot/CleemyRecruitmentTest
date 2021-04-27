using Cleemy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cleemy.Data
{
    public class DbInitializer
    {
        public static void Initialize(PurchaseContext context)
        {
            context.Database.EnsureCreated();

            if (context.Users.Any())
            {
                return;
            }

            var natures = new Nature[]
            {
                new Nature
                {
                    ID = 1,
                    Label = "Hotel"
                },
                new Nature
                {
                    ID = 2,
                    Label = "Restaurant"
                },
                new Nature
                {
                    ID = 3,
                    Label = "Misc"
                }
            };
            foreach (Nature n in natures)
            {
                context.Natures.Add(n);
            }
            context.SaveChanges();

            var currencies = new Currency[]
            {
                new Currency
                {
                    ID = 1,
                    Code = "EUR"
                },
                new Currency
                {
                    ID = 2,
                    Code = "USD"
                },
                new Currency
                {
                    ID = 3,
                    Code = "RUB"
                }
            };
            foreach (Currency c in currencies)
            {
                context.Currencies.Add(c);
            }
            context.SaveChanges();

            var users = new User[]
            {
                new User
                {
                    FirstName = "Anthony",
                    LastName = "Stark",
                    CurrencyID = 2
                },
                new User
                {
                    FirstName = "Natasha",
                    LastName = "Romanova",
                    CurrencyID = 3
                }
            };
            foreach (User u in users)
            {
                context.Users.Add(u);
            }
            context.SaveChanges();
        }
    }
}
