using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using netDxf;
using netDxf.Entities;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private List<List<PointF>> importedShape = new List<List<PointF>>();
        private float zoomFactor = 1.0f;
        private List<List<PointF>> packingShapes = new List<List<PointF>>(); // 用于 panel2 排列结果
        private float boxWidth = 0;
        private float boxHeight = 0;
        public Form1()
        {
            InitializeComponent();

            // 注册事件

            panel1.Paint += Panel1_Paint;
            panel1.MouseWheel += Panel1_MouseWheel;
            panel1.TabStop = true;
            panel1.Focus();


            panel2.Paint += panel2_Paint;
        }

        // 导入图纸按钮

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "DXF 文件|*.dxf";
            if (ofd.ShowDialog() != DialogResult.OK) return;

            string filePath = ofd.FileName;
            DxfDocument dxf;
            try
            {
                dxf = DxfDocument.Load(filePath);
            }
            catch
            {
                MessageBox.Show("DXF 文件加载失败");
                return;
            }

            importedShape.Clear();

            // 1. LwPolyline
            foreach (var pl in dxf.LwPolylines.Where(p => p.IsClosed))
            {
                var pts = pl.Vertexes.Select(v => new PointF((float)v.Position.X, (float)v.Position.Y)).ToList();
                importedShape.Add(pts);
                break;
            }

            // 2. Polyline
            if (importedShape.Count == 0)
            {
                foreach (var pl in dxf.Polylines.Where(p => p.IsClosed))
                {
                    var pts = pl.Vertexes.Select(v => new PointF((float)v.Position.X, (float)v.Position.Y)).ToList();
                    importedShape.Add(pts);
                    break;
                }
            }

            // 3. Lines 构成闭合图形
            if (importedShape.Count == 0)
            {
                var lines = dxf.Lines.ToList();
                if (lines.Count >= 3)
                {
                    var loop = TryFormClosedLoop(lines);
                    if (loop != null)
                        importedShape.Add(loop);
                }
            }

            // 4. Circle 转折线近似
            if (importedShape.Count == 0)
            {
                foreach (var circle in dxf.Circles)
                {
                    var center = circle.Center;
                    float r = (float)circle.Radius;
                    var points = new List<PointF>();
                    for (int i = 0; i < 36; i++)
                    {
                        double angle = Math.PI * 2 * i / 36;
                        float x = (float)(center.X + r * Math.Cos(angle));
                        float y = (float)(center.Y + r * Math.Sin(angle));
                        points.Add(new PointF(x, y));
                    }
                    importedShape.Add(points);
                    break;
                }
            }

            // 5. Arc 折线近似
            if (importedShape.Count == 0)
            {
                var arcs = dxf.Arcs.ToList();
                if (arcs.Count > 0)
                {
                    var pts = new List<PointF>();
                    foreach (var arc in arcs)
                    {
                        var subPts = ApproximateArc(arc, 10);
                        pts.AddRange(subPts);
                    }
                    importedShape.Add(pts);
                }
            }

            // 显示状态与重绘
            if (importedShape.Count == 0)
            {
                label4.Text = "尚未加载图形";
                label4.ForeColor = Color.Red;
                MessageBox.Show("无法识别图形，图纸中可能无封闭轮廓。");
            }
            else
            {
                label4.Text = "图形已加载";
                label4.ForeColor = Color.Green;
                panel1.Invalidate(); // 重绘
            }
        }
      
        


   

        // Panel1 绘图
        private void Panel1_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(Color.White);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            if (importedShape.Count == 0)
            {
                g.DrawString("无图形", this.Font, Brushes.Gray, 10, 10);
                return;
            }

            var shape = importedShape[0];

            float minX = shape.Min(p => p.X);
            float minY = shape.Min(p => p.Y);
            float maxX = shape.Max(p => p.X);
            float maxY = shape.Max(p => p.Y);

            float shapeWidth = maxX - minX;
            float shapeHeight = maxY - minY;

            float baseScale = Math.Min(panel1.Width / shapeWidth, panel1.Height / shapeHeight) * 0.9f;
            float scale = baseScale * zoomFactor;

            float offsetX = (panel1.Width - shapeWidth * scale) / 2;
            float offsetY = (panel1.Height - shapeHeight * scale) / 2;

            PointF Transform(PointF pt)
            {
                return new PointF(
                    offsetX + (pt.X - minX) * scale,
                    panel1.Height - (offsetY + (pt.Y - minY) * scale)
                );
            }

            using (Pen pen = new Pen(Color.Blue, 2))
            using (Font font = new Font("Arial", 8))
            {
                for (int i = 0; i < shape.Count; i++)
                {
                    PointF p1 = Transform(shape[i]);
                    PointF p2 = Transform(shape[(i + 1) % shape.Count]);

                    // 绘制边线
                    g.DrawLine(pen, p1, p2);

                    // 边长标注
                    float cx = (p1.X + p2.X) / 2;
                    float cy = (p1.Y + p2.Y) / 2;

                    var origLen = Math.Sqrt(
                        Math.Pow(shape[(i + 1) % shape.Count].X - shape[i].X, 2) +
                        Math.Pow(shape[(i + 1) % shape.Count].Y - shape[i].Y, 2));

                    string label = $"{origLen:F1} mm";
                    g.DrawString(label, font, Brushes.Black, cx, cy);
                }
            }
        }

        // 鼠标滚轮缩放
        private void Panel1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
                zoomFactor *= 1.1f;
            else
                zoomFactor /= 1.1f;

            panel1.Invalidate();
        }

        // Arc 折线近似
        private List<PointF> ApproximateArc(Arc arc, int segments)
        {
            List<PointF> points = new List<PointF>();
            double startRad = arc.StartAngle * Math.PI / 180;
            double endRad = arc.EndAngle * Math.PI / 180;
            double angleStep = (endRad - startRad) / segments;

            for (int i = 0; i <= segments; i++)
            {
                double angle = startRad + angleStep * i;
                double x = arc.Center.X + arc.Radius * Math.Cos(angle);
                double y = arc.Center.Y + arc.Radius * Math.Sin(angle);
                points.Add(new PointF((float)x, (float)y));
            }

            return points;
        }

        // 把 Line 拼接成闭合图形
        private List<PointF> TryFormClosedLoop(List<netDxf.Entities.Line> lines)
        {
            if (lines.Count < 3) return null;

            var pts = new List<PointF>();
            var used = new HashSet<netDxf.Entities.Line>();

            var start = lines[0].StartPoint;
            pts.Add(new PointF((float)start.X, (float)start.Y));
            var current = lines[0];
            used.Add(current);

            var endPoint = current.EndPoint;

            while (true)
            {
                var next = lines.FirstOrDefault(l =>
                    !used.Contains(l) &&
                    (l.StartPoint.Equals(endPoint) || l.EndPoint.Equals(endPoint)));

                if (next == null) break;

                used.Add(next);
                var nextPt = next.StartPoint.Equals(endPoint) ? next.EndPoint : next.StartPoint;
                pts.Add(new PointF((float)nextPt.X, (float)nextPt.Y));

                endPoint = nextPt;

                if (Math.Abs(endPoint.X - start.X) < 1e-4 &&
                    Math.Abs(endPoint.Y - start.Y) < 1e-4)
                {
                    return pts; // 成环
                }
            }

            return null;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (importedShape.Count == 0)
            {
                MessageBox.Show("请先导入图形！");
                return;
            }

            if (!float.TryParse(textBox1.Text, out boxHeight) || !float.TryParse(textBox2.Text, out boxWidth))
            {
                MessageBox.Show("请输入有效的盒子宽高！");
                return;
            }

            int maxCount = int.MaxValue;
            if (int.TryParse(textBox3.Text, out int val) && val > 0)
                maxCount = val;

            // 取原图形的最小外接矩形
            var shape = importedShape[0];
            float minX = shape.Min(p => p.X);
            float minY = shape.Min(p => p.Y);
            float maxX = shape.Max(p => p.X);
            float maxY = shape.Max(p => p.Y);

            float sWidth = maxX - minX;
            float sHeight = maxY - minY;

            // 平移到原点
            List<PointF> baseShape = shape.Select(p => new PointF(p.X - minX, p.Y - minY)).ToList();

            // 行列数
            int cols = (int)(boxWidth / sWidth);
            int rows = (int)(boxHeight / sHeight);
            int total = Math.Min(maxCount, cols * rows);

            packingShapes.Clear();
            int placed = 0;
            for (int r = 0; r < rows && placed < total; r++)
            {
                for (int c = 0; c < cols && placed < total; c++)
                {
                    // 复制并平移
                    List<PointF> inst = baseShape.Select(pt =>
                        new PointF(pt.X + c * sWidth, pt.Y + r * sHeight)
                    ).ToList();
                    packingShapes.Add(inst);
                    placed++;
                }
            }

            // 计算面积
            float shapeArea = Math.Abs(PolygonArea(baseShape));
            float boxArea = boxWidth * boxHeight;
            float fillRate = (packingShapes.Count * shapeArea) / boxArea * 100;

            label5.Text = $"填充率：{fillRate:F2}% (共放置{packingShapes.Count}个)";
            panel2.Invalidate();
        }


        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(Color.White);

            if (packingShapes.Count == 0) return;

            // 箱子轮廓
            float scale = Math.Min(panel2.Width / boxWidth, panel2.Height / boxHeight) * 0.95f;
            float offsetX = (panel2.Width - boxWidth * scale) / 2;
            float offsetY = (panel2.Height - boxHeight * scale) / 2;

            // 绘制盒子外框
            g.DrawRectangle(Pens.Gray, offsetX, offsetY, boxWidth * scale, boxHeight * scale);

            using (Pen pen = new Pen(Color.Blue, 1))
            {
                foreach (var shape in packingShapes)
                {
                    PointF[] arr = shape.Select(pt =>
                        new PointF(offsetX + pt.X * scale, offsetY + (boxHeight - pt.Y - (boxHeight - boxHeight)) * scale)
                    ).ToArray();
                    g.DrawPolygon(pen, arr);
                }
            }
        }

        private float PolygonArea(List<PointF> pts)
        {
            float area = 0;
            for (int i = 0; i < pts.Count; i++)
            {
                PointF p1 = pts[i];
                PointF p2 = pts[(i + 1) % pts.Count];
                area += (p1.X * p2.Y - p2.X * p1.Y);
            }
            return area / 2f;
        }


    }
}
