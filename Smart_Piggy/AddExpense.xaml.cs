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
    /// Interaction logic for AddExpense.xaml
    /// </summary>
    public partial class AddExpense : Window
    {
        public Budget budget;

        BudgetDatabaseContext BudgetDB;

        public int EditId = 0;
        public delegate void DataChangedEventHandler(object sender, EventArgs e);

        public event DataChangedEventHandler ChargeDataChanged;

        public event SelectionChangedEventHandler SelectedDateChanged;

        public AddExpense(Budget budget, BudgetDatabaseContext BudgetDB)
        {
            InitializeComponent();
            expenseCategory.ItemsSource = Enum.GetValues(typeof(Category));
            paymentMethod.ItemsSource = Enum.GetValues(typeof(PaymentMethod));
            expenseDate.Text = DateTime.Now.ToString();
            this.budget = budget;
            this.BudgetDB = BudgetDB;
        }


        private void AddSave(object sender, RoutedEventArgs e)
        {
            if (main.CheckAllEmptyFields().Contains(false))
            {
                errorMessage.Content = "Please fill all fields";
                MessageBox.Show(errorMessage.Content.ToString());

            }
            else
            {
                DateTime getDate = expenseDate.SelectedDate.Value;
                if (Manager.SetDateRange(getDate) == true)
                {
                    AddNewExpense(getDate);
                }

            }
        }


        private void AddNewExpense(DateTime getDate)
        {
            try
            {
                Double getDouble = Convert.ToDouble(amount.Text);

                Category getCategory = (Category)expenseCategory.SelectedItem;

                PaymentMethod getPaymentMethod = (PaymentMethod)paymentMethod.SelectedItem;

                string getName = expenseName.Text;
                if (EditId == 0)
                {
                    BudgetDB.Charges.Add(budget.CreateCharge(getDouble, getDate, getCategory, getPaymentMethod, getName));
                    BudgetDB.SaveChanges();

                    MessageBox.Show("Charge Added Successfully!");
                }
                else
                {
                    var query =
                    from charge in BudgetDB.Charges
                    where charge.Id == EditId
                    select charge;

                    foreach (Charge charge in query)
                    {
                        charge.Value = getDouble;
                        charge.Date = getDate;
                        charge.Category = getCategory;
                        charge.PaymentMethod = getPaymentMethod;
                        charge.Name = getName;
                    }
                    BudgetDB.SaveChanges();
                    //     EntryUpdated++;
                    MessageBox.Show("Charge updated Successfully!");
                }

                SaveChangedData();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Amount can only be in numbers ");
            }

        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {

            main.ClearAllControls();
            errorMessage.Content = " ";

        }

        private void SaveChangedData()
        {
            DataChangedEventHandler handler = ChargeDataChanged;
            if (handler != null)
            {
                handler(this, new EventArgs());
            }
        }

        private void expenseDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DateTime getDate = (DateTime)expenseDate.SelectedDate;

                Manager.SetDateWarning(getDate, errorMessage);
            }
            catch
            {

            }
        }
    }
}


