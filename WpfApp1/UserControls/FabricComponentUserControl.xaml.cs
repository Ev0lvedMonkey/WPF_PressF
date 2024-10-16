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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp1.Database;

namespace WpfApp1.UserControls
{
    /// <summary>
    /// Логика взаимодействия для FabricComponentUserControl.xaml
    /// </summary>
    public partial class FabricComponentUserControl : UserControl
    {
        private MComponents _component;
        public FabricComponentUserControl(MComponents component)
        {
            _component = component;
            InitializeComponent();
            OnNullCheck();
            UserRoleCheck();
            UpdateComponentsText();
        }

        private void UserRoleCheck()
        {
            if (App.CurrentUserRole == App.Roles.Директор.ToString()
                || App.CurrentUserRole == App.Roles.Менеджер.ToString())
            {
                DelComponentBtn.Visibility = Visibility.Visible;
                CompCountTB.IsReadOnly = false;
                CompTB.IsReadOnly = false;
                CompMassTB.IsReadOnly = false;
                CompPriceTB.IsReadOnly = false;
            }
        }


        private void OnNullCheck()
        {
            if (_component == null)
                return;
        }

        private void UpdateComponentsText()
        {
            ArticlText.Text = _component.Article;
            CompTB.Text = _component.Name;
            CompCountTB.Text = _component.Quantity.ToString();
            CompPriceTB.Text = _component.Price.ToString();
            CompTypeTB.Text = App.DB.UnitType.FirstOrDefault(x => x.ID == _component.UnitTypeID).Name;
            WarehousesTB.Text = App.DB.Warehouses.FirstOrDefault(x => x.WarehouseID == _component.WarehouseID).WarehouseName;
            CompMassTB.Text = _component.Weight.ToString();
        }


        private void TB_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, 0))
                e.Handled = true;
        }

        private void CompTB_LostFocus(object sender, RoutedEventArgs e)
        {
            _component.Name = CompTB.Text;
            App.DB.SaveChanges();

        }

        private void CompCountTB_LostFocus(object sender, RoutedEventArgs e)
        {
            _component.Quantity = int.Parse(CompCountTB.Text);
            App.DB.SaveChanges();
        }

        private void CompMassTB_LostFocus(object sender, RoutedEventArgs e)
        {
            _component.Weight = decimal.Parse(CompMassTB.Text);
            App.DB.SaveChanges();
        }

        private void CompPriceTB_LostFocus(object sender, RoutedEventArgs e)
        {
            _component.Price = decimal.Parse(CompPriceTB.Text);
            App.DB.SaveChanges();
        }

        private void DelComponentBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result =
                MessageBox.Show("Вы уверены, что хотите удалить запись?", "Подтверждение удаления",
                MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                App.DB.MComponents.Remove(_component);
                App.DB.SaveChanges();

                MessageBox.Show("Запись успешно удалена.", "Удаление", MessageBoxButton.OK);
                NavigateTo(App.GetRightPage());
            }
        }

        private void NavigateTo(object content)
        {
            Window window = Window.GetWindow(this);

            if (window == null)
                return;
            Frame mainFrame = LogicalTreeHelper.FindLogicalNode(window, "MainFrame") as Frame;
            mainFrame?.Navigate(content);
        }
    }
}
