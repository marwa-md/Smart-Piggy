using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for ViewLog.xaml
    /// </summary>
    public partial class ViewLog : Window
    {
        Budget Budget;
        Owner Owner;
        BudgetDatabaseContext BudgetDB;
        MainWindow Main;


        public delegate void DataChangedEventHandler(object sender, EventArgs e);

        public event DataChangedEventHandler DataChanged;
        public ViewLog(Budget Budget, Owner Owner, BudgetDatabaseContext BudgetDB, MainWindow main)
        {
            InitializeComponent();
            this.Budget = Budget;
            this.Owner = Owner;
            this.BudgetDB = BudgetDB;
            this.Main = main;
            greetingLabel.Content += char.ToUpper(Owner.FirstName[0]) + Owner.FirstName.Substring(1);
            todayDate.Content += "  " + DateTime.Now.ToShortDateString();
            DisplayBudget();
        }


        public void DisplayBudget()
        {

            displayAll.ItemsSource = Manager.GetDataTable(Budget.SortExpenses()).DefaultView;
        }



        private void done_Click_1(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void UpdateData(object sender, EventArgs e)
        {

            Main.UpdateAll();

        }
        private void edit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView row = (DataRowView)displayAll.SelectedItems[0];
                if (row != null)
                {
                    var id = row["Id"];
                    int i = Convert.ToInt32(id);
                    if (GetEntryType(row).Equals("Income"))
                    {

                        AddIncome editIncome = new AddIncome(Budget, BudgetDB);
                        editIncome.DataChanged += UpdateData;
                        ShowEditDetails(editIncome, i);

                    }
                    else
                    {

                        AddExpense editCharge = new AddExpense(Budget, BudgetDB);
                        editCharge.ChargeDataChanged += UpdateData;
                        ShowEditDetails(editCharge, i);
                    }
                }
            }
            catch (Exception ex)
            {
                
            }



        }



        private void delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView row = (DataRowView)displayAll.SelectedItem;
                if (row != null)
                {
                    if (MessageBox.Show("Are you sure you want to delete this entry ? ", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        DeleteEntry(row);
                        Main.UpdateAll();
                        //   DisplayBudget();
                        // SaveChangedData();
                    }

                }
            }
            catch (Exception ex)
            {
                
            }

        }

        private void ShowEditDetails(AddIncome incomeWindow, int i)
        {
            incomeWindow.incomeTitle.Content = "Edit Income";
            incomeWindow.EditId = i;
            var income = BudgetDB.Incomes.Where(x => x.Id == i).FirstOrDefault();
            incomeWindow.date.Text = income.Date.ToString();
            incomeWindow.amount.Text = income.Value.ToString();
            incomeWindow.type.SelectedItem = income.Type;
            incomeWindow.Title = "Edit Income";
            incomeWindow.Show();
        }

        private void ShowEditDetails(AddExpense expenseWindow, int i)
        {
            expenseWindow.expenseTitle.Content = "Edit Expense";
            expenseWindow.EditId = i;
            var charge = BudgetDB.Charges.Where(x => x.Id == i).FirstOrDefault();
            expenseWindow.expenseDate.Text = charge.Date.ToString();
            expenseWindow.amount.Text = charge.Value.ToString();
            expenseWindow.expenseName.Text = charge.Name;
            expenseWindow.expenseCategory.Text = charge.Category.ToString();
            expenseWindow.paymentMethod.Text = charge.PaymentMethod.ToString();
            expenseWindow.Title = "Edit Expense ";
            expenseWindow.Show();

        }
        private void SaveChangedData()
        {
            DataChangedEventHandler handler = DataChanged;
            if (handler != null)
            {
                handler(this, new EventArgs());
            }
        }

        private void DeleteEntry(DataRowView row)
        {
            try
            {
                var id = row["Id"];
                int i = Convert.ToInt32(id);

                if (GetEntryType(row).Equals("Income"))
                {

                    var entry = BudgetDB.Incomes.Where(x => x.Id == i).FirstOrDefault();
                    BudgetDB.Incomes.Remove(entry);
                    // BudgetDB.SaveChanges();
                    //  MessageBox.Show("Entry removed successfully !");
                }
                else
                {
                    var entry = BudgetDB.Charges.Where(x => x.Id == i).FirstOrDefault();
                    BudgetDB.Charges.Remove(entry);

                }
                BudgetDB.SaveChanges();
                MessageBox.Show("Entry removed successfully !");
            }
            catch
            {

            }


        }

        private string GetEntryType(DataRowView row)
        {
            return row["Type"].ToString();
        }
    }
}
