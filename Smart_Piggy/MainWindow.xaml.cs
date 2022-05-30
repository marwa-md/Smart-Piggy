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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Budget_Tracker_Library;
using Smart_Piggy_Database;

namespace Smart_Piggy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static List<Budget> Budgets;
        public static Budget currentBudget;

        BudgetDatabaseContext BudgetDB;

        public Owner Owner { get; set; }
        BudgetsHistory budgetsHistory;
        ViewLog log;
        public MainWindow(Owner Owner, BudgetDatabaseContext BudgetDB)
        {
            InitializeComponent();
            this.Owner = Owner;
            this.BudgetDB = BudgetDB;
            this.budgetsHistory = new BudgetsHistory(Owner, BudgetDB);
            MainWindow_Load();
            this.log = new ViewLog(currentBudget, Owner, BudgetDB, this);
            this.Closed += Manager.CloseAllWindows;
        }


       
        private void FindOrCreateBudget()
        {
            Budgets = Manager.LoadBudgets(Owner.Id, BudgetDB);
            if (Budgets.Where(x => x.Date.Month == DateTime.Now.Month).FirstOrDefault() == null)
            {
                currentBudget = new Budget(Owner.Id, DateTime.Now);
                BudgetDB.Budgets.Add(currentBudget);
                BudgetDB.SaveChanges();
            }
            else
            {
                currentBudget = Budgets.Where(x => x.Date.Month == DateTime.Now.Month).FirstOrDefault();

            }
        }

        private void AddIncome_Click(object sender, RoutedEventArgs e)
        {
            AddIncome newIncomeWindow = new AddIncome(currentBudget, BudgetDB);
            newIncomeWindow.DataChanged += DataChanged;
            // newIncomeWindow.DataChanged += budgetsHistory.RefreshAll;

            newIncomeWindow.Show();
        }

        private void MainWindow_Load()
        {
            greetingLabel.Content += char.ToUpper(Owner.FirstName[0]) + Owner.FirstName.Substring(1);
            todayDate.Content += "  " + DateTime.Now.ToShortDateString();


            FindOrCreateBudget();
            currentBudget.LoadExpenses(DateTime.Now, BudgetDB);
            thisMonth.Content += " " + currentBudget.Date.ToString("MMMM yyyy");
            MakeChart();
            DisplayStats();
        }
        private void AddExpense_Click(object sender, RoutedEventArgs e)
        {
            AddExpense newExpenseWindow = new AddExpense(currentBudget, BudgetDB);
            newExpenseWindow.ChargeDataChanged += DataChanged;
            //     newExpenseWindow.ChargeDataChanged += budgetsHistory.RefreshAll;
            newExpenseWindow.Show();
        }



        private void DataChanged(object sender, EventArgs e)
        {

            UpdateAll();
        }

        public void UpdateAll()
        {
            currentBudget.LoadExpenses(currentBudget.Date, BudgetDB);
            DisplayStats();
            log.DisplayBudget();
            budgetsHistory.RefreshAll();
            MakeChart();
        }

        private void DisplayStats()
        {
            displayIncome.Content = currentBudget.Incomes.CalculateTotal();
            displaySpent.Content = currentBudget.Charges.CalculateTotal();
            displayRemaining.Content = Manager.CalculateRemaining(currentBudget);
        }

        private void ViewHistory_Click(object sender, RoutedEventArgs e)
        {
            bool isOpen = Manager.IsOpen(budgetsHistory);
            if (isOpen == true)
            {
                budgetsHistory.Show();
            }
            else
            {
                budgetsHistory = new BudgetsHistory(Owner, BudgetDB);
                budgetsHistory.Show();
            }

        }

        private void MakeChart()
        {

           List<Charge> charges = BudgetDB.Charges.Where(charge => charge.Date.Month == DateTime.Now.Month && charge.BudgetId == currentBudget.Id).ToList();
            pieChart.DataContext = FindChartCategories(charges);

        }

        private  List<KeyValuePair<string, double>> FindChartCategories(List <Charge> charges)
        {

            List<Category> categories = new List<Category>();
            foreach (Charge charge in charges)
            {
                if (!categories.Contains(charge.Category))
                {
                    categories.Add(charge.Category);
                }
            }
            List<KeyValuePair<string, double>> chartList = new List<KeyValuePair<string, double>>();

            //  Dictionary<ChartCategories,double > chartCategories = new Dictionary<ChartCategories,double>();
            foreach (Category item in categories)
            {

                chartList.Add(new KeyValuePair<string, double>(item.ToString(), Math.Round(Manager.CalculatePercentage(charges, item))));
            }
            return chartList;
        }
        private void ViewBudget_Click(object sender, RoutedEventArgs e)
        {
            bool isOpen = Manager.IsOpen(log);
            if (isOpen == true)
            {
                log.Show();
            }
            else
            {
                log = new ViewLog(currentBudget, Owner, BudgetDB, this);

                log.DataChanged += DataChanged;
                //   log.DataChanged += budgetsHistory.RefreshAll;
                log.Show();
            }
        }

        private void EditUsers_Click(object sender, RoutedEventArgs e)
        {
            EditUsers editUsers = new EditUsers(BudgetDB, Owner);
            editUsers.Show();
        }
        //private void CloseAll()
        //{
        //   Manager.CloseAllWindows();
        //}
        //public  bool IsOpen(Window window)
        //{
        //    return Application.Current.Windows.Cast<Window>().Any(x => x == window);
        //}

        //public static void UpdateAll()
        //{
        //    if (Manager.IsOpen(log)== true)
        //    {

        //    }

        //}
    }
}


