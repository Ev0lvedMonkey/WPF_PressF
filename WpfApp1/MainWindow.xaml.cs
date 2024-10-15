using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using WpfApp1.Pages;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow Instance { get; private set; }
        public MainWindow()
        {
            InitializeComponent();
            if (Instance == null)
                Instance = this;
            else
                return;

            if (App.CurrentUser != null)
            {
                MainFrame.NavigationService.Navigate(App.GetRightPage());
                return;
            }

            MainFrame.NavigationService.Navigate(new AuthPage());
        }

        public void OpenComponets()
        {
            ExitBtn.Visibility = Visibility.Visible;
            UserFullNameText.Visibility = Visibility.Visible;
        }

        public void HideComponets()
        {
            ExitBtn.Visibility = Visibility.Hidden;
            UserFullNameText.Visibility = Visibility.Hidden;
        }

        public void SetUserFullName()
        {
            string userRole = App.DB.Role.FirstOrDefault(x => x.RoleID == App.CurrentUser.RoleID).RoleName;
            string userFullName =
                $"{userRole} {App.CurrentUser.LastName} {App.CurrentUser.FirstName} {App.CurrentUser.Patronymic}";;
            UserFullNameText.Text = userFullName;
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new AuthPage());
            App.ClearUserSettings();
        }
    }
}
