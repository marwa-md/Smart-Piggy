using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget_Tracker_Library
{

    public class Charge : Expense
    {
        public string Name { get; set; }
        public Category Category { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

        public Charge(int BudgetId, double value, DateTime date, string name, Category category, PaymentMethod paymentMethod)
            : base(BudgetId, value, date)
        {
            Name = name;
            PaymentMethod = paymentMethod;
            Category = category;
        }

        public Charge()
        {

        }

        public override string ToString()
        {
            return $"Name : {Name}\nCategory : {Category}\nPaid with : {PaymentMethod}";
        }
    }
}
