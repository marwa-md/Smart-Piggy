using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget_Tracker_Library
{
    public enum Category
    {
        RentMortgage,
        Utilities,
        Net,
        Mobile,
        Phone,
        Groceries,
        Medications,
        Entertainment,
        Clothes,
        SelfCare,
        Car,
        Insurance,
        Loans,
        Investment,
        PetCare,
        Other
    }

    public enum IncomeTypes
    {
        Salary,
        Investment,
        Loan,
        Savings,
        Other
    }

    public enum PaymentMethod
    {
        Visa,
        Debit,
        Cash,
        Cheque
    }

    internal class BudgetManager : IComparer<Expense>
    {
        public int Compare(Expense x, Expense y)
        {
            return x.Date.CompareTo(y.Date);
        }
    
    }
}
