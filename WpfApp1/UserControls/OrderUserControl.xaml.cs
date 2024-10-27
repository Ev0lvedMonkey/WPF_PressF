using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp1.Database;
using WpfApp1.Pages;
using WpfApp1.Pages.Lists;

namespace WpfApp1.UserControls
{
    /// <summary>
    /// Логика взаимодействия для OrderUserControl.xaml
    /// </summary>
    public partial class OrderUserControl : UserControl
    {
        private Order _order;
        public OrderUserControl(Order order)
        {
            _order = order;
            InitializeComponent();
            UserRoleCheck();
            UpdateOrderText();
        }

        private void UserRoleCheck()
        {
            if (App.CurrentUserRole == App.Roles.Заказчик.ToString())
            {
                if (_order.StatusID < 5 && _order.StatusID != 2)
                    DelOrderBtn.Visibility = Visibility.Visible;
                else
                    DelOrderBtn.Visibility = Visibility.Collapsed;
            }
            if (App.CurrentUserRole == App.Roles.Менеджер.ToString())
            {
                if (_order.StatusID == 1)
                {
                    GetOrderBtn.Visibility = Visibility.Visible;
                    DelOrderBtn.Visibility = Visibility.Visible;
                }
                if (_order.StatusID == 5)
                {
                    SendToProdBtn.Visibility = Visibility.Visible;
                }
                if (_order.StatusID == 8)
                {
                    CloseOrderBtn.Visibility = Visibility.Visible;
                }
            }
            if (App.CurrentUserRole == App.Roles.Конструктор.ToString())
            {
                if (_order.StatusID == 3)
                    ConfirmOrderBtn.Visibility = Visibility.Visible;
            }
            if (App.CurrentUserRole == App.Roles.Мастер.ToString())
            {
                if (_order.StatusID == 6)
                    SendToControlBtn.Visibility = Visibility.Visible;
                if (_order.StatusID == 7)
                    ConfirmOrderReadinessBtn.Visibility = Visibility.Visible;
                if (!App.DB.QualityChecks.Any(x => x.OrderID == _order.OrderID) && _order.StatusID == 7)
                    OrderQualityChecksBtn.Visibility = Visibility.Visible;
            }

        }

        private void UpdateOrderText()
        {
            User customer = App.DB.User.FirstOrDefault(x => x.UserID == _order.CustomerID);
            User manager = App.DB.User.FirstOrDefault(x => x.UserID == _order.ManagerID);
            OrderStatus status = App.DB.OrderStatus.FirstOrDefault(x => x.StatusID == _order.StatusID);
            NumbOrderText.Text = $"Заказ №{_order.OrderID} от";
            StatusText.Text = $"Статус: {status.StatusName}";
            DateOrderTB.Text = _order.OrderDate.ToString();
            OrderNameTB.Text = _order.OrderName.ToString();
            OrderPriceTB.Text = _order.Cost.ToString();
            PreDateTB.Text = _order.PlannedCompletionDate.ToString();
            CustomerText.Text = $"Заказчик: {customer.LastName} {customer.FirstName} {customer.Patronymic}";
            ManagerText.Text = $"Ответсвенный менеджер: {manager?.LastName} {manager?.FirstName} {manager?.Patronymic}";
        }

        private void ChangeOrderStatus(int statusNumber)
        {
            _order.StatusID = statusNumber;
            CreateHistoryRecord(statusNumber);
            App.DB.SaveChanges();
            MainWindow.Instance.Navigate(new OrdersPage());
        }

        private void CreateHistoryRecord(int statusNumber)
        {
            OrderStatusHistory orderStatusHistory = new OrderStatusHistory
            {
                OrderID = _order.OrderID,
                StatusID = statusNumber,
                ChangeDate = DateTime.Now,
            };
            App.DB.OrderStatusHistory.Add(orderStatusHistory);
        }

        private void DeductionOfComponentMaterials()
        {
            OrderSpecifications orderSpecifications = App.DB.OrderSpecifications.FirstOrDefault(x => x.OrderID == _order.OrderID);
            int? usingMaterialID = orderSpecifications.MaterialID;
            int? usingComponentID = orderSpecifications.ComponentID;
            int? usingComponentQuantity = orderSpecifications.Quantity;

            FMaterial usingMaterial = App.DB.FMaterial.FirstOrDefault(x => x.MaterialID == usingMaterialID);
            MComponents usingComponent = App.DB.MComponents.FirstOrDefault(x => x.ComponentID == usingComponentID);


            if ((usingMaterial.Quantity -= 1) >= 0)
            {
                usingMaterial.Quantity -= 1;
                App.DB.SaveChanges();
            }
            else
            {
                MessageBox.Show($"На складе\n" +
                    $"Материла {usingMaterial.Name} - {usingMaterial.Quantity} {usingMaterial.UnitType.Name}");
                return;
            }
            if ((usingComponent.Quantity -= usingComponentQuantity) >= 0)
            {
                usingComponent.Quantity -= usingComponentQuantity;
                App.DB.SaveChanges();
            }
            else
            {
                MessageBox.Show($"На складе\n" +
                    $"Компонентов {usingComponent.Name} - {usingComponent.Quantity} {usingComponent.UnitType.Name}");
                return;
            }
        }

        private void GetOrderBtn_Click(object sender, RoutedEventArgs e)
        {
            _order.ManagerID = App.CurrentUser.UserID;
            ChangeOrderStatus(3);
        }

        private void DelOrderBtn_Click(object sender, RoutedEventArgs e) =>
            ChangeOrderStatus(2);

        private void SendToProdBtn_Click(object sender, RoutedEventArgs e)
        {
            DeductionOfComponentMaterials();
            ChangeOrderStatus(6);
        }


        private void CloseOrderBtn_Click(object sender, RoutedEventArgs e) =>
            ChangeOrderStatus(9);

        private void ConfirmOrderBtn_Click(object sender, RoutedEventArgs e) =>
            ChangeOrderStatus(4);

        private void SendToControlBtn_Click(object sender, RoutedEventArgs e) =>
            ChangeOrderStatus(7);

        private void ConfirmOrderReadinessBtn_Click(object sender, RoutedEventArgs e) =>
            ChangeOrderStatus(8);

        private void OrderQualityChecksBtn_Click(object sender, RoutedEventArgs e) =>
        MainWindow.Instance.Navigate(new QualityChecksOrderPage(_order));

        
    }
}
