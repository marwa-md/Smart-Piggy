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
    /// Interaction logic for AddIncome.xaml
    /// </summary>
    public partial class AddIncome : Window
    {
        public Budget budget;

        BudgetDatabaseContext BudgetDB;

        public int EditId = 0;

        public delegate void DataChangedEventHandler(object sender, EventArgs e);

        public event DataChangedEventHandler DataChanged;

        public event SelectionChangedEventHandler SelectedDateChanged;

        public AddIncome(Budget budget, BudgetDatabaseContext BudgetDB)
        {
            InitializeComponent();
            type.ItemsSource = Enum.GetValues(typeof(IncomeTypes));
            date.Text = DateTime.Now.ToString();
            this.budget = budget;
            this.BudgetDB = BudgetDB;

        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            if (main.CheckAllEmptyFields().Contains(false))
            {
                errorMessage.Content = "Please fill all fields";
                MessageBox.Show(errorMessage.Content.ToString());

            }
            else
            {
                DateTime getDate = date.SelectedDate.Value;
                if (Manager.SetDateRange(getDate) == true)
                {
                    AddEditIncome(getDate);
                }

            }



        }

        private void clear_Click(object sender, RoutedEventArgs e)
        {
            main.ClearAllControls();
            errorMessage.Content = " ";
        }

        private void AddEditIncome(DateTime getDate)
        {
            try
            {
                IncomeTypes getIncomeType = (IncomeTypes)type.SelectedItem;
                Double getDouble = Convert.ToDouble(amount.Text);
                if (EditId == 0)
                {
                    BudgetDB.Incomes.Add(budget.CreateIncome(getDouble, getDate, getIncomeType));
                    BudgetDB.SaveChanges();

                    MessageBox.Show("Income Added Successfully!");

                }
                else
                {
                    var query =
                   from income in BudgetDB.Incomes
                   where income.Id == EditId
                   select income;

                    foreach (Income income in query)
                    {
                        income.Value = getDouble;
                        income.Date = getDate;
                        income.Type = getIncomeType;
                    }
                    BudgetDB.SaveChanges();
                    //     EntryUpdated++;
                    MessageBox.Show("Income updated Successfully!");
                }
                SaveChangedData();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Amount can only be in numbers ");
            }
        }




        private void SaveChangedData()
        {
            DataChangedEventHandler handler = DataChanged;
            if (handler != null)
            {
                handler(this, new EventArgs());
            }
        }

        private void date_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DateTime getDate = (DateTime)date.SelectedDate;

                Manager.SetDateWarning(getDate, errorMessage);
            }
            catch
            {

            }

        }


    }

}

