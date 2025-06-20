using DevExpress.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GlassCuttingOptimization.Views.GlassView
{
    public class GlassPanel : Panel
    {
        private int glassWidth = 1000;  // 玻璃件宽度 (mm)
        private int glassHeight = 600;  // 玻璃件高度 (mm)
        private float scale = 0.3f;     // 缩放比例，用于适应Panel显示

        public GlassPanel()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.UserPaint |
                          ControlStyles.DoubleBuffer |
                          ControlStyles.ResizeRedraw, true);
            this.BackColor = Color.White;
        }

        // 设置玻璃件尺寸的方法
        public void SetGlassSize(int width, int height)
        {
            this.glassWidth = width;
            this.glassHeight = height;

            // 根据Panel大小自动调整缩放比例
            if (this.Width > 0 && this.Height > 0)
            {
                float scaleX = (this.Width - 100f) / width;   // 留100像素边距
                float scaleY = (this.Height - 100f) / height; // 留100像素边距
                this.scale = Math.Min(scaleX, scaleY);
                this.scale = Math.Min(this.scale, 1.0f); // 最大不超过1:1
            }

            this.Invalidate(); // 重绘
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            // 计算实际绘制尺寸
            float drawWidth = glassWidth * scale;
            float drawHeight = glassHeight * scale;

            // 计算居中位置
            float centerX = this.Width / 2f;
            float centerY = this.Height / 2f;
            float rectX = centerX - drawWidth / 2f;
            float rectY = centerY - drawHeight / 2f;

            // 绘制玻璃件矩形
            DrawGlassRectangle(g, rectX, rectY, drawWidth, drawHeight);

            // 绘制尺寸标注
            DrawDimensions(g, rectX, rectY, drawWidth, drawHeight);
        }

        private void DrawGlassRectangle(Graphics g, float x, float y, float width, float height)
        {
            // 创建蓝绿色渐变画刷
            using (LinearGradientBrush brush = new LinearGradientBrush(
                new RectangleF(x, y, width, height),
                Color.FromArgb(150, 0, 150, 150),    // 半透明蓝绿色
                Color.FromArgb(100, 0, 200, 200),    // 更透明的蓝绿色
                LinearGradientMode.Vertical))
            {
                g.FillRectangle(brush, x, y, width, height);
            }

            // 绘制边框
            using (Pen borderPen = new Pen(Color.FromArgb(255, 0, 128, 128), 2f))
            {
                g.DrawRectangle(borderPen, x, y, width, height);
            }

            // 添加玻璃反光效果
            using (LinearGradientBrush reflectionBrush = new LinearGradientBrush(
                new RectangleF(x, y, width, height * 0.3f),
                Color.FromArgb(80, 255, 255, 255),
                Color.FromArgb(0, 255, 255, 255),
                LinearGradientMode.Vertical))
            {
                g.FillRectangle(reflectionBrush, x, y, width, height * 0.3f);
            }
        }


  
        private void DrawDimensions(Graphics g, float rectX, float rectY, float rectWidth, float rectHeight)
        {
            using (Font font = new Font("Arial", 10, FontStyle.Bold))
            using (Brush textBrush = new SolidBrush(Color.Black))
            using (Pen dimensionPen = new Pen(Color.Black, 1f))
            {
                // 绘制宽度标注 (下方)
                float widthLineY = rectY + rectHeight + 20;
                string widthText = $"{glassWidth} mm";
                SizeF widthTextSize = g.MeasureString(widthText, font);

                // 宽度标注线
                g.DrawLine(dimensionPen, rectX, widthLineY, rectX + rectWidth, widthLineY);
                g.DrawLine(dimensionPen, rectX, widthLineY - 5, rectX, widthLineY + 5);
                g.DrawLine(dimensionPen, rectX + rectWidth, widthLineY - 5, rectX + rectWidth, widthLineY + 5);

                // 宽度文字
                float widthTextX = rectX + (rectWidth - widthTextSize.Width) / 2;
                g.DrawString(widthText, font, textBrush, widthTextX, widthLineY + 8);

                // 绘制高度标注 (左侧) - 修改这部分
                float heightLineX = rectX - 20;  // 改为左侧，负值向左
                string heightText = $"{glassHeight} mm";
                SizeF heightTextSize = g.MeasureString(heightText, font);

                // 高度标注线
                g.DrawLine(dimensionPen, heightLineX, rectY, heightLineX, rectY + rectHeight);
                g.DrawLine(dimensionPen, heightLineX - 5, rectY, heightLineX + 5, rectY);
                g.DrawLine(dimensionPen, heightLineX - 5, rectY + rectHeight, heightLineX + 5, rectY + rectHeight);

                // 高度文字 (旋转90度，位置调整到左侧)
                g.TranslateTransform(heightLineX - 8 - heightTextSize.Height, rectY + (rectHeight + heightTextSize.Width) / 2);
                g.RotateTransform(-90);
                g.DrawString(heightText, font, textBrush, 0, 0);
                g.ResetTransform();
            }
        }
        private void DrawDimensions2(Graphics g, float rectX, float rectY, float rectWidth, float rectHeight)
        {
            using (Font font = new Font("Arial", 10, FontStyle.Bold))
            using (Brush textBrush = new SolidBrush(Color.Black))
            using (Pen dimensionPen = new Pen(Color.Black, 1f))
            {
                // 绘制宽度标注 (下方)
                float widthLineY = rectY + rectHeight + 20;
                string widthText = $"{glassWidth} mm";
                SizeF widthTextSize = g.MeasureString(widthText, font);

                // 宽度标注线
                g.DrawLine(dimensionPen, rectX, widthLineY, rectX + rectWidth, widthLineY);
                g.DrawLine(dimensionPen, rectX, widthLineY - 5, rectX, widthLineY + 5);
                g.DrawLine(dimensionPen, rectX + rectWidth, widthLineY - 5, rectX + rectWidth, widthLineY + 5);

                // 宽度文字
                float widthTextX = rectX + (rectWidth - widthTextSize.Width) / 2;
                g.DrawString(widthText, font, textBrush, widthTextX, widthLineY + 8);

                // 绘制高度标注 (右侧)
                float heightLineX = rectX + rectWidth + 20;
                string heightText = $"{glassHeight} mm";
                SizeF heightTextSize = g.MeasureString(heightText, font);

                // 高度标注线
                g.DrawLine(dimensionPen, heightLineX, rectY, heightLineX, rectY + rectHeight);
                g.DrawLine(dimensionPen, heightLineX - 5, rectY, heightLineX + 5, rectY);
                g.DrawLine(dimensionPen, heightLineX - 5, rectY + rectHeight, heightLineX + 5, rectY + rectHeight);

                // 高度文字 (旋转90度)
                g.TranslateTransform(heightLineX + 8 + heightTextSize.Height, rectY + (rectHeight + heightTextSize.Width) / 2);
                g.RotateTransform(-90);
                g.DrawString(heightText, font, textBrush, 0, 0);
                g.ResetTransform();
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            // Panel大小改变时重新计算缩放比例
            SetGlassSize(glassWidth, glassHeight);
        }
    }
}
