using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using WPF_Assessment.Enums;
using WPF_Assessment.Services;

namespace WPF_Assessment
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        private UIElement currentShape;
        private Point startPoint;
        private ShapeType currentShapeType = ShapeType.Rectangle;
        private SolidColorBrush customFillColor = new SolidColorBrush(Colors.Blue); // 自訂填充顏色
        private SolidColorBrush customStrokeColor = new SolidColorBrush(Colors.Red); // 自訂邊框顏色
        private Line myLine = new Line(); // 自訂邊框顏色
        private ShapeCreatorBase shapeCreator;

        /// <summary>
        /// 建構子
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            this.shapeCreator = new ShapeCreatorBase();
        }

        /// <summary>
        /// 抓取建立圖案的相關資料
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(canvas);
            myLine.StrokeThickness = thicknessSlider.Value;
            currentShape = shapeCreator.CreateShapes(currentShapeType, startPoint, myLine.StrokeThickness, customFillColor, customStrokeColor);
            Canvas.SetLeft(currentShape, startPoint.X);
            Canvas.SetTop(currentShape, startPoint.Y);
            canvas.Children.Add(currentShape);
        }

        /// <summary>
        /// 更新當下圖形的大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Canvas_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (currentShape != null && e.LeftButton == MouseButtonState.Pressed)
            {
                Point endPoint = e.GetPosition(canvas);

                double width = Math.Abs(endPoint.X - startPoint.X);
                double height = Math.Abs(endPoint.Y - startPoint.Y);

                UpdateShapeSize(width, height);
            }
        }

        /// <summary>
        /// 創建當下圖形的大小
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        private void UpdateShapeSize(double width, double height)
        {
            if (currentShapeType == ShapeType.Rectangle)
            {
                double sideLength = Math.Max(width, height);
                ((Rectangle)currentShape).Width = sideLength;
                ((Rectangle)currentShape).Height = sideLength;
            }
            else if (currentShapeType == ShapeType.Triangle)
            {
                ((Polygon)currentShape).Points[1] = new Point(startPoint.X + width / 2, startPoint.Y - height);
                ((Polygon)currentShape).Points[2] = new Point(startPoint.X + width, startPoint.Y);
            }
            else if (currentShapeType == ShapeType.Ellipse)
            {
                ((Ellipse)currentShape).Width = width;
                ((Ellipse)currentShape).Height = height;
            }
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            currentShape = null;
        }

        /// <summary>
        /// 介面上面，選擇形狀的按鈕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToggleShape_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button clickedButton = (System.Windows.Controls.Button)sender;
            string shapeType = clickedButton.Content.ToString();

            if (Enum.TryParse(shapeType, out ShapeType type))
            {
                currentShapeType = type;
            }
            // 更改按钮的背景颜色
            rectangleButton.Background = Brushes.Black;
            triangleButton.Background = Brushes.Black;
            ellipseButton.Background = Brushes.Black;
            FillColorButton.Background = customFillColor;
            StrokeColorButton.Background = customStrokeColor;
            clickedButton.Background = Brushes.LightGray; // 或者选择其他颜色
        }
        /// <summary>
        /// 選擇填充內部顏色的按鈕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChooseFillColor_Click(object sender, RoutedEventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                customFillColor = new SolidColorBrush(Color.FromArgb(colorDialog.Color.A, colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B));
                if (currentShape != null)
                {
                    ((Shape)currentShape).Fill = customFillColor;
                }
            }
            FillColorButton.Background = customFillColor;
        }
        /// <summary>
        /// 選擇填充線條顏色的按鈕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChooseStrokeColor_Click(object sender, RoutedEventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                customStrokeColor = new SolidColorBrush(Color.FromArgb(colorDialog.Color.A, colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B));
                if (currentShape != null)
                {
                    ((Shape)currentShape).Stroke = customStrokeColor;
                }
            }
            StrokeColorButton.Background = customStrokeColor;
        }
        /// <summary>
        /// 更新線條粗細的按鈕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ThicknessSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            myLine.StrokeThickness = thicknessSlider.Value;
        }
    }
}
