using System.Collections.Generic;
using System.Drawing;

namespace WindowsFormsApp2
{

    public enum InputMode
    {
        Polygon,
        Arc
    }

    public class GraphicItem
    {
        public InputMode Mode;
        public List<PointF> PolyPoints;
        public ArcRecord Arc;
    }

    public class ArcRecord
    {
        public PointF Start, End, Center;
        public int Stage; 
    }

    public static class GeometryHelper
    {
        public static float Distance(PointF a, PointF b)
        {
            float dx = a.X - b.X, dy = a.Y - b.Y;
            return (float)System.Math.Sqrt(dx * dx + dy * dy);
        }

        public static PointF MidPoint(PointF a, PointF b)
        {
            return new PointF((a.X + b.X) / 2, (a.Y + b.Y) / 2);
        }
    }
}
