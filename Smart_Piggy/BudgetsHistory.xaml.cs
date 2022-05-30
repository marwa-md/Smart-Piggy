using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Budget_Tracker_Library;
using Smart_Piggy_Database;

namespace Smart_Piggy
{
    /// <summary>
    /// Interaction logic for BudgetsHistory.xaml
    /// </summary>
    public partial class BudgetsHistory : Window
    {
        private Owner owner;
        private List<Budget> budgets;
        BudgetDatabaseContext BudgetDB;


        public event SelectionChangedEventHandler SelectionChanged;

        public BudgetsHistory(Owner owner, BudgetDatabaseContext BudgetDB)
        {
            InitializeComponent();
            this.owner = owner;
            PopulateData(BudgetDB);
            this.BudgetDB = BudgetDB;

        }


        private void DisplayBudget(object sender, SelectionChangedEventArgs args)
        {
            greeting.Content += history.SelectedItem.ToString();
            DisplayAll();
        }

        private void DisplayAll()
        {

            if (history.SelectedItem != null)
            {
                var selectedBudget = budgets.Where(x => x.Date.ToString("MMMM,yyyy").Equals(history.SelectedItem)).FirstOrDefault();

                budgetLog.ItemsSource = Manager.GetDataTable(selectedBudget.SortExpenses()).DefaultView;
            }
        }

        private void PopulateData(BudgetDatabaseContext BudgetDB)
        {
            budgets = Manager.LoadBudgets(owner.Id, BudgetDB);
            foreach (Budget budget in budgets)
            {
                budget.LoadExpenses(budget.Date,BudgetDB);

            }

            history.ItemsSource = budgets.Select(x => x.Date.ToString("MMMM,yyyy"));

        }

        private void Done_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        //   public void RefreshAll(object sender, EventArgs e)
        // {
        //     DisplayAll();    

        //  }

        public void RefreshAll()
        {
            DisplayAll();

        }
    }
}

