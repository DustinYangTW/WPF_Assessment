using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using WPF_Assessment.Enums;

namespace WPF_Assessment.Services
{
    internal class ShapeCreatorBase
    {
        /// <summary>
        /// 共用方法，產出相關圖案
        /// </summary>
        /// <param name="shapeType">自訂義旗標</param>
        /// <param name="startPoint">起始位置</param>
        /// <param name="strokeThickness">線條粗細</param>
        /// <param name="customFillColor">線條顏色</param>
        /// <param name="customStrokeColor">圖案顏色</param>
        /// <returns></returns>
        public Shape CreateShapes(ShapeType shapeType, Point startPoint, double strokeThickness, Brush customFillColor, Brush customStrokeColor)
        {
            Shape shape = null;

            switch (shapeType)
            {
                case ShapeType.Rectangle:
                    shape = new Rectangle
                    {
                        Stroke = customFillColor,
                        StrokeThickness = strokeThickness,
                        Fill = customStrokeColor
                    };
                    break;

                case ShapeType.Triangle:
                    shape = new Polygon
                    {
                        Stroke = customFillColor,
                        StrokeThickness = strokeThickness,
                        Fill = customStrokeColor
                    };
                    ((Polygon)shape).Points = new PointCollection
                    {
                        new Point(startPoint.X, startPoint.Y),
                        new Point(startPoint.X + 20, startPoint.Y - 40),
                        new Point(startPoint.X + 40, startPoint.Y)
                    };
                    break;

                case ShapeType.Ellipse:
                    shape = new Ellipse
                    {
                        Stroke = customFillColor,
                        StrokeThickness = strokeThickness,
                        Fill = customStrokeColor
                    };
                    break;
            }
            return shape;
        }
    }
}