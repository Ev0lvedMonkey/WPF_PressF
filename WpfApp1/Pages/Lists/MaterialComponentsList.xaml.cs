using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Логика взаимодействия для MaterialComponentsList.xaml
    /// </summary>
    public partial class MaterialComponentsList : Page
    {
        List<FMaterial> _materialList;
        List<MComponents> _componentsList;
        public MaterialComponentsList()
        {
            InitializeComponent();
            InitStartComponentsState();
            UpdateList();
        }

        private void InitStartComponentsState()
        {
            SelectShowCB.SelectedIndex = 0;
            WarehousesCB.SelectedIndex = 0;
        }

        private void UpdateList()
        {
            MaterialComponentsWP.Children.Clear();
            _materialList = App.DB.FMaterial.ToList();
            _componentsList = App.DB.MComponents.ToList();

            UseFilters();

            switch (SelectShowCB.SelectedIndex)
            {
                case 0:
                    foreach (FMaterial material in _materialList)
                        MaterialComponentsWP.Children.Add(new FabricMaterialUserControl(material));
                    foreach (MComponents component in _componentsList)
                        MaterialComponentsWP.Children.Add(new FabricComponentUserControl(component));
                    break;
                case 1:
                    foreach (FMaterial material in _materialList)
                        MaterialComponentsWP.Children.Add(new FabricMaterialUserControl(material));
                    _componentsList.Clear();
                    break;
                case 2:
                    foreach (MComponents component in _componentsList)
                        MaterialComponentsWP.Children.Add(new FabricComponentUserControl(component));
                    _materialList.Clear();
                    break;
            }

            CalculateBottomText();
        }

        private void CalculateBottomText()
        {
            CompPosAmountText.Text = $"Всего компонентов: {_componentsList.Count}";
            MatPosAmoutText.Text = $"Всего материалов: {_materialList.Count}";
            TotalPosAmountText.Text = $"Всего позиций: {_componentsList.Count + _materialList.Count}";
            TotalPrice.Text = $"Общая стоимость: {_componentsList.Sum(p => p.Price * p.Quantity) + _materialList.Sum(p => p.Price * p.Quantity)}";
            Debug.WriteLine("Calculated");
        }

        private void UseFilters()
        {
            switch (WarehousesCB.SelectedIndex)
            {
                case 1:
                    _materialList = _materialList?.Where(x => x.WarehouseID == 1).ToList();
                    _componentsList = _componentsList?.Where(x => x.WarehouseID == 1).ToList();
                    break;
                case 2:
                    _materialList = _materialList?.Where(x => x.WarehouseID == 2).ToList();
                    _componentsList = _componentsList?.Where(x => x.WarehouseID == 2).ToList();
                    break;
                case 3:
                    _materialList = _materialList?.Where(x => x.WarehouseID == 3).ToList();
                    _componentsList = _componentsList?.Where(x => x.WarehouseID == 3).ToList();
                    break;
            }
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e) =>
            NavigationService.Navigate(App.GetRightPage());

        private void SelectShowCB_SelectionChanged(object sender, SelectionChangedEventArgs e) =>
            UpdateList();

        private void WarehousesCB_SelectionChanged(object sender, SelectionChangedEventArgs e) =>
            UpdateList();
    }
}
