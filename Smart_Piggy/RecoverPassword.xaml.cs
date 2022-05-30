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
    /// Interaction logic for RecoverPassword.xaml
    /// </summary>
    public partial class RecoverPassword : Window
    {
        BudgetDatabaseContext BudgetDB;
        Owner FoundOwner;

        public RecoverPassword(BudgetDatabaseContext BudgetDB)
        {
            InitializeComponent();
            this.BudgetDB = BudgetDB;
        }


        private void RetrievePass_Click(object sender, RoutedEventArgs e)
        {
            bool nameFilled = Manager.ValidateTextBox(fname) && Manager.ValidateTextBox(lname);

            if (Manager.ValidateTextBox(userName) == true && nameFilled == true)
            {
                MessageBox.Show("Please fill at least one of these options:\nUsername or First and Last name");
            }
            else
            {
                switch (Manager.ValidateTextBox(userName))
                {
                    case false:
                        UserNameEntered(userName.Text);
                        break;
                    case true:
                        FirstLastNamesEntered(fname, lname);
                        break;
                }

            }

        }

        private void FirstLastNamesEntered(TextBox fname, TextBox lname)
        {
            if (Manager.ValidateTextBox(fname) == true && Manager.ValidateTextBox(lname) == true)
            {
                MessageBox.Show("Please fill both first and last names or use username instead");
            }
            else
            {
                var owner = Manager.FindUser(fname.Text, lname.Text, BudgetDB);
                OwnerFoundState(owner);
            }
        }




        private void UserNameEntered(string username)
        {
            var owner = Manager.FindUser(username, BudgetDB);
            OwnerFoundState(owner);
        }

        private void OwnerFoundState(Owner owner)
        {
            switch (owner)
            {
                case null:
                    MessageBox.Show("This user doesn't exist !\n" +
                    "Please try entering another first and last name instead or try another username ...");
                    break;
                default:
                    fname.Text = owner.FirstName;
                    lname.Text = owner.LastName;
                    userName.Text = owner.UserName;
                    securityQuest.Content = owner.SecQuestion1;
                    FoundOwner = owner;
                    break;
            }
        }
        private void anotherQuest_Click(object sender, RoutedEventArgs e)
        {
            switch (FoundOwner)
            {
                case null:
                    MessageBox.Show("Requested details are not provided !\nPlease provide a username or first & last names\nthen click retrieve password to proceed...");
                    break;
                default:
                    if (securityQuest.Content.Equals(FoundOwner.SecQuestion1))
                    {
                        securityQuest.Content = FoundOwner.SecQuestion2;
                    }
                    else
                    {
                        securityQuest.Content = FoundOwner.SecQuestion1;
                    }
                    break;

            }
        }

        private void submit_Click(object sender, RoutedEventArgs e)
        {
            if (FoundOwner != null)
            {
                string ans = answer.Text;
                string quest = (string)securityQuest.Content;
                switch (quest.Equals(FoundOwner.SecQuestion1))
                {
                    case true:
                        switch (ans.Equals(FoundOwner.Answ1))
                        {
                            case true:

                                MessageBox.Show($"Your password is {FoundOwner.Password}");
                                break;
                            case false:
                                MessageBox.Show("Your answer is not correct!\nPlease try another question");
                                break;

                        }
                        break;
                    case false:
                        switch (ans.Equals(FoundOwner.Answ2))
                        {
                            case true:
                                MessageBox.Show($"Your password is {FoundOwner.Password}");
                                break;
                            case false:
                                MessageBox.Show("Your answer is not correct!\nPlease try another question");
                                break;

                        }
                        break;

                }
            }
        }

        private void Done_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
