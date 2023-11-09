using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(canvas);

            if (currentShapeType == ShapeType.Rectangle)
            {
                currentShape = new Rectangle
                {
                    Stroke = customFillColor,
                    StrokeThickness = 2,
                    Fill = customStrokeColor
                };
            }
            else if (currentShapeType == ShapeType.Triangle)
            {
                currentShape = new Polygon
                {
                    Stroke = customFillColor,
                    StrokeThickness = 2,
                    Fill = customStrokeColor
                };
                ((Polygon)currentShape).Points = new PointCollection
                {
                    new Point(startPoint.X, startPoint.Y),
                    new Point(startPoint.X + 20, startPoint.Y - 40),
                    new Point(startPoint.X + 40, startPoint.Y)
                };
            }
            else if (currentShapeType == ShapeType.Ellipse)
            {
                currentShape = new Ellipse
                {
                    Stroke = customFillColor,
                    StrokeThickness = 2,
                    Fill = customStrokeColor
                };
            }

            Canvas.SetLeft(currentShape, startPoint.X);
            Canvas.SetTop(currentShape, startPoint.Y);

            canvas.Children.Add(currentShape);
        }

        private void Canvas_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (currentShape != null && e.LeftButton == MouseButtonState.Pressed)
            {
                Point endPoint = e.GetPosition(canvas);

                double width = Math.Abs(endPoint.X - startPoint.X);
                double height = Math.Abs(endPoint.Y - startPoint.Y);

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
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            currentShape = null;
        }

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

        private void ChooseFillColor_Click(object sender, RoutedEventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                customFillColor = new SolidColorBrush(Color.FromArgb(colorDialog.Color.A, colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B));
                // 將選擇的填充顏色應用到當前形狀
                if (currentShape != null)
                {
                    ((Shape)currentShape).Fill = customFillColor;
                }
            }
            FillColorButton.Background = customFillColor;
        }

        private void ChooseStrokeColor_Click(object sender, RoutedEventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                customStrokeColor = new SolidColorBrush(Color.FromArgb(colorDialog.Color.A, colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B));
                // 將選擇的邊框顏色應用到當前形狀
                if (currentShape != null)
                {
                    ((Shape)currentShape).Stroke = customStrokeColor;
                }
            }
            StrokeColorButton.Background = customStrokeColor;
        }

        enum ShapeType
        {
            Rectangle,
            Triangle,
            Ellipse
        }

    }
}
