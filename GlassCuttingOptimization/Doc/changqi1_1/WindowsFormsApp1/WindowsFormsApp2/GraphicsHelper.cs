using System.Drawing;

namespace WindowsFormsApp2
{
    public static class GraphicsHelper
    {
        /// <summary>
        /// 画线段并显示长度
        /// </summary>
        public static void DrawLineSegment(Graphics g, LineSegment seg, Pen pen, Brush fontBrush, Font font)
        {
            g.DrawLine(pen, seg.Start, seg.End);
            float len = GeometryHelper.Distance(seg.Start, seg.End);
            var mid = GeometryHelper.MidPoint(seg.Start, seg.End);
            g.DrawString($"{len:F1}", font, fontBrush, mid.X + 5, mid.Y + 5);
            g.FillEllipse(Brushes.Red, seg.Start.X - 3, seg.Start.Y - 3, 7, 7);
            g.FillEllipse(Brushes.Red, seg.End.X - 3, seg.End.Y - 3, 7, 7);
        }

        /// <summary>
        /// 画弧段并显示弧长
        /// </summary>
        public static void DrawArcSegment(Graphics g, ArcSegment arc, Pen pen, Brush fontBrush, Font font)
        {
            float r = GeometryHelper.Distance(arc.Center, arc.Start);
            float a1 = (float)(System.Math.Atan2(arc.Start.Y - arc.Center.Y, arc.Start.X - arc.Center.X) * 180 / System.Math.PI);
            float a2 = (float)(System.Math.Atan2(arc.End.Y - arc.Center.Y, arc.End.X - arc.Center.X) * 180 / System.Math.PI);
            float sweep = a2 - a1;
            while (sweep < 0) sweep += 360;
            while (sweep > 360) sweep -= 360;

            RectangleF rect = new RectangleF(arc.Center.X - r, arc.Center.Y - r, 2 * r, 2 * r);
            g.DrawArc(pen, rect, a1, sweep);

            float arcLen = (float)(r * System.Math.PI * sweep / 180.0);
            float midAngle = a1 + sweep / 2;
            float rad = (float)(midAngle * System.Math.PI / 180.0);
            PointF midPt = new PointF(
                arc.Center.X + r * (float)System.Math.Cos(rad),
                arc.Center.Y + r * (float)System.Math.Sin(rad)
            );
            g.DrawString($"弧长:{arcLen:F1}", font, fontBrush, midPt.X + 5, midPt.Y + 5);

            g.FillEllipse(Brushes.Green, arc.Center.X - 3, arc.Center.Y - 3, 7, 7);
            g.FillEllipse(Brushes.Red, arc.Start.X - 3, arc.Start.Y - 3, 7, 7);
            g.FillEllipse(Brushes.Red, arc.End.X - 3, arc.End.Y - 3, 7, 7);
            g.DrawLine(Pens.Gray, arc.Center, arc.Start);
            g.DrawLine(Pens.Gray, arc.Center, arc.End);
        }
    }
}
