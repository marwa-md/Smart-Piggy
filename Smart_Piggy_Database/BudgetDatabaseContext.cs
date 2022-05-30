using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Budget_Tracker_Library;

namespace Smart_Piggy_Database
{
    public class BudgetDatabaseContext :DbContext
    {
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<Charge> Charges { get; set; }
        public DbSet<Income> Incomes { get; set; }
        public DbSet<Owner> Owners { get; set; }
    }
}
