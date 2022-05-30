using System;
using System.Data;
using System.Linq;

using System.Windows;
using Budget_Tracker_Library;
using Microsoft.VisualBasic;
using Smart_Piggy_Database;

namespace Smart_Piggy
{
    /// <summary>
    /// Interaction logic for EditUsers.xaml
    /// </summary>
    public partial class EditUsers : Window
    {
        BudgetDatabaseContext BudgetDB;
        Owner Owner;
        public EditUsers(BudgetDatabaseContext BudgetDB, Owner owner)
        {
            InitializeComponent();
            this.BudgetDB = BudgetDB;   
            this.Owner = owner;
            PopulateData();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string selection = displayAll.SelectedItem.ToString();
                if (selection != null)
                {
                    var owner = BudgetDB.Owners.Where(x => x.UserName == selection).FirstOrDefault();
                    int id = Convert.ToInt32(owner.Id);
                   switch(CheckPassword(owner))
                    {
                        case true:
                            SetUserForm(id, owner);
                            break;
                        case false:
                            MessageBox.Show("Password is empty or incorrect !");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
               Console.WriteLine(ex.Message);
            }
        }

        private void SetUserForm(int id,Owner owner)
        {
            Register userForm = new Register(BudgetDB);
            userForm.EditId = id;
            userForm.DataChanged += RefreshData;

            userForm.fname.Text = owner.FirstName;
            userForm.lname.Text = owner.LastName;
            userForm.userName.Text = owner.UserName;    
            userForm.quest1.Text = owner.SecQuestion1;
            userForm.quest2.Text = owner.SecQuestion2;
            userForm.answ1.Text = owner.Answ1;
            userForm.answ2.Text = owner.Answ2;
            userForm.createAccount.Content = "Submit Changes";
            userForm.Title = "Edit existing user";
            userForm.title.Content = "Edit Existing user";
            
            userForm.Show();
        }


        private void PopulateData()
        {

            var owners = BudgetDB.Owners.Select(x => x.UserName).ToList();

            displayAll.ItemsSource = owners;



        }

        private void RefreshData(object sender , EventArgs e)
        {
            PopulateData();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string selection = displayAll.SelectedItem.ToString();
                if (selection != null)
                {
                    
                  if(  MessageBox.Show("Are you sure you want to delete this user ?\nThis means losing all budget data related to this user" , "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {

                        var owner = BudgetDB.Owners.Where(x => x.UserName == selection).FirstOrDefault();
                        switch (CheckPassword(owner) )
                        {
                            case true:
                            BudgetDB.Owners.Remove(owner);
                            BudgetDB.SaveChanges();
                            MessageBox.Show("User removed successfully!\nPlease Restart the program !");
                            Manager.CloseAllWindows();
                                break;
                            case false:
                                MessageBox.Show("Password is empty or incorrect !");
                                break;
                        }     
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void Done_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private bool CheckPassword(Owner owner)
        {
            string password =
            Interaction.InputBox($"Please enter password for {owner.UserName} to proceed","Enter password","",200,200);
            if (password.Equals(owner.Password))
            {
                return true;
            }
            return false;
        }

       
    }
}
