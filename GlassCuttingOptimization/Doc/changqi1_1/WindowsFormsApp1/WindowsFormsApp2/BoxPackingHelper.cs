using System.Collections.Generic;
using System.Drawing;

namespace WindowsFormsApp2
{
    /// <summary>
    /// 最基础“顺序摆放”实现：仅展示，不做旋转/碰撞
    /// </summary>
    public static class BoxPackingHelper
    {
        public static List<(GraphicItem Item, PointF Offset)> SimplePlace(
            List<GraphicItem> items, int boxWidth, int boxHeight, out float totalShapeArea)
        {
            var result = new List<(GraphicItem, PointF)>();
            totalShapeArea = 0f;

            float curY = 0, maxRowH = 0, margin = 5;

            foreach (var item in items)
            {
                // 只演示多边形，弧可扩展
                if (item.Mode == InputMode.Polygon && item.PolyPoints.Count > 0)
                {
                    var pts = item.PolyPoints;
                    float minX = float.MaxValue, minY = float.MaxValue, maxX = float.MinValue, maxY = float.MinValue;
                    foreach (var pt in pts)
                    {
                        if (pt.X < minX) minX = pt.X;
                        if (pt.X > maxX) maxX = pt.X;
                        if (pt.Y < minY) minY = pt.Y;
                        if (pt.Y > maxY) maxY = pt.Y;
                    }
                    float w = maxX - minX;
                    float h = maxY - minY;

                    if (curY + h + margin > boxHeight) break; // 不下沉，演示用
                    var offset = new PointF(margin - minX, curY + margin - minY);
                    result.Add((item, offset));
                    curY += h + margin;
                    totalShapeArea += PolygonArea(pts.ToArray());
                }
            }
            return result;
        }

        // 多边形面积
        public static float PolygonArea(PointF[] pts)
        {
            float area = 0;
            int n = pts.Length;
            for (int i = 0; i < n; i++)
            {
                int j = (i + 1) % n;
                area += (pts[i].X * pts[j].Y) - (pts[j].X * pts[i].Y);
            }
            return System.Math.Abs(area / 2f);
        }
    }
}
