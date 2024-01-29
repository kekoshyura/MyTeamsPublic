using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MyTeamsCore.Common;

namespace MyTeamsCore;

public readonly struct 
Point: IEquatable<Point> {
    public double X {get;}
    public double Y {get;}
    [JsonConstructor]
    public Point(double x, double y) {
        X = x;
        Y = y;
    }
 
    public static Point Zero = new Point(0, 0);
    public override string ToString() => $"{X}, {Y}";
    public string ToCssString() => $"{X} {Y}";

    public bool Equals(Point other) => X.IsEqual(other.X) && Y.IsEqual(other.Y);
    public override bool Equals(object? obj) => obj is Point other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(X, Y);
}

public readonly struct 
IntPoint {
    public int X {get;}
    public int Y {get;}
    [JsonConstructor]
    public IntPoint(int x, int y) {
        X = x;
        Y = y;
    }
    public override string ToString() => $"{X}, {Y}";
    public string ToCssString() => $"{X} {Y}";
}

public readonly struct 
Vector {
    public double X {get;}
    public double Y {get;}
    [JsonConstructor]
    public Vector(double x, double y) {
        X = x;
        Y = y;
    }
    public override string ToString() => $"{X}, {Y}";
}

public readonly struct 
Size: IEquatable<Size> {
    public double Width {get;}
    public double Height {get;}
    [JsonConstructor]
    public Size(double width, double height) {
        Width = width;
        Height = height;
    }

    public override string ToString() => $"{Width}x{Height}";

    public bool Equals(Size other) => Width.IsEqual(other.Width) && Height.IsEqual(other.Height);
    public override bool Equals(object? obj) => obj is Size other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(Width, Height);
}

public readonly struct 
Line {
    public Point Start {get;}
    public Point End {get;}
    [JsonConstructor]
    public Line(Point start, Point end) {
        Start = start;
        End = end;
    }
    
    public double Length => Math.Sqrt((End.X - Start.X).Squared() + (End.Y - Start.Y).Squared());
}

public readonly struct
Rectangle: IEquatable<Rectangle> {
    public double Left { get; }
    public double Top { get; }
    public double Width { get; }
    public double Height { get; }
    [JsonConstructor]
    public Rectangle(double left, double top, double width, double height) {
        Left = left;
        Top = top;
        Width = width.VerifyArgumentNotNegative(nameof(width));
        Height = height.VerifyArgumentNotNegative(nameof(height));
    }
    public Point TopLeftCorner => new (Left, Top);
    public Point TopRightCorner => new (Right, Top);
    public Point BottomRightCorner => new (Right, Bottom);
    public Point BottomLeftCorner => new (Left, Bottom);
    public Point Center => new (Left + Width/ 2, Top + Height / 2);
    public double Right => Left + Width;
    public double Bottom => Top + Height;
    public Size Size => new (Width, Height);
    public bool IsEmpty => Width.IsZero() || Height.IsZero();
    public double TotalArea => Height * Width;
    public bool IsHorizontal => Width.IsGreater(Height);
    public bool IsVertical => Height.IsGreater(Width);
    
    public bool 
    Intersects(Rectangle other) => 
        other.Left.IsLess(Left + Width) && 
        Left.IsLess(other.Left + other.Width) &&
        other.Top.IsLess(Top + Height) &&
        Top.IsLess(other.Top + other.Height);
    
    public bool 
    Touches(Rectangle other) =>
        other.Right.IsEqual(Left) || 
        other.Left.IsEqual(Right) ||
        other.Top.IsEqual(Bottom) ||
        other.Bottom.IsEqual(Top);
    
    public Rectangle
    GetIntersection(Rectangle other) => Intersect(this, other);
    
    public static Rectangle 
    Intersect(Rectangle a, Rectangle b) {
        var left = Math.Max(a.Left, b.Left);
        var right = Math.Min(a.Right, b.Right);
        var top = Math.Max(a.Top, b.Top);
        var bottom = Math.Min(a.Bottom, b.Bottom);
 
        if (right >= left && bottom >= top)
            return new Rectangle(left, top, right - left, bottom - top);
        return new Rectangle(0,0,0,0);
    }
    
    public IEnumerable<(RectangleCorner corner, Point position)>
    GetCornerPositions() {
        yield return (RectangleCorner.TopLeft, position: TopLeftCorner);
        yield return (RectangleCorner.TopRight, position: TopRightCorner);
        yield return (RectangleCorner.BottomLeft, position: BottomLeftCorner);
        yield return (RectangleCorner.BottomRight, position: BottomRightCorner);
        
    }

    public override string ToString() => $"x={Left.Rounded()}, y={Top.Rounded()}, width={Width.Rounded()}, height={Height.Rounded()}";

    public bool Equals(Rectangle other) => Left.IsEqual(other.Left) && Top.IsEqual(other.Top) && Width.IsEqual(other.Width) && Height.IsEqual(other.Height);
    public override bool Equals(object? obj) => obj is Rectangle other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(Left, Top, Width, Height);
}

public struct 
IntRectangle {
    public int Left {get;}
    public int Top {get;}
    public int Width {get;}
    public int Height {get;}
    public IntRectangle(int left, int top, int width, int height) {
        Left = left;
        Top = top;
        Width = width;
        Height = height;
    }

    public int Bottom => Top + Height;
    public int Right => Left + Width;

    public bool 
    ContainsPoint(IntPoint point) =>
        Left <= point.X && point.X <= Right && Top <= point.Y && point.Y <= Bottom;

    public IntRectangle
    GetIntersection(IntRectangle other) => Intersect(this, other);
    
    public bool Intersects(IntRectangle other) =>  Intersect(this, other).Width != 0;
    
    public static IntRectangle 
    Intersect(IntRectangle a, IntRectangle b) {
        var left = Math.Max(a.Left, b.Left);
        var right = Math.Min(a.Right, b.Right);
        var top = Math.Max(a.Top, b.Top);
        var bottom = Math.Min(a.Bottom, b.Bottom);
 
        if (right >= left && bottom >= top)
            return new IntRectangle(left, top, right - left, bottom - top);
        return new IntRectangle(0,0,0,0);
    }

    public IntPoint TopLeftCorner => new IntPoint(Left, Top);
}

public enum 
RectangleCorner {
    TopLeft,
    TopRight,
    BottomRight,
    BottomLeft
}

public struct 
Indicator {
    public Point Position {get;}
    public string Text {get;}
    public Indicator(Point position, string text) {
        Position = position;
        Text = text;
    }
}

/// <summary>
/// Represents a rectangle with an arrow like in a tool tip
/// </summary>
public class
RectangleTriangle {
    public Rectangle Rectangle { get; }
    //Absolute position X of the center of a triangle
    public double TriangleLeft { get;}
    public double TriangleEdge { get; }
    public double CornerRadius { get;}
    public RectangleTriangle(Rectangle rectangle, double triangleLeft, double triangleEdge, double cornerRadius, VerticalAlignment triangleVerticalAlignment) {
        Rectangle = rectangle;
        TriangleLeft = triangleLeft;
        TriangleEdge = triangleEdge;
        CornerRadius = cornerRadius;
        TriangleVerticalAlignment = triangleVerticalAlignment;
    }
    public VerticalAlignment TriangleVerticalAlignment { get; }
    public double TriangleHeight => TriangleEdge * Math.Sqrt(3) / 2;
}


public readonly struct RelativePoint {
    
    public Point Value {get; }

    public RelativePoint(double x, double y) => Value = new Point(x,y);
    [JsonConstructor]
    public RelativePoint(Point value) => Value = value;

    public bool IsZero => Value.X.IsZero() && Value.Y.IsZero();
    
    public RelativePoint 
    Shift(RelativePoint other) => 
        new RelativePoint(Value.Shift(other.Value));
        
    public Point 
    ToAbsolute(Size size) => new Point(size.Width * Value.X, size.Height * Value.Y);

    public override string ToString() => Value.ToString();
}

public readonly struct 
Border {
    public string Color {get;}
    public Thickness Thickness {get;}
    public Border(string color, Thickness thickness) {
        Color = color;
        Thickness = thickness;
    }
    
    public static Border None = new Border("white", Thickness.Zero);
}


public readonly struct
Thickness {
    public double Left { get; }
    public double Top { get; }
    public double Right { get; }
    public double Bottom { get; }

    [JsonConstructor]
    public Thickness(double left, double top, double right, double bottom) {
        Left = left;
        Top = top;
        Right = right;
        Bottom = bottom;
    }

    public static Thickness Zero = new Thickness(0,0,0,0);
    public static Thickness One = new Thickness(1,1,1,1);
    public static Thickness FromLeftTop(double left, double top) => new Thickness(left, top, right: 0, bottom: 0);
    public static Thickness FromSingleValue(double value) => new Thickness(value, value, value, value);
    public bool IsZero => Left.IsZero() && Top.IsZero() && Right.IsZero() && Bottom.IsZero();
    public bool IsUniform => Left.IsEqual(Top) && Top.IsEqual(Right) && Right.IsEqual(Bottom);
    public Thickness IncrementBottom(double delta) => new Thickness(Left, Top, Right, Bottom + delta);
}

public record Alignment(HorizontalAlignment Horizontal, VerticalAlignment Vertical) {
    public static Alignment LeftTop = new (Horizontal: HorizontalAlignment.Left, Vertical: VerticalAlignment.Top);
    public static Alignment LeftCenter = new (Horizontal: HorizontalAlignment.Left, Vertical: VerticalAlignment.Center);
    public static Alignment Center = new(Horizontal: HorizontalAlignment.Center, Vertical: VerticalAlignment.Center);
    public static Alignment Stretch = new (Horizontal: HorizontalAlignment.Stretch, Vertical: VerticalAlignment.Stretch);
    
}

public enum
HorizontalAlignment {
    Left,
    Right,
    Center,
    Stretch
}

public enum
VerticalAlignment {
    Top,
    Bottom,
    Center,
    Stretch
}

public enum 
Orientation {
    Horizontal,
    Vertical
}
