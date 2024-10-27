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

namespace WpfApp1.Pages.Lists
{
    /// <summary>
    /// Логика взаимодействия для EquipmentFailuresList.xaml
    /// </summary>
    public partial class EquipmentFailuresList : Page
    {
        public EquipmentFailuresList()
        {
            InitializeComponent();
            EquipFailuresList.DataContext = App.DB.EquipmentFailures.Where(x => x.MasterID == App.CurrentUser.UserID).ToList();
            DataContext = App.DB.EquipmentFailures.Where(x => x.MasterID == App.CurrentUser.UserID).ToList(); 
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(App.GetRightPage());
        }

        private void FixDateDP_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is DatePicker fixDateDP)
            {
                if (EquipFailuresList.SelectedItem is EquipmentFailures selectedFailure)
                {
                    selectedFailure.FixDate = fixDateDP.SelectedDate;
                    App.DB.SaveChanges();
                }
                else
                {
                    MessageBox.Show($"{EquipFailuresList.SelectedItem}");
                }
            }
            else
            {
                MessageBox.Show("Sender is not a DatePicker.");
            }
        }

        private void RegNewEqupFailureBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegNewEqupFailure());
        }
    }
}
