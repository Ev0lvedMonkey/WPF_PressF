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

namespace WpfApp1.Pages.AuthRegPages
{
    /// <summary>
    /// Логика взаимодействия для NewOrderRegPage.xaml
    /// </summary>
    public partial class NewOrderRegPage : Page
    {
        public NewOrderRegPage()
        {
            InitializeComponent();
            RoleCheck();
            UpdateComboBoxsSource();
        }

        private void UpdateComboBoxsSource()
        {
            SelectManagerCB.ItemsSource = App.DB.User.Where(x => x.RoleID == 3).ToList();
            SelectCustomerCB.ItemsSource = App.DB.User.Where(x => x.RoleID == 5).ToList();
            SelectManagerCB.DisplayMemberPath = "LastName";
            SelectCustomerCB.DisplayMemberPath = "LastName";
        }

        private void RoleCheck()
        {
            if (App.CurrentUserRole == App.Roles.Менеджер.ToString())
                SelectCustomerSP.Visibility = Visibility.Visible;
            if (App.CurrentUserRole == App.Roles.Заказчик.ToString())
                SelectManagerSP.Visibility = Visibility.Visible;
        }

        private void TB_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, 0))
                e.Handled = true;
        }

        private void EnterBtn_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder stringBuilder = new StringBuilder();

            if (CheckTextFields(stringBuilder))
            {
                MessageBox.Show(stringBuilder.ToString());
                stringBuilder.Clear();
                return;
            }
            else
                CreateNewOrder();
            NavigationService.GoBack();
        }

        private void CreateNewOrder()
        {
            if (App.CurrentUserRole == App.Roles.Менеджер.ToString())
            {
                Order newOrder = new Order
                {
                    ManagerID = App.CurrentUser.UserID,
                    CustomerID = (SelectCustomerCB.SelectedItem as User).UserID,
                    StatusID = 3,
                    OrderName = OrderNameTB.Text,
                    PlannedCompletionDate = PreDateDP.SelectedDate,
                    OrderDate = DateTime.Now,
                    Cost = int.Parse(OrderPriceTB.Text),
                };
                App.DB.Order.Add(newOrder);
                MessageBox.Show($"Добавлен заказ менеджером {newOrder.OrderName}");
            }
            if (App.CurrentUserRole == App.Roles.Заказчик.ToString())
            {
                Order newOrder = new Order
                {
                    ManagerID = (SelectManagerCB.SelectedItem as User).UserID,
                    CustomerID = App.CurrentUser.UserID,
                    StatusID = 3,
                    OrderName = OrderNameTB.Text,
                    PlannedCompletionDate = PreDateDP.SelectedDate,
                    OrderDate = DateTime.Now,
                    Cost = int.Parse(OrderPriceTB.Text),
                };
                App.DB.Order.Add(newOrder);
                MessageBox.Show($"Добавлен заказ заказчиком {newOrder.OrderName}");

            }
            App.DB.SaveChanges();
        }

        private bool CheckTextFields(StringBuilder stringBuilder)
        {
            if (App.CurrentUserRole == App.Roles.Менеджер.ToString())
            {
                if (SelectCustomerCB.SelectedItem == null)
                    stringBuilder.AppendLine("Выберите заказчика");
                if (string.IsNullOrEmpty(OrderNameTB.Text))
                    stringBuilder.AppendLine("Введите название заказа");
                if (string.IsNullOrEmpty(PreDateDP.Text))
                    stringBuilder.AppendLine("Введите предварительную дату");
                if (string.IsNullOrEmpty(OrderPriceTB.Text))
                    stringBuilder.AppendLine("Введите стоимость заказа");

                if (stringBuilder.Length > 0)
                    return true;
                else return false;
            }
            if (App.CurrentUserRole == App.Roles.Заказчик.ToString())
            {
                if (SelectManagerCB.SelectedItem == null)
                    stringBuilder.AppendLine("Выберите менеджера");
                if (string.IsNullOrEmpty(OrderNameTB.Text))
                    stringBuilder.AppendLine("Введите название заказа");
                if (string.IsNullOrEmpty(PreDateDP.Text))
                    stringBuilder.AppendLine("Введите предварительную дату");
                if (string.IsNullOrEmpty(OrderPriceTB.Text))
                    stringBuilder.AppendLine("Введите стоимость заказа");

                if (stringBuilder.Length > 0)
                    return true;
                else return false;
            }
            else return true;
        }

        private void CustomerRegBtn_Click(object sender, RoutedEventArgs e) =>
            NavigationService.Navigate(new CustomerRegPage(false));

        private void ExitBtn_Click(object sender, RoutedEventArgs e) =>
                    NavigationService.Navigate(App.GetRightPage());
    }
}
