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
using WpfApp1.UserControls;

namespace WpfApp1.Pages.Lists
{
    /// <summary>
    /// Логика взаимодействия для OrdersPage.xaml
    /// </summary>
    public partial class OrdersPage : Page
    {
        private List<Order> _order;

        public OrdersPage()
        {
            InitializeComponent();
            CheckRole();
            UpdateOrdersList();
        }

        private void CheckRole()
        {
            if (App.CurrentUserRole == App.Roles.Заказчик.ToString())
            {
                NewOrderBtn.Visibility = Visibility.Visible;
                _order = App.DB.Order.Where(x => x.CustomerID == App.CurrentUser.UserID).ToList();
            }
            else if (App.CurrentUserRole == App.Roles.Менеджер.ToString())
            {
                NewOrderBtn.Visibility = Visibility.Visible;
                _order = App.DB.Order.Where(x => x.ManagerID == App.CurrentUser.UserID || x.StatusID == 1).ToList();
            }
            else if (App.CurrentUserRole == App.Roles.Директор.ToString())
                _order = App.DB.Order.ToList();
            else if (App.CurrentUserRole == App.Roles.Конструктор.ToString())
                _order = App.DB.Order.Where(x => x.StatusID == 3).ToList();
            else if (App.CurrentUserRole == App.Roles.Мастер.ToString())
                _order = App.DB.Order.Where(x => x.StatusID == 6 || x.StatusID == 7).ToList();
        }

        private void UpdateOrdersList()
        {
            OrdersWP.Children?.Clear();
            foreach (Order order in _order)
                OrdersWP.Children.Add(new OrderUserControl(order));
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e) =>
            NavigationService.Navigate(App.GetRightPage());

        private void NewOrderBtn_Click(object sender, RoutedEventArgs e) =>
            NavigationService.Navigate(new NewOrderRegPage());
    }
}
