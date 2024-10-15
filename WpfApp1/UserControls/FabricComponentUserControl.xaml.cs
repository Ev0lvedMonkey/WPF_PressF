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

namespace WpfApp1.UserControls
{
    /// <summary>
    /// Логика взаимодействия для FabricComponentUserControl.xaml
    /// </summary>
    public partial class FabricComponentUserControl : UserControl
    {
        private MComponents _component;
        private bool _canUseBtn;
        private Action OnTextFieldsChanged;
        public FabricComponentUserControl(MComponents component)
        {
            InitializeComponent();
            OnNullCheck();
            UserRoleCheck();

            OnTextFieldsChanged += () => { SaveComponentBtn.Visibility = Visibility.Visible; };
            if (_canUseBtn)
                DelComponentBtn.Visibility = Visibility.Visible;
            UpdateComponentsText();
        }

        private void UserRoleCheck()
        {
            if (App.CurrentUserRole == App.Roles.Director.ToString()
                || App.CurrentUserRole == App.Roles.Manager.ToString())
            {
                _canUseBtn = true;
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
            CompTypeTB.Text = App.DB.ComponentsType.FirstOrDefault(x => x.ID == _component.ComponentTypeID).Name;
            WarehousesTB.Text = App.DB.Warehouses.FirstOrDefault(x => x.WarehouseID == _component.WarehouseID).WarehouseName;
            CompMassTB.Text = _component.Weight.ToString();
        }

        private void SaveComponentBtn_Click(object sender, RoutedEventArgs e)
        {
            SaveComponentBtn.Visibility = Visibility.Hidden;
        }

        private void MaterialTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            OnTextFieldsChanged.Invoke();
        }

        private void CompCountTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            OnTextFieldsChanged.Invoke();
        }

        private void CompPriceTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            OnTextFieldsChanged.Invoke();
        }

        private void CompMassTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            OnTextFieldsChanged.Invoke();
        }
    }
}
