using System.Collections.Generic;
using System.Drawing;

namespace WindowsFormsApp2
{
    /// <summary>
    /// 线段/圆弧混合图形
    /// </summary>
    public class CompoundShape
    {
        public List<IGeometrySegment> Segments = new List<IGeometrySegment>();
    }

    public interface IGeometrySegment { }

    public class LineSegment : IGeometrySegment
    {
        public PointF Start, End;
        public LineSegment(PointF s, PointF e)
        {
            Start = s; End = e;
        }
    }

    public class ArcSegment : IGeometrySegment
    {
        public PointF Start, End, Center;
        public ArcSegment(PointF s, PointF e, PointF c)
        {
            Start = s; End = e; Center = c;
        }
    }
}
