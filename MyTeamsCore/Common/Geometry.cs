using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MyTeamsCore.Common;
public static class 
GeometryHelper {
    
    public static Vector
    ToVector(this Point point) => new Vector(point.X, point.Y);
    
    public static Point 
    Shift(this Point point, Point other) => point.Shift(other.ToVector());
    
    public static Point
    Shift(this Point point, double deltaX, double deltaY) => new Point(x: point.X + deltaX, y: point.Y + deltaY);
    
    public static double
    Cos(double angle) {
        var result = Math.Cos(Math.PI / 180 * angle);
        return Math.Abs(result) < 0.000000001 ? 0 : result;
    }
    
    public static double
    Sin(double angle) {
        var result = Math.Sin(Math.PI / 180 * angle);
        return Math.Abs(result) < 0.000000001 ? 0 : result;
    }
    
    public static Point
    Scale(this Point point, Size size) => new(point.X * size.Width, point.Y * size.Height);

    public static Point 
    ShiftX(this Point point, double deltaX) => new(point.X + deltaX, point.Y);
    
    public static Point 
    Shift(this Point point, Vector delta) => new(point.X + delta.X, point.Y + delta.Y);
    
    public static Point
    ShiftPolar(this Point startPoint, double angle, double length) =>
        startPoint.Shift(angle.GetVector(length));
    
    public static Point 
    Divide(this Point point, double value) => new Point(point.X/value, point.Y/value);
    
    public static Point 
    Divide(this Point point, Size size) => new Point(point.X/size.Width, point.Y/size.Height);

    public static Vector
    GetVector(this double angle, double length) =>
        new(Cos(angle) * length, Sin(angle) * length);
    
    public static Line
    Shift(this Line line, Vector delta) => new(line.Start.Shift(delta), line.End.Shift(delta));
    
    public static Vector 
    GetUnitVector(this Line line) {
        var length = line.Length;
        return new Vector((line.End.X - line.Start.X) / length, (line.End.Y - line.Start.Y) / length);
    }
    
    public static Vector 
    GetPerpendicularVector(this Line line) {
        var unitVector = line.GetUnitVector();
        return new Vector(- unitVector.Y, unitVector.X);
    }
        
    public static Vector 
    Scale(this Vector vector, double size) =>
        new(vector.X * size, vector.Y * size);
    
    public static Rectangle
    RelativeTo(this Rectangle rectangle, Point point) => 
        new(rectangle.Left - point.X, rectangle.Top - point.Y, rectangle.Width, rectangle.Height);

    public static Point 
    RelativeTo(this Point point, Point other) => new(point.X - other.X, point.Y - other.Y);

    public static Point 
    ReplaceNegativeWithZeros(this Point point) => new(point.X < 0 ? 0 : point.X, point.Y < 0 ? 0 : point.Y);
    
    public static Point 
    RelativeWithin(this Point point, Size size) => new(point.X / size.Width, point.Y / size.Height);
    
    public static Point 
    RelativeWithin(this Point point, Rectangle rectangle) => 
        point.RelativeTo(rectangle.TopLeftCorner).RelativeWithin(rectangle.Size);
    
    public static Rectangle
    ShiftX(this Rectangle rectangle, double deltaX) =>
        new(rectangle.Left + deltaX, rectangle.Top, rectangle.Width, rectangle.Height);
    
    public static Rectangle
    ShiftY(this Rectangle rectangle, double deltaY) =>
        new(rectangle.Left, rectangle.Top + deltaY, rectangle.Width, rectangle.Height);

    public static Rectangle 
    Shift(this Rectangle rectangle, double deltaX, double deltaY) => 
        rectangle.Shift(new Point(deltaX, deltaY));
        
    public static Rectangle
    Shift(this Rectangle rectangle, Point delta) => 
        new Rectangle(left: rectangle.Left + delta.X, top: rectangle.Top + delta.Y, width: rectangle.Width, height: rectangle.Height);
    
    public static RectangleTriangle
    ShiftRectangleX(this RectangleTriangle rectangleTriangle, double deltaX) {
        var newRectangle = rectangleTriangle.Rectangle.ShiftX(deltaX);
        var cornerRadius= rectangleTriangle.CornerRadius;
        var minTriangleX = newRectangle.Left + cornerRadius + rectangleTriangle.TriangleEdge / 2;
        var maxTriangleX = newRectangle.Right - cornerRadius - rectangleTriangle.TriangleEdge / 2;
        return new RectangleTriangle(
            rectangle: newRectangle,
            triangleLeft: Math.Min(Math.Max(minTriangleX, rectangleTriangle.TriangleLeft), maxTriangleX),
            triangleEdge: rectangleTriangle.TriangleEdge,
            cornerRadius: rectangleTriangle.CornerRadius,
            triangleVerticalAlignment: rectangleTriangle.TriangleVerticalAlignment);
    } 
    
    public static Line
    GetParallelLine(this Line line, double distance) =>
        line.Shift(line.GetPerpendicularVector().Scale(distance));

    public static Line
    GetLine(this Point startPoint, double angle, double width) =>
        new(startPoint, startPoint.ShiftPolar(angle, width));
    
    public static string
    GetEllipsePath(double width, double height, double angle) {
        var startPoint = new Point(0, 0);
        var endPoint = startPoint.ShiftPolar(angle, width);
        var diagonal = new Line(startPoint, endPoint);
        var parallelLine1 = diagonal.GetParallelLine(height);
        var parallelLine2 = diagonal.GetParallelLine(-height);
        var css = $"M {startPoint.ToCssString()} C {parallelLine1.Start.ToCssString()}, {parallelLine1.End.ToCssString()}, {endPoint.ToCssString()} C {parallelLine2.End.ToCssString()}, {parallelLine2.Start.ToCssString()}, {startPoint.ToCssString()}";
        return css;
    }
    
    public static RelativePoint 
    ToRelativePoint(this Point point, Size size) => 
        new RelativePoint(point.X / size.Width, point.Y / size.Height);
    
    public static RelativePoint 
    ToRelativePoint(this Point point) => 
        new RelativePoint(point);

    public static Thickness 
    Multiply(this Thickness thickness, double value) => new Thickness(left: thickness.Left * value, top: thickness.Top * value, right: thickness.Right * value, bottom: thickness.Bottom * value);
    
    public static Size 
    Scale(this Size size, double scale) => new Size(size.Width * scale, size.Height * scale);
 
    public static double 
    GetDistanceTo(this Point point, Point other) => Math.Sqrt(Math.Pow(point.X - other.X, 2) + Math.Pow(point.Y - other.Y, 2));
    
    public static IntPoint
    ToIntPoint(this Point point) => new ((int)point.X, (int)point.Y);
    
    public static Rectangle
    ToRectangle(this Point point, double width, double height) => new Rectangle(point.X, point.Y, width, height);
    
    public static Thickness 
    ReplaceLeft(this Thickness thickness, double newValue) => new Thickness(left: newValue, top: thickness.Top, right: thickness.Right, bottom: thickness.Bottom);
    
    public static Thickness 
    ReplaceRight(this Thickness thickness, double newValue) => new Thickness(left: thickness.Left, top: thickness.Top, right: newValue, bottom: thickness.Bottom);
    
    public static Thickness 
    ReplaceTop(this Thickness thickness, double newValue) => new Thickness(left: thickness.Left, top: newValue, right: thickness.Right, bottom: thickness.Bottom);
    
    public static Thickness 
    ReplaceBottom(this Thickness thickness, double newValue) => new Thickness(left: thickness.Left, top: thickness.Top, right: thickness.Right, bottom: newValue);
       
    public static Rectangle 
    Resize(this Rectangle rectangle, Size newSize) => new (left: rectangle.Left, top: rectangle.Top, width: newSize.Width, height: newSize.Height);
    
    public static Rectangle
    ReplaceLeft(this Rectangle rectangle, double newLeft) =>
        new Rectangle(
            left: newLeft, 
            top: rectangle.Top,
            width: rectangle.Width,
            height: rectangle.Height);

    public static Rectangle
    ReplaceTop(this Rectangle rectangle, double newTop) =>
        new Rectangle(
            left: rectangle.Left,
            top: newTop,
            width: rectangle.Width,
            height: rectangle.Height);
            
    public static IntRectangle
    RelativeTo(this IntRectangle rectangle, IntPoint point) => 
        new IntRectangle(left: rectangle.Left - point.X, top: rectangle.Top - point.Y, width: rectangle.Width, height: rectangle.Height);
}