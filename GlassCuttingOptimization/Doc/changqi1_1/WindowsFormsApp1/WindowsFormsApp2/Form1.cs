using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        private int boxWidth = 0, boxHeight = 0;

        private List<CompoundShape> items = new List<CompoundShape>();
        private CompoundShape currentShape = new CompoundShape();

        private PointF? tempStart = null;
        private PointF tempEnd;
        private bool isDragging = false;
        private enum MyDrawMode { None, LineStart, LineEnd, ArcStart, ArcEnd, ArcCenter }
        private MyDrawMode drawMode = MyDrawMode.LineStart;
        private List<PointF> polygonPoints = new List<PointF>();

        public Form1()
        {
            InitializeComponent();

            panel1.MouseDown += panel1_MouseDown;
            panel1.MouseMove += panel1_MouseMove;
            panel1.MouseUp += panel1_MouseUp;
            panel1.Paint += panel1_Paint;

            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            comboBox1.SelectedIndex = 0;

            button2.Click += button2_Click;
            button4.Click += button4_Click;

            panel2.Paint += panel2_Paint;

            button3.Click += button3_Click;
            button1.Click += button1_Click;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBox1.Text, out int w) && int.TryParse(textBox2.Text, out int h) && w > 0 && h > 0)
            {
                boxWidth = w;
                boxHeight = h;
                label5.Text = $"盒子尺寸: {w} × {h}";
                panel2.Invalidate();
            }
            else
            {
                MessageBox.Show("请输入有效的盒子宽度和高度！");
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            drawMode = comboBox1.SelectedIndex == 0 ? MyDrawMode.LineStart : MyDrawMode.ArcStart;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            items.Clear();
            currentShape = new CompoundShape();
            drawMode = MyDrawMode.LineStart;
            panel1.Invalidate();
            panel2.Invalidate();
        }


        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (comboBox1.SelectedIndex == 0) // 多边形模式
            {
                if (drawMode == MyDrawMode.LineStart && e.Button == MouseButtons.Left)
                {
                    tempStart = e.Location;
                    isDragging = true;
                    drawMode = MyDrawMode.LineEnd;
                }
                // 右键闭合多边形
                else if (drawMode == MyDrawMode.LineStart && e.Button == MouseButtons.Right && polygonPoints.Count >= 2)
                {
                    // 闭合：加首尾一段
                    currentShape.Segments.Add(new LineSegment(polygonPoints[polygonPoints.Count - 1], polygonPoints[0]));
                    // 清理点
                    polygonPoints.Clear();
                    isDragging = false;
                    drawMode = MyDrawMode.LineStart;
                    tempStart = null;
                    tempEnd = PointF.Empty;
                    panel1.Invalidate();
                }
            }
            else // 弧形模式
            {
                if (drawMode == MyDrawMode.ArcStart && e.Button == MouseButtons.Left)
                {
                    tempStart = e.Location;
                    isDragging = true;
                    drawMode = MyDrawMode.ArcEnd;
                }
            }
        }




        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                tempEnd = e.Location;
                panel1.Invalidate();
            }
        }


        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            if (comboBox1.SelectedIndex == 0 && drawMode == MyDrawMode.LineEnd && isDragging)
            {
                tempEnd = e.Location;
                if (tempStart != null)
                {
                    currentShape.Segments.Add(new LineSegment(tempStart.Value, tempEnd));
                    // 录入点用于闭合
                    if (polygonPoints.Count == 0) polygonPoints.Add(tempStart.Value);
                    polygonPoints.Add(tempEnd);
                }
                isDragging = false;
                drawMode = MyDrawMode.LineStart;
                tempStart = null;
                panel1.Invalidate();
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                if (drawMode == MyDrawMode.ArcEnd && isDragging)
                {
                    tempEnd = e.Location;
                    isDragging = false;
                    drawMode = MyDrawMode.ArcCenter;
                    panel1.Invalidate();
                    MessageBox.Show("请点击圆心位置");
                }
                else if (drawMode == MyDrawMode.ArcCenter && e.Button == MouseButtons.Left)
                {
                    var arcCenter = e.Location;
                    if (tempStart != null)
                    {
                        currentShape.Segments.Add(new ArcSegment(tempStart.Value, tempEnd, arcCenter));
                    }
                    drawMode = MyDrawMode.ArcStart;
                    panel1.Invalidate();
                }
            }
        }






        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.Clear(Color.White);

            foreach (var seg in currentShape.Segments)
            {
                if (seg is LineSegment l)
                    GraphicsHelper.DrawLineSegment(g, l, Pens.Blue, Brushes.Black, this.Font);
                else if (seg is ArcSegment a)
                    GraphicsHelper.DrawArcSegment(g, a, Pens.Orange, Brushes.DarkRed, this.Font);
            }
            if (isDragging)
            {
                if (drawMode == MyDrawMode.LineEnd && tempStart != null)
                    g.DrawLine(Pens.DarkBlue, tempStart.Value, tempEnd);
                else if (drawMode == MyDrawMode.ArcEnd && tempStart != null)
                    g.DrawLine(Pens.DarkGray, tempStart.Value, tempEnd);
            }
        }



        private void button2_Click(object sender, EventArgs e)
        {
            if (currentShape.Segments.Count > 0)
            {
                items.Add(currentShape);
                currentShape = new CompoundShape();
                drawMode = (comboBox1.SelectedIndex == 0) ? MyDrawMode.LineStart : MyDrawMode.ArcStart;
                polygonPoints.Clear(); // <--- 新增
                panel1.Invalidate();
                panel2.Invalidate();
                MessageBox.Show("异型图形已添加到结果区！");
            }
            else
            {
                MessageBox.Show("当前没有录入任何线段或弧段，无法保存！");
            }
        }



        private void button3_Click(object sender, EventArgs e)
        {
            // TODO: 这里可调用你的装箱算法，用 items 作为输入
            // packedResult = BoxPackingHelper.SimplePlace(items, boxWidth, boxHeight, out totalShapeArea);
            // panel2.Invalidate();
            // float boxArea = boxWidth * boxHeight;
            // float ratio = (boxArea > 0) ? (totalShapeArea / boxArea) * 100f : 0;
            // label8.Text = $"装箱率：{ratio:F2}%";
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.Clear(Color.White);

            if (boxWidth > 0 && boxHeight > 0)
            {
                g.DrawRectangle(Pens.Black, 0, 0, boxWidth, boxHeight);
            }
            foreach (var shape in items)
            {
                foreach (var seg in shape.Segments)
                {
                    if (seg is LineSegment l)
                        GraphicsHelper.DrawLineSegment(g, l, Pens.Blue, Brushes.Black, this.Font);
                    else if (seg is ArcSegment a)
                        GraphicsHelper.DrawArcSegment(g, a, Pens.Orange, Brushes.DarkRed, this.Font);
                }
            }
        }
    }
}
