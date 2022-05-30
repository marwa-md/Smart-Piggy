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
using System.Data.Entity;
using Budget_Tracker_Library;
using Smart_Piggy_Database;

namespace Smart_Piggy
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        BudgetDatabaseContext BudgetDB;
        public int EditId = 0;

        public delegate void DataChangedEventHandler(object sender, EventArgs e);

        public event DataChangedEventHandler DataChanged;
        public Register(BudgetDatabaseContext BudgetDB)
        {
            InitializeComponent();
            this.BudgetDB = BudgetDB;

        }

        private void CreateAccount_Click(object sender, RoutedEventArgs e)
        {
            if (Manager.CheckAllEmptyFields(main).Contains(false))

            {
                MessageBox.Show("Please fill all fields !");
            }
            else
            {
                EditAddUser();
            };
        }

        private void ClearAll_Click(object sender, RoutedEventArgs e)
        {
            main.ClearAllControls();
        }

        private bool MatchPasswords(string pass, string confirmPass)
        {
            return pass.Equals(confirmPass);
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            if (EditId == 0)
            {
                Login login = new Login(BudgetDB);
                this.Close();
                login.Show();
            }
            else
            {
                this.Close();   
            }
            
        }
        private void EditAddUser()
        {
            Owner owner = Manager.FindUser(userName.Text, BudgetDB);
            if (EditId == 0 && owner == null)
            {
                RegisterUser();

            }
            else if (EditId == 0 && owner != null)
            {
                MessageBox.Show("This username already exists !\nPlease choose another username and try again...");
            }

            else
            {
                Owner currentOwner = BudgetDB.Owners.FirstOrDefault(x => x.Id == EditId);
                if (currentOwner.Equals(owner))
                {
                    UpdateUser();
                }
                else
                {
                    MessageBox.Show("This username already exists !\nPlease choose another username and try again...");
                }
            }
        }
        private void RegisterUser()
        {
            switch (MatchPasswords(pass.Password, confirmPass.Password))
                    {
                        case false:
                            MessageBox.Show("Passwords have to match !");
                            break;
                        case true:
                                BudgetDB.Owners.Add(new Owner()
                                {
                                    FirstName = fname.Text,
                                    LastName = lname.Text,
                                    UserName = userName.Text,
                                    Password = pass.Password.ToString(),
                                    SecQuestion1 = quest1.Text,
                                    SecQuestion2 = quest2.Text,
                                    Answ1 = answ1.Text,
                                    Answ2 = answ2.Text
                                });
                                BudgetDB.SaveChanges();
                                MessageBox.Show("User created successfully\nPlease login now !");
                                Login login = new Login(userName.Text, BudgetDB);
                                this.Close();
                            login.Show();    
                            break;
                    }
         }

        private void UpdateUser()
        {
            switch (MatchPasswords(pass.Password, confirmPass.Password))
            {
                case false:
                    MessageBox.Show("Passwords have to match !");
                    break;
                case true:
                    var query =
                    from owner in BudgetDB.Owners
                    where owner.Id == EditId
                    select owner;

                    foreach (Owner owner in query)
                    {
                        owner.FirstName = fname.Text;
                        owner.LastName = lname.Text;
                        owner.UserName = userName.Text;
                        owner.SecQuestion1 = quest1.Text;
                        owner.SecQuestion2 = quest2.Text;
                        owner.Answ1 = answ1.Text;
                        owner.Answ2 = answ2.Text;
                        owner.Password = pass.Password.ToString();
                    }
                    BudgetDB.SaveChanges();

                    MessageBox.Show("User updated Successfully!");
                    SaveChangedData();
                    this.Close();
                    break;
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
    }
}
