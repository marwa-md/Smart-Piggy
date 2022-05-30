using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget_Tracker_Library
{

    public class Income : Expense
    {
        public IncomeTypes Type { get; set; }

        public Income(int budgetId, double value, DateTime date, IncomeTypes type)
            : base(budgetId, value, date)
        {
            Type = type;

        }

        public Income()
        {

        }

        public override string ToString()
        {
            return $"{Type}";
        }


    }
}
