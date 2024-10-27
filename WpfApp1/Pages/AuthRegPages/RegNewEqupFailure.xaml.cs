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
    /// Логика взаимодействия для RegNewEqupFailure.xaml
    /// </summary>
    public partial class RegNewEqupFailure : Page
    {
        public RegNewEqupFailure()
        {
            InitializeComponent();
            EqupCB.ItemsSource = App.DB.Equipments.ToList();
            EqupCB.DataContext = "EquipName";
            ReasonCB.ItemsSource = App.DB.FailureReasons.ToList();
            ReasonCB.DataContext = "ReasonName";
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
            {
                EquipmentFailures equipmentFailures = new EquipmentFailures
                {
                    EquipmentID = (EqupCB.SelectedItem as Equipments).EquipmentID,
                    FailureReasonID = (EqupCB.SelectedItem as FailureReasons).FailureReasonID,
                    FailureDate = (DateTime)FailureDateDP.SelectedDate,
                    MasterID = App.CurrentUser.UserID,
                    FixDate = null
                };
                App.DB.EquipmentFailures.Add(equipmentFailures);
                App.DB.SaveChanges();
                NavigationService.Navigate(new RegNewEqupFailure());
            }
        }

        private bool CheckTextFields(StringBuilder stringBuilder)
        {
            if (ReasonCB.SelectedItem == null)
                stringBuilder.AppendLine("Выберите причину поломки");
            if (EqupCB.SelectedItem == null)
                stringBuilder.AppendLine("Выберите оборудование");
            if (FailureDateDP.SelectedDate == null)
                stringBuilder.AppendLine("Выберите время поломки");
            if (stringBuilder.Length > 0)
                return true;
            else return false;
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegNewEqupFailure());
        }
    }
}
