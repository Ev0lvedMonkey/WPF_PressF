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
using WpfApp1.Pages.Lists;

namespace WpfApp1.Pages
{
    /// <summary>
    /// Логика взаимодействия для QualityChecksOrderPage.xaml
    /// </summary>
    public partial class QualityChecksOrderPage : Page
    {
        private Order _order;
        public QualityChecksOrderPage(Order order)
        {
            _order = order;
            InitializeComponent();
            ParamListView.DataContext = App.DB.QualityParameters.ToList();
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(App.GetRightPage());
        }

        private void SaveExitBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in ParamListView.Items)
            {
                if (ParamListView.ItemContainerGenerator.ContainerFromItem(item) is ListViewItem container)
                {
                    var commentTextBox = FindVisualChild<TextBox>(container, "CommentTB");
                    var isCheckCheckBox = FindVisualChild<CheckBox>(container, "IsCheckParam");

                    if (commentTextBox != null && isCheckCheckBox != null)
                    {
                        QualityChecks qualityCheck = new QualityChecks
                        {
                            OrderID = _order.OrderID,
                            ParameterID = ParamListView.Items.IndexOf(item) + 1,
                            IsCheck = isCheckCheckBox.IsChecked ?? false,
                            Comment = commentTextBox.Text,
                            CheckDate = DateTime.Now,
                        };
                        App.DB.QualityChecks.Add(qualityCheck);
                    }
                }
            }
            SaveQualityCheck();
        }

        private T FindVisualChild<T>(DependencyObject depObj, string name) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    var child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T && ((FrameworkElement)child).Name == name)
                    {
                        return (T)child;
                    }

                    var childItem = FindVisualChild<T>(child, name);
                    if (childItem != null)
                    {
                        return childItem;
                    }
                }
            }
            return null;
        }

        private void SaveQualityCheck()
        {
            MessageBoxResult result =
                MessageBox.Show("Вы уверены, что хотите сохранить запись?", "Подтверждение удаления",
                MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                App.DB.SaveChanges();
                NavigationService.Navigate(new OrdersPage());
            }
        }
    }
}
