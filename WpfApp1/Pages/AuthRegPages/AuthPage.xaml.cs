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
using WpfApp1.Database;
using WpfApp1.Pages.AuthRegPages;
using WpfApp1.Pages.RolesPages;

namespace WpfApp1.Pages
{
    /// <summary>
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        public AuthPage()
        {
            InitializeComponent();
            MainWindow.Instance.HideComponets();
            LoginTB.Focus();
        }

        private void EnterBtn_Click(object sender, RoutedEventArgs e)
        {
            User enteredUser =
                App.DB.User.FirstOrDefault(x => x.Login == LoginTB.Text && x.Password == PasswordTB.Text);
            if (App.SetNewUser(enteredUser))
            {
                if (RememberMeChBtn.IsChecked == true)
                {
                    NavigationService.Navigate(App.GetRightPage());
                    App.SaveUserSettings(enteredUser);
                }
                else
                {
                    NavigationService.Navigate(App.GetRightPage());
                }
                MainWindow.Instance.OpenComponets();
            }
        }

        private void CustomerRegBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CustomerRegPage());
        }
    }
}
