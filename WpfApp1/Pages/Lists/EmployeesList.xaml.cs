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
using WpfApp1.UserControls;

namespace WpfApp1.Pages.Lists
{
    /// <summary>
    /// Логика взаимодействия для EmployeesList.xaml
    /// </summary>
    public partial class EmployeesList : Page
    {
        public EmployeesList(){
            InitializeComponent();
            UpdateEmployeesList();}

        private void UpdateEmployeesList(){
            EmployeesWP.Children?.Clear();
            List<Employees> employees =
                App.DB.Employees.OrderByDescending(x => x.ExperienceYears).ToList();
            foreach (Employees employee in employees)
                EmployeesWP.Children.Add(new EmployeeUserControl(employee));}

        private void ExitBtn_Click(object sender, RoutedEventArgs e) =>
            NavigationService.Navigate(App.GetRightPage());
    }
}
