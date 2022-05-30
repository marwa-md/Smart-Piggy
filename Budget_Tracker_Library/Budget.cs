using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget_Tracker_Library
{
    public class Budget
    {
        public int Id { get; set; }

        public int OwnerId { get; set; }
        public DateTime Date { get; set; }

        public List<Charge> Charges { get; set; }

        public List<Income> Incomes { get; set; }

        public Budget(int ownerId, DateTime date)
        {

            OwnerId = ownerId;
            Date = date;
            Charges = new List<Charge>();
            Incomes = new List<Income>();
        }

        public Budget()
        {
            Charges = new List<Charge>();
            Incomes = new List<Income>();
        }



        public Charge CreateCharge(double value, DateTime date, Category category,
            PaymentMethod paymentMethod, string name)
        {
            Charge newCharge = new Charge(Id, value, date, name, category, paymentMethod);
            return newCharge;
        }

        public void AddCharge(Charge aCharge)
        {
            Charges.Add(aCharge);
        }

        public void DeleteCharge(Charge aCharge)
        {
            Charges.Remove(aCharge);
            // Charges.All(x => x.Equals(aCharge));
        }

        public bool SearchCharge(Charge aCharge)
        {
            return Charges.Contains(aCharge);

        }

        public Income CreateIncome(double value, DateTime date, IncomeTypes type)
        {
            Income newIncome = new Income(Id, value, date, type);
            return newIncome;

        }

        public void AddIncome(Income income)
        {
            Incomes.Add(income);
        }

        public override string ToString()
        {

            string getIncomes = "";
            foreach (Income income in Incomes)
            {
                getIncomes += income.ToString() + "\n\n";
            }
            string getCharges = "";
            foreach (Charge charge in Charges)
            {
                getCharges += charge.ToString() + "\n\n";
            }
            return $"Date : {Date.ToString("MMMM")} {Date.Year}\nIncome :\n{getIncomes}\nExpenses :\n{getCharges}";
        }

        public List<Expense> SortExpenses()
        {
            List<Expense> expenses = new List<Expense>();
            expenses.AddRange(Incomes);
            expenses.AddRange(Charges);

            BudgetManager sortByDate = new BudgetManager();
            expenses.Sort(sortByDate);


            return expenses;


        }


    }
}
