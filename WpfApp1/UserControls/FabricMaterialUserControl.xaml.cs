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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp1.Database;

namespace WpfApp1.UserControls
{
    /// <summary>
    /// Логика взаимодействия для FabricMaterialUserControl.xaml
    /// </summary>
    public partial class FabricMaterialUserControl : UserControl
    {
        private FMaterial _material;
        public FabricMaterialUserControl(FMaterial material)
        {
            _material = material;
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
                DelMaterialBtn.Visibility = Visibility.Visible;
                MaterialCountTB.IsReadOnly = false;
                MaterialTB.IsReadOnly = false;
                MaterialMassTB.IsReadOnly = false;
                MaterialPriceTB.IsReadOnly = false;
            }
        }
        private void OnNullCheck()
        {
            if (_material == null)
                return;
        }
        private void UpdateComponentsText()
        {
            ArticlText.Text = _material.Article;
            MaterialTB.Text = _material.Name;
            MaterialCountTB.Text = _material.Quantity.ToString();
            MaterialPriceTB.Text = _material.Price.ToString();
            MaterialTypeTB.Text = App.DB.MaterialsType.FirstOrDefault(x => x.ID == _material.MaterialTypeID).Name;
            WarehousesTB.Text = App.DB.Warehouses.FirstOrDefault(x => x.WarehouseID == _material.WarehouseID).WarehouseName;
            MaterialMassTB.Text = _material.Weight.ToString();
        }

        private void TB_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, 0))
                e.Handled = true;
        }

        private void MaterialTB_LostFocus(object sender, RoutedEventArgs e)
        {
            _material.Name = MaterialTB.Text;
            App.DB.SaveChanges();
        }

        private void MaterialCountTB_LostFocus(object sender, RoutedEventArgs e)
        {
            _material.Quantity = int.Parse(MaterialMassTB.Text);
            App.DB.SaveChanges();

        }

        private void MaterialMassTB_LostFocus(object sender, RoutedEventArgs e)
        {
            _material.Weight = decimal.Parse(MaterialMassTB.Text);
            App.DB.SaveChanges();
        }

        private void MaterialGHOSTTB_LostFocus(object sender, RoutedEventArgs e)
        {
            _material.GOST = MaterialGHOSTTB.Text;
            App.DB.SaveChanges();

        }

        private void MaterialPriceTB_LostFocus(object sender, RoutedEventArgs e)
        {
            _material.Price = int.Parse(MaterialPriceTB.Text);
            App.DB.SaveChanges();
        }

        private void DelMaterialBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result =
                MessageBox.Show("Вы уверены, что хотите удалить запись?", "Подтверждение удаления",
                MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                App.DB.FMaterial.Remove(_material);
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
