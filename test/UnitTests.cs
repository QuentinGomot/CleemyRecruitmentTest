using Cleemy.Models;
using System;
using Xunit;

namespace CleemyUnitTest
{
    public class UnitTests
    {
        [Theory]
        [InlineData("02/03/2010", false)]
        [InlineData("25/04/2021", true)]
        [InlineData("02/03/2030", false)]
        public void TestIsValidDate(string date, bool expectedResult)
        {
            DateTime newDdate = Convert.ToDateTime(date);

            var expense = new Expense
            {
                Date = newDdate
            };
            Assert.Equal(expense.IsValidDate(), expectedResult);
        }
    }
}
