using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Budget_Tracker_Library;
using Smart_Piggy_Database;

namespace Smart_Piggy
{
    internal static class Manager
    {
        // check if textbox is empty
        public static bool ValidateTextBox(TextBox textbox)
        {
            return String.IsNullOrEmpty(textbox.Text);
        }

        // check if password is empty
        public static bool ValidatePasswordBox(PasswordBox passwordbox)
        {
            return String.IsNullOrEmpty(passwordbox.Password);
        }

        private static bool ValidateComboBox(ComboBox comboBox)
        {
            return comboBox.SelectedItem == null;

        }

        private static bool ValidateDate(DatePicker datePicker)
        {
            return !datePicker.SelectedDate.HasValue;

        }
        // check all empty controls 
        public static List<bool> CheckAllEmptyFields(this Canvas canvas)
        {
            List<bool> allValidated = new List<bool>();

            for (int i = 0; i < canvas.Children.Count; i++)
            {
                switch (canvas.Children[i])
                {
                    case TextBox _:
                        if (ValidateTextBox((TextBox)canvas.Children[i]) == true) { allValidated.Add(false); }
                        break;

                    case PasswordBox _:
                        if (ValidatePasswordBox((PasswordBox)canvas.Children[i]) == true) { allValidated.Add(false); }
                        break;

                    case ComboBox _:
                        if (ValidateComboBox((ComboBox)canvas.Children[i]) == true) { allValidated.Add(false); }
                        break;


                    case DatePicker _:
                        if (ValidateDate((DatePicker)canvas.Children[i]) == true) { allValidated.Add(false); }
                        break;
                }
            }
            return allValidated;

        }

        //find user by username
        public static Owner FindUser(string userInfo, BudgetDatabaseContext db)
        {
            Owner user = db.Owners.FirstOrDefault(x => x.UserName.Equals(userInfo));
            return user;

        }

        // find user by first and last names
        public static Owner FindUser(string fName, string lName, BudgetDatabaseContext db)
        {
            var user = db.Owners.FirstOrDefault(x => x.FirstName == fName && x.LastName == lName);
            return user;
        }

        //public static dynamic FindData <T>(T obj , BudgetDBContext db , string info)
        //{

        //    switch (obj)
        //    {
        //        case Owner _:
        //            return db.Owners.FirstOrDefault(x => x.UserName == info);

        //        case Budget _: 
        //            return db.Budgets.FirstOrDefault(x => x.Date.Month.ToString() == info);

        //        default: 
        //            return null;   
        //    }
        //}

        // Check if password is correct 
        public static bool CheckPassword(Owner owner, string pass)
        {
            return pass.Equals(owner.Password);
        }

        public static bool IsOpen(Window window)
        {
            return Application.Current.Windows.Cast<Window>().Any(x => x == window);
        }
        //private void deleteAll_Click(object sender, RoutedEventArgs e)
        //{
        //    using (var db = new BudgetDBContext())
        //    {
        //        var data = db.Budgets.Where(x => x.Date.Month.Equals(DateTime.Now.Month)).ToList();
        //        db.Budgets.RemoveRange(data);
        //        var data2 = db.Incomes.Where(x => x.Date.Month.Equals(DateTime.Now.Month)).ToList();
        //        db.Incomes.RemoveRange(data2);

        //        db.SaveChanges();
        //    }
        //}

        // clear all entries
        public static void ClearAllControls(this Canvas main)
        {
            foreach (Control control in main.Children)
            {
                if (control is TextBox)
                {
                    control.ClearValue(TextBox.TextProperty);
                }
                if (control.GetType() == typeof(CheckBox))
                {
                    control.SetValue(CheckBox.IsCheckedProperty, false);
                }
                if (control is ComboBox)
                {
                    control.ClearValue(ComboBox.SelectedItemProperty);
                }
                if (control is DatePicker)
                {
                    control.SetValue(DatePicker.TextProperty, null);
                }
                if (control is PasswordBox)
                {
                    ClearPasswordFields((PasswordBox)control);
                }
            }
        }
        //   private static void ResetDate(DatePicker datePicker)
        //  {
        //     datePicker.Text = DateTime.Now.ToString();

        //  }

        private static void ClearPasswordFields(PasswordBox passwordBox)
        {
            passwordBox.Clear();
        }

        // load budgets for specific owner
        public static List<Budget> LoadBudgets(int ownerId, BudgetDatabaseContext BudgetDB)
        {
            return BudgetDB.Budgets.Where(x => x.OwnerId == ownerId).ToList();

        }

        public static void LoadExpenses(this Budget currentBudget, DateTime date, BudgetDatabaseContext db)
        {
            var incomes = db.Incomes.Where(x => x.BudgetId == currentBudget.Id && x.Date.Month.Equals(date.Month)).ToList();
            currentBudget.Incomes = incomes;
            var charges = db.Charges.Where(x => x.BudgetId == currentBudget.Id && x.Date.Month.Equals(date.Month)).ToList();
            currentBudget.Charges = charges;
        }

        public static double CalculateTotal<T>(this List<T> values) where T : Expense
        {
            return values.Select(x => x.Value).Sum();
        }


        public static double CalculateRemaining(Budget budget)
        {
            return budget.Incomes.CalculateTotal() - budget.Charges.CalculateTotal();

        }

        public static bool SetDateRange(DateTime getDate)
        {


            if (getDate.ToString("MMMM yyyy") != DateTime.Now.ToString("MMMM yyyy"))
            {
                MessageBox.Show($"Date entered has to be for current month only !\n e.g:Any date within {DateTime.Now.ToString("MMMM yyyy")} ");
                return false;
            }

            else
            {
                return true;
            }
        }

        public static void SetDateWarning(DateTime getDate, Label errorMessage)
        {
            DateTime dateToCompare = new DateTime(getDate.Year, getDate.Month, getDate.Day);
            DateTime dateNow = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            int i = DateTime.Compare(dateToCompare, dateNow);
            if (i < 0)
            {
                errorMessage.Content = "Warning : The date is set to some previous date !";
            }

            else if (i > 0)
            {
                errorMessage.Content = "Warning : The date is set to some future date !";
            }
            else
            {
                errorMessage.Content = " ";
            }
        }

        public static DataTable GetDataTable(List<Expense> expenses)
        {
            DataTable dt = new DataTable();
            string[] columns = { "Id", "Date", "Type", "Description", "Debit", "Credit", "Balance" };
            for (int i = 0; i < columns.Length; i++)
            {
                dt.Columns.Add(columns[i]);
            }

            double balance = 0;

            foreach (Expense expense in expenses)
            {
                if (expense.GetType() == typeof(Income))
                {
                    balance += expense.Value;
                    dt.Rows.Add(new object[] { $"{expense.Id}", $"{expense.Date}", "Income", $"{expense.ToString()}", "", $"+ {expense.Value}", $"{balance }" });
                }
                else
                {
                    balance -= expense.Value;
                    dt.Rows.Add(new Object[] { $"{expense.Id}", $"{expense.Date}", "Charge", $"{expense.ToString()}", $"- {expense.Value} ", "", $"{balance}" });
                }

            }

            return dt;
        }

       

        public static double CalculatePercentage(List<Charge> charges, Category category)
        {
            return (charges.Where(x => x.Category == category).Select(x => x.Value).Sum()
                 / charges.Select(x => x.Value).Sum()) * 100;
        }
        public static void CloseAllWindows(object sender, EventArgs e)
        {
            foreach (Window window in Application.Current.Windows)
            {
                window.Close();
            }

        }
        public static void CloseAllWindows()
        {
            foreach (Window window in Application.Current.Windows)
            {
                window.Close();
            }
        }

    }
}