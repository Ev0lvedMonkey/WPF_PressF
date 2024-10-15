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
            InitializeComponent();
            _material = material;
            UpdateComponentsText();
        }

        private void UpdateComponentsText()
        {
            ArticlText.Text = _material.Article;
            MaterialTB.Text = _material.Name;
            MaterialCountTB.Text = _material.Quantity.ToString();
            MaterialPriceTB.Text = _material.Price.ToString();
            MaterialTypeTB.Text = App.DB.UnitType.FirstOrDefault(x => x.ID == _material.UnitTypeID).Name;
            WarehousesTB.Text = App.DB.Warehouses.FirstOrDefault(x => x.WarehouseID == _material.WarehouseID).WarehouseName;
            MaterialMassTB.Text = _material.Weight.ToString();
        }
    }
}
