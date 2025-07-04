using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GlassCuttingOptimization.Views.Controls
{
    public partial class WelcomeControl : UserControl
    {
        private Timer animationTimer;
        private float animationProgress = 0f;
        private bool animationDirection = true;

        // 颜色主题
        private readonly Color PrimaryColor = Color.FromArgb(41, 128, 185);
        private readonly Color SecondaryColor = Color.FromArgb(52, 152, 219);
        private readonly Color AccentColor = Color.FromArgb(46, 204, 113);
        private readonly Color BackgroundColor = Color.FromArgb(236, 240, 241);
        private readonly Color TextColor = Color.FromArgb(44, 62, 80);
        private readonly Color SubTextColor = Color.FromArgb(127, 140, 141);

        public WelcomeControl()
        {
            InitializeComponent1();
            SetupAnimation();
        }

        private void InitializeComponent1()
        {
            this.SuspendLayout();

            // 设置控件属性
            this.BackColor = BackgroundColor;
            this.Dock = DockStyle.Fill;
            this.DoubleBuffered = true;
            this.Name = "WelcomeControl";
            this.Size = new Size(1000, 700);

            // 启用双缓冲减少闪烁
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                         ControlStyles.UserPaint |
                         ControlStyles.DoubleBuffer |
                         ControlStyles.ResizeRedraw, true);

            this.ResumeLayout(false);
        }

        private void SetupAnimation()
        {
            animationTimer = new Timer();
            animationTimer.Interval = 50; // 20 FPS
            animationTimer.Tick += AnimationTimer_Tick;
            animationTimer.Start();
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            if (animationDirection)
            {
                animationProgress += 0.02f;
                if (animationProgress >= 1.0f)
                {
                    animationProgress = 1.0f;
                    animationDirection = false;
                }
            }
            else
            {
                animationProgress -= 0.02f;
                if (animationProgress <= 0.0f)
                {
                    animationProgress = 0.0f;
                    animationDirection = true;
                }
            }

            this.Invalidate(); // 触发重绘
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            DrawBackground(g);
            DrawHeader(g);
            DrawMainContent(g);
            DrawFeatures(g);
            DrawFooter(g);
            DrawAnimatedElements(g);
        }

        private void DrawBackground(Graphics g)
        {
            // 渐变背景
            using (var brush = new LinearGradientBrush(
                this.ClientRectangle,
                Color.FromArgb(240, 248, 255),
                Color.FromArgb(230, 240, 250),
                LinearGradientMode.Vertical))
            {
                g.FillRectangle(brush, this.ClientRectangle);
            }

            // 装饰性几何图形
            DrawDecorativeShapes(g);
        }

        private void DrawDecorativeShapes(Graphics g)
        {
            // 右上角装饰圆圈
            using (var brush = new SolidBrush(Color.FromArgb(20, PrimaryColor)))
            {
                g.FillEllipse(brush, this.Width - 200, -100, 300, 300);
            }

            // 左下角装饰圆圈
            using (var brush = new SolidBrush(Color.FromArgb(15, AccentColor)))
            {
                g.FillEllipse(brush, -100, this.Height - 200, 250, 250);
            }

            // 中间装饰矩形
            using (var brush = new SolidBrush(Color.FromArgb(10, SecondaryColor)))
            {
                var rect = new Rectangle(this.Width / 2 - 150, this.Height / 2 - 100, 300, 200);
                g.FillRectangle(brush, rect);
            }
        }

        private void DrawHeader(Graphics g)
        {
            var headerRect = new Rectangle(0, 50, this.Width, 120);

            // 主标题
            using (var titleFont = new Font("Microsoft YaHei UI", 36, FontStyle.Bold))
            using (var titleBrush = new SolidBrush(PrimaryColor))
            {
                var title = "玻璃排版优化软件";
                var titleSize = g.MeasureString(title, titleFont);
                var titleX = (this.Width - titleSize.Width) / 2;
                var titleY = headerRect.Y + 20;

                g.DrawString(title, titleFont, titleBrush, titleX, titleY);
            }

            // 副标题
            using (var subFont = new Font("Microsoft YaHei UI", 14, FontStyle.Regular))
            using (var subBrush = new SolidBrush(SubTextColor))
            {
                var subtitle = "Glass Cutting Optimization System";
                var subSize = g.MeasureString(subtitle, subFont);
                var subX = (this.Width - subSize.Width) / 2;
                var subY = headerRect.Y + 80;

                g.DrawString(subtitle, subFont, subBrush, subX, subY);
            }
        }

        private void DrawMainContent(Graphics g)
        {
            var contentY = 220;
            var contentWidth = 600;
            var contentX = (this.Width - contentWidth) / 2;

            // 欢迎文字卡片
            var cardRect = new Rectangle(contentX, contentY, contentWidth, 120);
            DrawCard(g, cardRect);

            using (var welcomeFont = new Font("Microsoft YaHei UI", 18, FontStyle.Bold))
            using (var welcomeBrush = new SolidBrush(TextColor))
            {
                var welcomeText = "欢迎使用玻璃排版优化软件";
                var welcomeSize = g.MeasureString(welcomeText, welcomeFont);
                var welcomeX = cardRect.X + (cardRect.Width - welcomeSize.Width) / 2;
                var welcomeY = cardRect.Y + 25;

                g.DrawString(welcomeText, welcomeFont, welcomeBrush, welcomeX, welcomeY);
            }

            using (var descFont = new Font("Microsoft YaHei UI", 12, FontStyle.Regular))
            using (var descBrush = new SolidBrush(SubTextColor))
            {
                var description = "智能化玻璃切割路径规划，提升材料利用率，降低生产成本";
                var descSize = g.MeasureString(description, descFont);
                var descX = cardRect.X + (cardRect.Width - descSize.Width) / 2;
                var descY = cardRect.Y + 70;

                g.DrawString(description, descFont, descBrush, descX, descY);
            }
        }

        private void DrawFeatures(Graphics g)
        {
            var featuresY = 380;
            var featureWidth = 180;
            var featureHeight = 140;
            var spacing = 40;
            var totalWidth = 3 * featureWidth + 2 * spacing;
            var startX = (this.Width - totalWidth) / 2;

            var features = new[]
            {
                new { Icon = "📊", Title = "智能优化", Desc = "先进算法优化\n玻璃排版布局" },
                new { Icon = "⚡", Title = "高效处理", Desc = "快速生成切割\n路径和G代码" },
                new { Icon = "💾", Title = "数据管理", Desc = "完善的订单\n和库存管理" }
            };

            for (int i = 0; i < features.Length; i++)
            {
                var feature = features[i];
                var featureX = startX + i * (featureWidth + spacing);
                var featureRect = new Rectangle(featureX, featuresY, featureWidth, featureHeight);

                DrawFeatureCard(g, featureRect, feature.Icon, feature.Title, feature.Desc);
            }
        }

        private void DrawFeatureCard(Graphics g, Rectangle rect, string icon, string title, string description)
        {
            // 卡片背景
            using (var brush = new SolidBrush(Color.White))
            using (var pen = new Pen(Color.FromArgb(220, 220, 220), 1))
            {
                var cardRect = new Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
                g.FillRectangle(brush, cardRect);
                g.DrawRectangle(pen, cardRect);
            }

            // 添加阴影效果
            using (var shadowBrush = new SolidBrush(Color.FromArgb(20, 0, 0, 0)))
            {
                var shadowRect = new Rectangle(rect.X + 3, rect.Y + 3, rect.Width, rect.Height);
                g.FillRectangle(shadowBrush, shadowRect);
            }

            // 重新绘制卡片以覆盖阴影
            using (var brush = new SolidBrush(Color.White))
            {
                g.FillRectangle(brush, rect);
            }

            // 图标
            using (var iconFont = new Font("Segoe UI Emoji", 24, FontStyle.Regular))
            using (var iconBrush = new SolidBrush(AccentColor))
            {
                var iconSize = g.MeasureString(icon, iconFont);
                var iconX = rect.X + (rect.Width - iconSize.Width) / 2;
                var iconY = rect.Y + 20;
                g.DrawString(icon, iconFont, iconBrush, iconX, iconY);
            }

            // 标题
            using (var titleFont = new Font("Microsoft YaHei UI", 12, FontStyle.Bold))
            using (var titleBrush = new SolidBrush(TextColor))
            {
                var titleSize = g.MeasureString(title, titleFont);
                var titleX = rect.X + (rect.Width - titleSize.Width) / 2;
                var titleY = rect.Y + 70;
                g.DrawString(title, titleFont, titleBrush, titleX, titleY);
            }

            // 描述
            using (var descFont = new Font("Microsoft YaHei UI", 9, FontStyle.Regular))
            using (var descBrush = new SolidBrush(SubTextColor))
            {
                var lines = description.Split('\n');
                for (int i = 0; i < lines.Length; i++)
                {
                    var lineSize = g.MeasureString(lines[i], descFont);
                    var lineX = rect.X + (rect.Width - lineSize.Width) / 2;
                    var lineY = rect.Y + 95 + i * 15;
                    g.DrawString(lines[i], descFont, descBrush, lineX, lineY);
                }
            }
        }

        private void DrawCard(Graphics g, Rectangle rect)
        {
            // 阴影
            using (var shadowBrush = new SolidBrush(Color.FromArgb(30, 0, 0, 0)))
            {
                var shadowRect = new Rectangle(rect.X + 5, rect.Y + 5, rect.Width, rect.Height);
                g.FillRectangle(shadowBrush, shadowRect);
            }

            // 卡片背景
            using (var brush = new LinearGradientBrush(
                rect,
                Color.White,
                Color.FromArgb(250, 250, 250),
                LinearGradientMode.Vertical))
            {
                g.FillRectangle(brush, rect);
            }

            // 卡片边框
            using (var pen = new Pen(Color.FromArgb(200, 200, 200), 1))
            {
                g.DrawRectangle(pen, rect);
            }
        }

        private void DrawFooter(Graphics g)
        {
            var footerY = this.Height - 80;

            using (var footerFont = new Font("Microsoft YaHei UI", 10, FontStyle.Regular))
            using (var footerBrush = new SolidBrush(SubTextColor))
            {
                var currentTime = DateTime.Now.ToString("yyyy年MM月dd日 HH:mm");
                var footerText = $"当前时间: {currentTime} ";
                var footerSize = g.MeasureString(footerText, footerFont);
                var footerX = (this.Width - footerSize.Width) / 2;

                g.DrawString(footerText, footerFont, footerBrush, footerX, footerY);
            }

            // 版本信息
            using (var versionFont = new Font("Microsoft YaHei UI", 9, FontStyle.Regular))
            using (var versionBrush = new SolidBrush(Color.FromArgb(150, 150, 150)))
            {
                var versionText = "Version 1.0 | © 2025 Glass Cutting Optimization";
                var versionSize = g.MeasureString(versionText, versionFont);
                var versionX = (this.Width - versionSize.Width) / 2;
                var versionY = footerY + 25;

                g.DrawString(versionText, versionFont, versionBrush, versionX, versionY);
            }
        }

        private void DrawAnimatedElements(Graphics g)
        {
            // 动画脉冲效果
            var pulseAlpha = (int)(50 + 30 * animationProgress);
            using (var pulseBrush = new SolidBrush(Color.FromArgb(pulseAlpha, AccentColor)))
            {
                var pulseSize = (int)(20 + 10 * animationProgress);
                var pulseX = this.Width / 2 - pulseSize / 2;
                var pulseY = 180;
                g.FillEllipse(pulseBrush, pulseX, pulseY, pulseSize, pulseSize);
            }

            // 动画装饰线条
            using (var linePen = new Pen(Color.FromArgb((int)(100 * animationProgress), PrimaryColor), 2))
            {
                var lineY = 200;
                var lineWidth = (int)(200 * animationProgress);
                var lineX = this.Width / 2 - lineWidth / 2;
                g.DrawLine(linePen, lineX, lineY, lineX + lineWidth, lineY);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                animationTimer?.Stop();
                animationTimer?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}