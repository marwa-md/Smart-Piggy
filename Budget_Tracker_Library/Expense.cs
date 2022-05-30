using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget_Tracker_Library
{
   
    public class Expense
    {

        public int Id { get; set; }

        public int BudgetId { get; set; }
        public DateTime Date { get; set; }

        public double Value { get; set; }

        public Expense(int BudgetId, double value, DateTime date)
        {
            this.BudgetId = BudgetId;
            Date = date;
            Value = value;

        }

        public Expense()
        {

        }

        public override string ToString()
        {
            return $"Date : {Date}\nValue : {Value:F2}";
        }

    }
}
