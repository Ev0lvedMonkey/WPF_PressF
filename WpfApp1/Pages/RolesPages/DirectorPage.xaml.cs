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
using WpfApp1.Pages.Lists;

namespace WpfApp1.Pages.RolesPages
{
    /// <summary>
    /// Логика взаимодействия для DirectorPage.xaml
    /// </summary>
    public partial class DirectorPage : Page
    {
        public DirectorPage()
        {
            InitializeComponent();
            MainWindow.Instance.SetUserFullName();
        }

        private void ToEmployeeListBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EmployeesList());
        }

        private void ToMaterialsAndComponentsListBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MaterialComponentsList());
        }

        private void ToWarehousesPageBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new WarehousesPage());
        }

        private void ToOrderBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new OrdersPage());
        }
    }
}
