using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WpfApp1.PopUps;

namespace WpfApp1.Pages
{
    public partial class WarehousesPage : Page
    {
        private const string BaseImagePath = "/Resources/WarehousesResources/WarehousesLayouts/";
        private const string BaseDefImagePath = "/Resources/WarehousesResources/DefaultLayouts/";
        private const string SaveDirectory = "WarehouseData";
        private Image _draggedImage;
        private Point _dragStartPoint;
        private bool _isDragging;

        public WarehousesPage()
        {
            InitializeComponent();
            WarehousesCB.SelectedIndex = 0;
            Directory.CreateDirectory(SaveDirectory); // Создаем папку для сохранений
        }

        private void WarehousesCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ResetCanvasChildren();
            switch (WarehousesCB.SelectedIndex)
            {
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
                    break;
            }
        }

        private void SetNewImageSource(Image image, string path)
        {
            ResetImageSource(image);
            image.Source = GetImage(path);
        }

        private void ResetImageSource(Image image) => image.Source = null;

        private BitmapImage GetImage(string path) => new BitmapImage(new Uri(path, UriKind.Relative));

        private void Button_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Button button = sender as Button;
                Image image = (Image)button.Content;
                _draggedImage = new Image
                {
                    Source = image.Source,
                    Width = 40,
                    Height = 40,
                };

                _dragStartPoint = e.GetPosition(this);
                _isDragging = true;
                Canvas.SetLeft(_draggedImage, _dragStartPoint.X - _draggedImage.Width / 2);
                Canvas.SetTop(_draggedImage, _dragStartPoint.Y - _draggedImage.Height / 2);
                WarehousesCanvas.Children.Add(_draggedImage);
            }
        }

        private void WarehousesCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDragging && _draggedImage != null)
            {
                Point currentPosition = e.GetPosition(WarehousesCanvas);
                Canvas.SetLeft(_draggedImage, currentPosition.X - _draggedImage.Width / 2);
                Canvas.SetTop(_draggedImage, currentPosition.Y - _draggedImage.Height / 2);
                _dragStartPoint = currentPosition;
            }
        }

        private void WarehousesCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_isDragging && _draggedImage != null)
            {
                _isDragging = false;
                _draggedImage = null;
            }
        }

        private void WarehousesCanvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point clickPosition = e.GetPosition(WarehousesCanvas);
            UIElement element = WarehousesCanvas.InputHitTest(clickPosition) as UIElement;

            if (element is Image image)
            {
                WarehousesCanvas.Children.Remove(image);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveImagesForWarehouse();
            MessageBox.Show("Сохранение выполнено!");
        }

        private void ResetCanvasChildren()
        {
            if (WarehousesCanvas.Children.Count > 1)
            {
                List<UIElement> elementsToRemove = new List<UIElement>();
                for (int i = 1; i < WarehousesCanvas.Children.Count; i++)
                    elementsToRemove.Add(WarehousesCanvas.Children[i]);
                foreach (var element in elementsToRemove)
                    WarehousesCanvas.Children.Remove(element);
            }
        }

        private string GetFilePathForWarehouse(int warehouseIndex)
        {
            return Path.Combine(SaveDirectory, $"Warehouse_{warehouseIndex}.txt");
        }

        private void SaveImagesForWarehouse()
        {
            int warehouseIndex = WarehousesCB.SelectedIndex;
            string filePath = GetFilePathForWarehouse(warehouseIndex);

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var child in WarehousesCanvas.Children)
                {
                    if (child is Image image)
                    {
                        double left = Canvas.GetLeft(image);
                        double top = Canvas.GetTop(image);

                        string sourcePath = "";

                        if (image.Source is BitmapImage bitmapImage)
                        {
                            sourcePath = bitmapImage.UriSource.IsAbsoluteUri
                                ? bitmapImage.UriSource.AbsolutePath
                                : bitmapImage.UriSource.ToString();
                        }
                        else if (image.Source is BitmapSource bitmapSource && bitmapSource is BitmapFrame frame)
                        {
                            if (frame.Decoder.Frames.Count > 0)
                            {
                                sourcePath = frame.Decoder.Frames[0].ToString();
                            }
                        }

                        if (!string.IsNullOrEmpty(sourcePath))
                        {
                            writer.WriteLine($"{sourcePath}|{left}|{top}");
                        }
                    }
                }
            }
        }


        private void LoadImagesForWarehouse(int warehouseIndex)
        {
            string filePath = GetFilePathForWarehouse(warehouseIndex);

            if (!File.Exists(filePath))
                return;

            using (StreamReader reader = new StreamReader(filePath))
            {
                reader.ReadLine();

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split('|');
                    if (parts.Length == 3)
                    {
                        string imagePath = parts[0];
                        double left = double.Parse(parts[1]);
                        double top = double.Parse(parts[2]);

                        Image image = new Image
                        {
                            Source = new BitmapImage(new Uri(imagePath, UriKind.Absolute)),
                            Width = 40,
                            Height = 40,
                        };

                        Canvas.SetLeft(image, left);
                        Canvas.SetTop(image, top);
                        WarehousesCanvas.Children.Add(image);
                    }
                }
            }
        }

        private void ResetBtn_Click(object sender, RoutedEventArgs e)
        {
            ResetCanvasChildren();

            int warehouseIndex = WarehousesCB.SelectedIndex;
            if (warehouseIndex > 0)
            {
                string filePath = GetFilePathForWarehouse(warehouseIndex);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    MessageBox.Show("Все данные для выбранного склада удалены.");
                }
                else
                    MessageBox.Show("Файл для текущего склада не найден.");
            }
            else
                MessageBox.Show("Выберите склад для сброса данных.");
        }


        private void ExitBtn_Click(object sender, RoutedEventArgs e) => NavigationService.GoBack();

        private void ShowDeafaultWarehousesLayoutBtn_Click(object sender, RoutedEventArgs e)
        {
            switch (WarehousesCB.SelectedIndex)
            {
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
                    break;
            }
        }
        private void ShowImagePopup(string imagePath)
        {
            DefaultWarehousePopUp popupWindow = new DefaultWarehousePopUp(imagePath);
            popupWindow.ShowDialog();
        }
    }
}