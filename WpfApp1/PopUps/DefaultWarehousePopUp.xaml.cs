using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace WpfApp1.PopUps
{
    /// <summary>
    /// Логика взаимодействия для DefaultWarehousePopUp.xaml
    /// </summary>
    public partial class DefaultWarehousePopUp : Window
    {

        public DefaultWarehousePopUp(string imagePath)
        {
            InitializeComponent();
            PopupImage.Source = new BitmapImage(new Uri(imagePath, UriKind.Relative));
        }
    }
}
