using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using WpfApp1.Database;
using WpfApp1.Pages.RolesPages;

namespace WpfApp1.Pages.AuthRegPages
{
    /// <summary>
    /// Логика взаимодействия для CustomerRegPage.xaml
    /// </summary>
    public partial class CustomerRegPage : Page
    {
        private const string Pattern1 = @"[*&{}\|+,]";
        private const string Pattern2 = @"[A-ZА-Я]";
        private const string Pattern3 = @"\d";

        public CustomerRegPage()
        {
            InitializeComponent();
        }

        private void EnterBtn_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder stringBuilder = new StringBuilder();

            if (CheckTextFields(stringBuilder))
            {
                MessageBox.Show(stringBuilder.ToString());
                stringBuilder.Clear();
            }
            else
            {
                CretaeNewCustomer();
                NavigationService.Navigate(new CustomerPage());
                MainWindow.Instance.SetUserFullName();
                MainWindow.Instance.OpenComponets();
            }
        }

        private void CretaeNewCustomer()
        {
            User newCustomerUser = new User
            {
                FirstName = FirstNameTB.Text,
                LastName = LastNameTB.Text,
                Patronymic = PatronymicTB.Text,
                Login = LoginTB.Text,
                Password = PasswordTB.Text,
                RoleID = 5,

            };
            App.DB.User.Add(newCustomerUser);
            App.SetNewUser(newCustomerUser);
            App.DB.SaveChanges();
        }

        private bool CheckTextFields(StringBuilder stringBuilder)
        {
            if (string.IsNullOrEmpty(FirstNameTB.Text))
                stringBuilder.AppendLine("Введите имя");
            if (string.IsNullOrEmpty(LastNameTB.Text))
                stringBuilder.AppendLine("Введите отчество");
            if (string.IsNullOrEmpty(PatronymicTB.Text))
                stringBuilder.AppendLine("Введите отчество");
            if (string.IsNullOrEmpty(LoginTB.Text))
                stringBuilder.AppendLine("Введите логин");
            if (string.IsNullOrEmpty(PasswordTB.Text))
                stringBuilder.AppendLine("Введите пароль");
            if (PasswordTB.Text.Length > 16 || PasswordTB.Text.Length < 4)
                stringBuilder.AppendLine("Пароль должен содержать от 4 до 16 знаков");
            if (Regex.IsMatch(PasswordTB.Text, Pattern1))
                stringBuilder.AppendLine("Пароль не должен содержать знаки: * & { } | + ,");
            if (Regex.IsMatch(PasswordTB.Text, Pattern2))
                stringBuilder.AppendLine("Пароль не должен содержать заглвные буквы");
            if (!Regex.IsMatch(PasswordTB.Text, Pattern3))
                stringBuilder.AppendLine("Пароль должен содержать хотя бы одну цифру");

            if (stringBuilder.Length > 0)
                return true;
            else return false;
        }
    }
}
