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
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        // Predicate<TextBox> IsEmpty = Manager.ValidateTextBox;

        BudgetDatabaseContext BudgetDB;
        public Login()
        {
            InitializeComponent();
            BudgetDB = new BudgetDatabaseContext();
           
        }
        public Login(string newUser, BudgetDatabaseContext BudgetDB)
        {
            InitializeComponent();
            this.BudgetDB = BudgetDB;
            user.Text = newUser;
        }

        public Login(BudgetDatabaseContext BudgetDB)
        {
            InitializeComponent();
            this.BudgetDB = BudgetDB;

        }

        private void submit_Click(object sender, RoutedEventArgs e)
        {

            if (Manager.ValidateTextBox(user) == true || Manager.ValidatePasswordBox(password) == true)
            {
                errorMessage.Content = "Please fill both username and password fields !";
                MessageBox.Show(errorMessage.Content.ToString());
            }
            else
            {
                Owner owner = Manager.FindUser(user.Text, BudgetDB);

                switch (owner)
                {
                    case null:
                        MessageBox.Show("User not found ! Please try again");
                        break;
                    default:
                        bool isCorrect = Manager.CheckPassword(owner, password.Password.ToString());
                        LoginUser(isCorrect, owner);
                        break;
                }
            }
        }


        private void LoginUser(bool isCorrect, Owner owner)
        {
            if (isCorrect is true)
            {
                MainWindow mainWindow = new MainWindow(owner, BudgetDB);
                this.Close();
                mainWindow.Show();
            }
            else
            {
                MessageBox.Show("Password is incorrect\nPlease try again !");
            }

        }


        private void CreateNew_Click(object sender, RoutedEventArgs e)
        {
            Register newRegister = new Register(BudgetDB);
            this.Close();
            newRegister.Show();
        }

        private void recoverPass_Click(object sender, RoutedEventArgs e)
        {
            RecoverPassword recoverScreen = new RecoverPassword(BudgetDB);
            
            recoverScreen.Show();
        }
    }
}
