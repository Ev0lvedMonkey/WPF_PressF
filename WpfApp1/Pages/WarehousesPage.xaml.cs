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
using WpfApp1.PopUps;

namespace WpfApp1.Pages
{
    /// <summary>
    /// Логика взаимодействия для WarehousesPage.xaml
    /// </summary>
    public partial class WarehousesPage : Page
    {
        private const string BaseImagePath = "/Resources/WarehousesResources/WarehousesLayouts/";
        private const string BaseDefImagePath = "/Resources/WarehousesResources/DefaultLayouts/";
        private Image _draggedImage;
        private Point _dragStartPoint;
        private bool _isDragging;
        private Dictionary<int, List<Tuple<ImageSource, double, double>>> _warehouseImages =
            new Dictionary<int, List<Tuple<ImageSource, double, double>>>();

        public WarehousesPage(){
            InitializeComponent();
            WarehousesCB.SelectedIndex = 0;}

        private void WarehousesCB_SelectionChanged(object sender, SelectionChangedEventArgs e){
            switch (WarehousesCB.SelectedIndex){
                case 1:
                    SetNewImageSource(WarehousesImage, BaseImagePath + "FirstWH.png");
                    LoadImagesForWarehouse(1);
                    break;
                case 2:
                    SetNewImageSource(WarehousesImage, BaseImagePath + "SecondWH.png");
                    LoadImagesForWarehouse(2);
                    break;
                case 3:
                    SetNewImageSource(WarehousesImage, BaseImagePath + "ThirdWh.png");
                    LoadImagesForWarehouse(3);
                    break;
                default:
                    ResetImageSource(WarehousesImage);
                    LoadImagesForWarehouse(0);
                    break;}}

        private void SetNewImageSource(Image image, string path){
            ResetCanvasChildren();
            ResetImageSource(image);
            image.Source = GetImage(path);}

        private void ResetImageSource(Image image) => image.Source = null;

        private BitmapImage GetImage(string path){
            return new BitmapImage(new Uri(path, UriKind.Relative));}

        private void Button_PreviewMouseMove(object sender, MouseEventArgs e){
            if (e.LeftButton == MouseButtonState.Pressed){
                Button button = sender as Button;
                Image image = (Image)button.Content;
                _draggedImage = new Image{
                    Source = image.Source,
                    Width = 40,
                    Height = 40,};

                _dragStartPoint = e.GetPosition(this);
                _isDragging = true;
                Canvas.SetLeft(_draggedImage, _dragStartPoint.X - _draggedImage.Width / 2);
                Canvas.SetTop(_draggedImage, _dragStartPoint.Y - _draggedImage.Height / 2);
                WarehousesCanvas.Children.Add(_draggedImage);}}

        private void WarehousesCanvas_MouseMove(object sender, MouseEventArgs e){
            if (_isDragging && _draggedImage != null){
                Point currentPosition = e.GetPosition(WarehousesCanvas);
                double deltaX = currentPosition.X - _dragStartPoint.X;
                double deltaY = currentPosition.Y - _dragStartPoint.Y;

                double left = Canvas.GetLeft(_draggedImage);
                double top = Canvas.GetTop(_draggedImage);

                Canvas.SetLeft(_draggedImage, left + deltaX);
                Canvas.SetTop(_draggedImage, top + deltaY);

                _dragStartPoint = currentPosition;}}

        private void WarehousesCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e){
            if (_isDragging && _draggedImage != null){
                _isDragging = false;
                SaveImagePosition(_draggedImage);
                _draggedImage = null;}}

        private void WarehousesCanvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e){
            Point clickPosition = e.GetPosition(WarehousesCanvas);
            UIElement element = WarehousesCanvas.InputHitTest(clickPosition) as UIElement;

            if (element is Image){
                WarehousesCanvas.Children.Remove(element);
                RemoveImagePosition((Image)element);}}

        private void SaveButton_Click(object sender, RoutedEventArgs e){
            MessageBox.Show("Сохранение выполнено!");
            SaveImagesForWarehouse();}

        private void ResetCanvasChildren()        {
            if (WarehousesCanvas.Children.Count > 1){
                List<UIElement> elementsToRemove = new List<UIElement>();
                for (int i = 1; i < WarehousesCanvas.Children.Count; i++)
                    elementsToRemove.Add(WarehousesCanvas.Children[i]);
                foreach (var element in elementsToRemove)
                    WarehousesCanvas.Children.Remove(element);}
            _warehouseImages.Clear();}

        private void ResetButton_Click(object sender, RoutedEventArgs e) =>ResetCanvasChildren();

        private void SaveImagePosition(Image image){
            int warehouseIndex = WarehousesCB.SelectedIndex;
            if (!_warehouseImages.ContainsKey(warehouseIndex)){
                _warehouseImages[warehouseIndex] = new List<Tuple<ImageSource, double, double>>();}

            double left = Canvas.GetLeft(image);
            double top = Canvas.GetTop(image);
            _warehouseImages[warehouseIndex].Add(new Tuple<ImageSource, double, double>(image.Source, left, top));}

        private void RemoveImagePosition(Image image){
            int warehouseIndex = WarehousesCB.SelectedIndex;
            if (_warehouseImages.ContainsKey(warehouseIndex)){
                var imagePosition = _warehouseImages[warehouseIndex].FirstOrDefault(i => i.Item1 == image.Source);
                if (imagePosition != null)
                    _warehouseImages[warehouseIndex].Remove(imagePosition);}}

        private void SaveImagesForWarehouse(){
            int warehouseIndex = WarehousesCB.SelectedIndex;
            if (!_warehouseImages.ContainsKey(warehouseIndex))
                _warehouseImages[warehouseIndex] = new List<Tuple<ImageSource, double, double>>();
            _warehouseImages[warehouseIndex].Clear();
            foreach (var child in WarehousesCanvas.Children){
                if (child is Image image){
                    double left = Canvas.GetLeft(image);
                    double top = Canvas.GetTop(image);
                    _warehouseImages[warehouseIndex].Add(new Tuple<ImageSource, double, double>(image.Source, left, top));}}}

        private void LoadImagesForWarehouse(int warehouseIndex){
            if (_warehouseImages.ContainsKey(warehouseIndex)){
                foreach (var imagePosition in _warehouseImages[warehouseIndex]){
                    Image image = new Image{
                        Source = imagePosition.Item1,
                        Width = 40,
                        Height = 40,};
                    Canvas.SetLeft(image, imagePosition.Item2);
                    Canvas.SetTop(image, imagePosition.Item3);
                    WarehousesCanvas.Children.Add(image);}}}


        private void ExitBtn_Click(object sender, RoutedEventArgs e) =>NavigationService.GoBack();

        private void ShowDeafaultWarehousesLayoutBtn_Click(object sender, RoutedEventArgs e){
            switch (WarehousesCB.SelectedIndex){
                case 1:
                    ShowImagePopup(BaseDefImagePath + "FisrtD_LO.png");
                    break;
                case 2:
                    ShowImagePopup(BaseDefImagePath + "SecondD_LO.png");
                    break;
                case 3:
                    ShowImagePopup(BaseDefImagePath + "ThirdD_LO.png");
                    break;
                default:
                    MessageBox.Show("Выберите цех");
                    break;}}

        private void ShowImagePopup(string imagePath){
            DefaultWarehousePopUp popupWindow = new DefaultWarehousePopUp(imagePath);
            popupWindow.ShowDialog();}
    }
}