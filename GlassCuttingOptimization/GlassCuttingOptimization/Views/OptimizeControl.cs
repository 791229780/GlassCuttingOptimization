//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Drawing;
//using System.Data;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;
//using AntdUI;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement;
//using System.Drawing.Drawing2D;
//using Panel = System.Windows.Forms.Panel;
//using Label = System.Windows.Forms.Label;
//using ListView = System.Windows.Forms.ListView;

//namespace GlassCuttingOptimization.Views
//{
//    public partial class OptimizeControl: UserControl
//    {
//        private List<GlassSheet> availableSheets;
//        private List<GlassPiece> orderPieces;
//        private OptimizationResult optimizationResult;

//        // UI Components - 直接引用避免索引访问
//        private Panel mainVisualizationPanel;
//        private Panel sheetListPanel;
//        private Panel controlPanel;
//        private Label currentSheetLabel;
//        private Label utilizationLabel;
//        private Label wasteLabel;
//        private Label pieceCountLabel;
//        private System.Windows.Forms.ProgressBar efficiencyBar;
//        private FlowLayoutPanel thumbnailContainer;

//        private int selectedSheetIndex = 0;
//        private readonly Color[] pieceColors = new Color[]
//        {
//            Color.FromArgb(180, 52, 152, 219),   // 蓝色
//            Color.FromArgb(180, 46, 204, 113),   // 绿色
//            Color.FromArgb(180, 155, 89, 182),   // 紫色
//            Color.FromArgb(180, 241, 196, 15),   // 黄色
//            Color.FromArgb(180, 230, 126, 34),   // 橙色
//            Color.FromArgb(180, 231, 76, 60),    // 红色
//            Color.FromArgb(180, 26, 188, 156),   // 青色
//            Color.FromArgb(180, 243, 156, 18),   // 金色
//            Color.FromArgb(180, 192, 57, 43),    // 深红色
//            Color.FromArgb(180, 142, 68, 173)    // 深紫色
//        };

//        public OptimizeControl()
//        {
//            InitializeComponent1();
//            InitializeData1();
//            SetupUI();

//            // 在所有控件创建完成后初始化显示
//            this.Load += MainForm_Load;
//        }

//        private void MainForm_Load(object sender, EventArgs e)
//        {
//            // 初始化选择第一个板材
//            SelectSheet(0);
//        }

//        private void InitializeComponent1()
//        {
//            this.Text = "玻璃优化排版系统 - 专业版 v2.0";
//            this.Size = new Size(1600, 1000);
//            this.BackColor = Color.FromArgb(245, 247, 250);
//            this.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Regular);
//        }

//        private void InitializeData1()
//        {
//            // 模拟大量订单数据
//            availableSheets = new List<GlassSheet>
//            {
//                new GlassSheet { Id = 1, Width = 3210, Height = 2250, Thickness = 6, Name = "6mm白玻" },
//                new GlassSheet { Id = 2, Width = 3300, Height = 2100, Thickness = 8, Name = "8mm钢化玻璃" },
//                new GlassSheet { Id = 3, Width = 2440, Height = 1830, Thickness = 5, Name = "5mm浮法玻璃" }
//            };

//            // 模拟多个订单的玻璃片
//            orderPieces = GenerateOrderPieces();

//            // 执行优化算法
//            optimizationResult = OptimizeLayout();
//        }

//        private List<GlassPiece> GenerateOrderPieces()
//        {
//            var pieces = new List<GlassPiece>();
//            var random = new Random(42); // 固定种子以获得一致的结果

//            var pieceTemplates = new[]
//            {
//                new { Width = 800, Height = 600, Name = "标准窗户" },
//                new { Width = 1200, Height = 800, Name = "大型窗户" },
//                new { Width = 400, Height = 300, Name = "小窗户" },
//                new { Width = 600, Height = 400, Name = "中型窗户" },
//                new { Width = 900, Height = 500, Name = "横向窗户" },
//                new { Width = 500, Height = 800, Name = "竖向窗户" },
//                new { Width = 1000, Height = 600, Name = "宽窗户" },
//                new { Width = 300, Height = 200, Name = "装饰窗" },
//                new { Width = 700, Height = 700, Name = "方形窗户" },
//                new { Width = 1100, Height = 900, Name = "大方窗" }
//            };

//            // 生成多个订单
//            for (int orderId = 1; orderId <= 15; orderId++)
//            {
//                int pieceCount = random.Next(3, 8);
//                for (int i = 0; i < pieceCount; i++)
//                {
//                    var template = pieceTemplates[random.Next(pieceTemplates.Length)];
//                    pieces.Add(new GlassPiece
//                    {
//                        Id = pieces.Count + 1,
//                        Width = template.Width + random.Next(-50, 51),
//                        Height = template.Height + random.Next(-50, 51),
//                        Quantity = random.Next(1, 4),
//                        Name = $"订单{orderId:D3}-{template.Name}",
//                        OrderId = orderId,
//                        Thickness = availableSheets[random.Next(availableSheets.Count)].Thickness
//                    });
//                }
//            }

//            return pieces;
//        }

//        private void SetupUI()
//        {
//            var mainContainer = new TableLayoutPanel
//            {
//                Dock = DockStyle.Fill,
//                RowCount = 1,
//                ColumnCount = 3,
//                Padding = new Padding(8)
//            };

//            // 设置列宽比例：主视图70%，控制面板20%，间距10%
//            mainContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
//            mainContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 10F));
//            mainContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));

//            // 左侧主视图区域
//            var leftPanel = CreateMainVisualizationArea();
//            mainContainer.Controls.Add(leftPanel, 0, 0);

//            // 右侧控制面板
//            controlPanel = CreateControlPanel();
//            mainContainer.Controls.Add(controlPanel, 2, 0);

//            this.Controls.Add(mainContainer);
//        }

//        private Panel CreateMainVisualizationArea()
//        {
//            var container = new Panel
//            {
//                Dock = DockStyle.Fill,
//                BackColor = Color.White,
//                BorderStyle = BorderStyle.FixedSingle
//            };

//            var layout = new TableLayoutPanel
//            {
//                Dock = DockStyle.Fill,
//                RowCount = 3,
//                ColumnCount = 1,
//                Padding = new Padding(5)
//            };

//            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));  // 标题栏
//            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 70F));   // 主视图
//            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));   // 板材列表

//            // 标题栏
//            var titlePanel = CreateTitlePanel();
//            layout.Controls.Add(titlePanel, 0, 0);

//            // 主要可视化面板
//            mainVisualizationPanel = new Panel
//            {
//                Dock = DockStyle.Fill,
//                BackColor = Color.White,
//                BorderStyle = BorderStyle.FixedSingle
//            };
//            mainVisualizationPanel.Paint += MainVisualizationPanel_Paint;
//            layout.Controls.Add(mainVisualizationPanel, 0, 1);

//            // 板材列表面板
//            sheetListPanel = CreateSheetListPanel();
//            layout.Controls.Add(sheetListPanel, 0, 2);

//            container.Controls.Add(layout);
//            return container;
//        }

//        private Panel CreateTitlePanel()
//        {
//            var panel = new Panel
//            {
//                Dock = DockStyle.Fill,
//                BackColor = Color.FromArgb(52, 73, 94)
//            };

//            currentSheetLabel = new Label
//            {
//                Text = "板材排版视图 - 正在加载...",
//                Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Bold),
//                ForeColor = Color.White,
//                Dock = DockStyle.Fill,
//                TextAlign = ContentAlignment.MiddleLeft,
//                Padding = new Padding(15, 0, 0, 0)
//            };
//            panel.Controls.Add(currentSheetLabel);

//            return panel;
//        }

//        private Panel CreateSheetListPanel()
//        {
//            var panel = new Panel
//            {
//                Dock = DockStyle.Fill,
//                BackColor = Color.FromArgb(248, 249, 250),
//                BorderStyle = BorderStyle.FixedSingle
//            };

//            var titleLabel = new Label
//            {
//                Text = "所有板材排版预览 (点击查看详细)",
//                Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold),
//                ForeColor = Color.FromArgb(52, 73, 94),
//                Dock = DockStyle.Top,
//                Height = 30,
//                TextAlign = ContentAlignment.MiddleLeft,
//                Padding = new Padding(10, 5, 0, 0),
//                BackColor = Color.FromArgb(236, 240, 245)
//            };
//            panel.Controls.Add(titleLabel);

//            // 创建板材缩略图列表
//            var scrollPanel = new Panel
//            {
//                Dock = DockStyle.Fill,
//                AutoScroll = true,
//                BackColor = Color.FromArgb(248, 249, 250)
//            };

//            thumbnailContainer = new FlowLayoutPanel
//            {
//                Dock = DockStyle.Top,
//                FlowDirection = FlowDirection.LeftToRight,
//                WrapContents = true,
//                AutoSize = true,
//                Padding = new Padding(10)
//            };

//            // 添加板材缩略图
//            CreateThumbnails();

//            scrollPanel.Controls.Add(thumbnailContainer);
//            panel.Controls.Add(scrollPanel);

//            return panel;
//        }

//        private void CreateThumbnails()
//        {
//            thumbnailContainer.Controls.Clear();

//            for (int i = 0; i < optimizationResult.Layouts.Count; i++)
//            {
//                var thumbnail = CreateSheetThumbnail(i);
//                thumbnailContainer.Controls.Add(thumbnail);
//            }
//        }

//        private Panel CreateSheetThumbnail(int sheetIndex)
//        {
//            var layout = optimizationResult.Layouts[sheetIndex];
//            var sheet = availableSheets.First(s => s.Id == layout.SheetId);

//            var panel = new Panel
//            {
//                Size = new Size(200, 160),
//                BackColor = sheetIndex == selectedSheetIndex ? Color.FromArgb(52, 152, 219) : Color.White,
//                BorderStyle = BorderStyle.FixedSingle,
//                Margin = new Padding(5),
//                Cursor = Cursors.Hand,
//                Tag = sheetIndex
//            };

//            // 缩略图绘制面板
//            var drawPanel = new Panel
//            {
//                Size = new Size(180, 120),
//                Location = new Point(10, 5),
//                BackColor = Color.FromArgb(250, 250, 250),
//                BorderStyle = BorderStyle.FixedSingle
//            };
//            drawPanel.Paint += (s, e) => DrawThumbnail(e.Graphics, sheetIndex, drawPanel.Size);
//            panel.Controls.Add(drawPanel);

//            // 信息标签
//            var infoLabel = new Label
//            {
//                Text = $"板材 #{sheetIndex + 1}\n利用率: {layout.UtilizationRate:P1}\n{sheet.Name}",
//                Font = new Font("Microsoft YaHei UI", 8F),
//                ForeColor = sheetIndex == selectedSheetIndex ? Color.White : Color.FromArgb(52, 73, 94),
//                Location = new Point(5, 130),
//                Size = new Size(190, 25),
//                TextAlign = ContentAlignment.TopCenter
//            };
//            panel.Controls.Add(infoLabel);

//            // 点击事件
//            panel.Click += (s, e) => SelectSheet(sheetIndex);
//            drawPanel.Click += (s, e) => SelectSheet(sheetIndex);
//            infoLabel.Click += (s, e) => SelectSheet(sheetIndex);

//            return panel;
//        }

//        private void DrawThumbnail(Graphics g, int sheetIndex, Size panelSize)
//        {
//            if (sheetIndex >= optimizationResult.Layouts.Count) return;

//            var layout = optimizationResult.Layouts[sheetIndex];
//            var sheet = availableSheets.First(s => s.Id == layout.SheetId);

//            g.SmoothingMode = SmoothingMode.AntiAlias;

//            // 计算缩放比例
//            var scale = Math.Min((float)panelSize.Width / sheet.Width, (float)panelSize.Height / sheet.Height) * 0.9f;
//            var shWidth = sheet.Width * scale;
//            var shHeight = sheet.Height * scale;
//            var offsetX = (panelSize.Width - shWidth) / 2;
//            var offsetY = (panelSize.Height - shHeight) / 2;

//            // 绘制板材背景
//            var sheetRect = new RectangleF(offsetX, offsetY, shWidth, shHeight);
//            using (var brush = new SolidBrush(Color.FromArgb(230, 230, 230)))
//            {
//                g.FillRectangle(brush, sheetRect);
//            }

//            // 绘制玻璃片
//            var colorIndex = 0;
//            foreach (var placement in layout.Placements)
//            {
//                var pieceRect = new RectangleF(
//                    offsetX + placement.X * scale,
//                    offsetY + placement.Y * scale,
//                    placement.Width * scale,
//                    placement.Height * scale);

//                using (var brush = new SolidBrush(pieceColors[colorIndex % pieceColors.Length]))
//                {
//                    g.FillRectangle(brush, pieceRect);
//                }

//                using (var pen = new Pen(Color.FromArgb(100, 100, 100), 0.5f))
//                {
//                    g.DrawRectangle(pen, Rectangle.Round(pieceRect));
//                }

//                colorIndex++;
//            }

//            // 绘制边框
//            using (var pen = new Pen(Color.FromArgb(150, 150, 150), 1f))
//            {
//                g.DrawRectangle(pen, Rectangle.Round(sheetRect));
//            }
//        }

//        private void SelectSheet(int sheetIndex)
//        {
//            if (sheetIndex < 0 || sheetIndex >= optimizationResult.Layouts.Count || selectedSheetIndex == sheetIndex)
//                return;

//            selectedSheetIndex = sheetIndex;

//            // 更新标题
//            var layout = optimizationResult.Layouts[sheetIndex];
//            var sheet = availableSheets.First(s => s.Id == layout.SheetId);
//            currentSheetLabel.Text = $"板材 #{sheetIndex + 1} - {sheet.Name} ({sheet.Width}×{sheet.Height}mm) - 利用率: {layout.UtilizationRate:P2}";

//            // 刷新主视图
//            mainVisualizationPanel.Invalidate();

//            // 刷新缩略图选中状态
//            RefreshThumbnails();

//            // 更新统计信息
//            UpdateStatistics();
//        }

//        private void RefreshThumbnails()
//        {
//            for (int i = 0; i < thumbnailContainer.Controls.Count; i++)
//            {
//                var thumbnail = thumbnailContainer.Controls[i] as Panel;
//                if (thumbnail == null) continue;

//                var isSelected = i == selectedSheetIndex;
//                thumbnail.BackColor = isSelected ? Color.FromArgb(52, 152, 219) : Color.White;

//                if (thumbnail.Controls.Count > 1)
//                {
//                    var infoLabel = thumbnail.Controls[1] as Label;
//                    if (infoLabel != null)
//                    {
//                        infoLabel.ForeColor = isSelected ? Color.White : Color.FromArgb(52, 73, 94);
//                    }
//                }
//            }
//        }

//        private void MainVisualizationPanel_Paint(object sender, PaintEventArgs e)
//        {
//            if (optimizationResult?.Layouts == null || selectedSheetIndex >= optimizationResult.Layouts.Count)
//                return;

//            var g = e.Graphics;
//            g.SmoothingMode = SmoothingMode.AntiAlias;
//            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

//            var layout = optimizationResult.Layouts[selectedSheetIndex];
//            var sheet = availableSheets.First(s => s.Id == layout.SheetId);

//            // 计算缩放和居中
//            var panelWidth = mainVisualizationPanel.Width - 60;
//            var panelHeight = mainVisualizationPanel.Height - 60;
//            var scale = Math.Min((float)panelWidth / sheet.Width, (float)panelHeight / sheet.Height) * 0.85f;

//            var shWidth = sheet.Width * scale;
//            var shHeight = sheet.Height * scale;
//            var offsetX = (mainVisualizationPanel.Width - shWidth) / 2;
//            var offsetY = (mainVisualizationPanel.Height - shHeight) / 2;

//            // 绘制板材背景
//            var sheetRect = new RectangleF(offsetX, offsetY, shWidth, shHeight);
//            using (var bgBrush = new LinearGradientBrush(sheetRect,
//                Color.FromArgb(250, 250, 250), Color.FromArgb(235, 235, 235), 45f))
//            {
//                g.FillRectangle(bgBrush, sheetRect);
//            }

//            // 绘制网格
//            DrawGrid(g, sheetRect, scale);

//            // 绘制玻璃片
//            var colorIndex = 0;
//            var pieceToColorMap = new Dictionary<int, int>();

//            foreach (var placement in layout.Placements)
//            {
//                if (!pieceToColorMap.ContainsKey(placement.PieceId))
//                {
//                    pieceToColorMap[placement.PieceId] = colorIndex++;
//                }

//                var pieceRect = new RectangleF(
//                    offsetX + placement.X * scale,
//                    offsetY + placement.Y * scale,
//                    placement.Width * scale,
//                    placement.Height * scale);

//                // 绘制玻璃片主体
//                var color = pieceColors[pieceToColorMap[placement.PieceId] % pieceColors.Length];
//                using (var brush = new SolidBrush(color))
//                {
//                    g.FillRectangle(brush, pieceRect);
//                }

//                // 绘制边框
//                using (var pen = new Pen(Color.FromArgb(80, 80, 80), 1.5f))
//                {
//                    g.DrawRectangle(pen, Rectangle.Round(pieceRect));
//                }

//                // 绘制标签信息
//                if (pieceRect.Width > 80 && pieceRect.Height > 40)
//                {
//                    DrawPieceLabel(g, pieceRect, placement);
//                }
//            }

//            // 绘制尺寸标注
//            DrawDimensions(g, sheetRect, sheet);

//            // 绘制板材边框
//            using (var pen = new Pen(Color.FromArgb(52, 73, 94), 2f))
//            {
//                g.DrawRectangle(pen, Rectangle.Round(sheetRect));
//            }
//        }

//        private void DrawGrid(Graphics g, RectangleF sheetRect, float scale)
//        {
//            using (var pen = new Pen(Color.FromArgb(30, 200, 200, 200), 1f)
//            {
//                DashStyle = DashStyle.Dot
//            })
//            {
//                // 绘制网格线，每100mm一条线
//                var gridSpacing = 100 * scale;

//                // 垂直线
//                for (float x = sheetRect.Left + gridSpacing; x < sheetRect.Right; x += gridSpacing)
//                {
//                    g.DrawLine(pen, x, sheetRect.Top, x, sheetRect.Bottom);
//                }

//                // 水平线
//                for (float y = sheetRect.Top + gridSpacing; y < sheetRect.Bottom; y += gridSpacing)
//                {
//                    g.DrawLine(pen, sheetRect.Left, y, sheetRect.Right, y);
//                }
//            }
//        }

//        private void DrawPieceLabel(Graphics g, RectangleF pieceRect, PiecePlacement placement)
//        {
//            var font = new Font("Microsoft YaHei UI", 8F, FontStyle.Bold);
//            var textBrush = new SolidBrush(Color.White);
//            var shadowBrush = new SolidBrush(Color.FromArgb(100, 0, 0, 0));

//            var lines = new[]
//            {
//                placement.PieceName,
//                $"{placement.Width:F0}×{placement.Height:F0}mm",
//                $"订单{placement.OrderId:D3}"
//            };

//            var textRect = new RectangleF(pieceRect.X + 5, pieceRect.Y + 5,
//                pieceRect.Width - 10, pieceRect.Height - 10);

//            var lineHeight = font.Height + 2;
//            var totalHeight = lines.Length * lineHeight;
//            var startY = textRect.Y + (textRect.Height - totalHeight) / 2;

//            for (int i = 0; i < lines.Length; i++)
//            {
//                var y = startY + i * lineHeight;
//                var textSize = g.MeasureString(lines[i], font);
//                var x = textRect.X + (textRect.Width - textSize.Width) / 2;

//                // 绘制阴影
//                g.DrawString(lines[i], font, shadowBrush, x + 1, y + 1);
//                // 绘制文字
//                g.DrawString(lines[i], font, textBrush, x, y);
//            }

//            font.Dispose();
//            textBrush.Dispose();
//            shadowBrush.Dispose();
//        }

//        private void DrawDimensions(Graphics g, RectangleF sheetRect, GlassSheet sheet)
//        {
//            var font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
//            var brush = new SolidBrush(Color.FromArgb(52, 73, 94));
//            var pen = new Pen(Color.FromArgb(100, 100, 100), 1.5f);
//            var arrowPen = new Pen(Color.FromArgb(52, 73, 94), 2f);

//            // 绘制宽度标注
//            var widthY = sheetRect.Bottom + 15;
//            g.DrawLine(pen, sheetRect.Left, widthY, sheetRect.Right, widthY);

//            // 宽度箭头
//            DrawArrow(g, arrowPen, new PointF(sheetRect.Left, widthY), new PointF(sheetRect.Left + 20, widthY));
//            DrawArrow(g, arrowPen, new PointF(sheetRect.Right, widthY), new PointF(sheetRect.Right - 20, widthY));

//            var widthText = $"{sheet.Width:F0} mm";
//            var widthSize = g.MeasureString(widthText, font);
//            g.DrawString(widthText, font, brush,
//                sheetRect.Left + (sheetRect.Width - widthSize.Width) / 2, widthY + 8);

//            // 绘制高度标注
//            var heightX = sheetRect.Right + 15;
//            g.DrawLine(pen, heightX, sheetRect.Top, heightX, sheetRect.Bottom);

//            // 高度箭头
//            DrawArrow(g, arrowPen, new PointF(heightX, sheetRect.Top), new PointF(heightX, sheetRect.Top + 20));
//            DrawArrow(g, arrowPen, new PointF(heightX, sheetRect.Bottom), new PointF(heightX, sheetRect.Bottom - 20));

//            var heightText = $"{sheet.Height:F0} mm";
//            var heightSize = g.MeasureString(heightText, font);

//            g.TranslateTransform(heightX + 8, sheetRect.Top + (sheetRect.Height + heightSize.Width) / 2);
//            g.RotateTransform(-90);
//            g.DrawString(heightText, font, brush, 0, 0);
//            g.ResetTransform();

//            font.Dispose();
//            brush.Dispose();
//            pen.Dispose();
//            arrowPen.Dispose();
//        }

//        private void DrawArrow(Graphics g, Pen pen, PointF from, PointF to)
//        {
//            g.DrawLine(pen, from, to);

//            var angle = Math.Atan2(to.Y - from.Y, to.X - from.X);
//            var arrowLength = 8;
//            var arrowAngle = Math.PI / 6;

//            var arrowPoint1 = new PointF(
//                to.X - (float)(arrowLength * Math.Cos(angle - arrowAngle)),
//                to.Y - (float)(arrowLength * Math.Sin(angle - arrowAngle)));

//            var arrowPoint2 = new PointF(
//                to.X - (float)(arrowLength * Math.Cos(angle + arrowAngle)),
//                to.Y - (float)(arrowLength * Math.Sin(angle + arrowAngle)));

//            g.DrawLine(pen, to, arrowPoint1);
//            g.DrawLine(pen, to, arrowPoint2);
//        }

//        private Panel CreateControlPanel()
//        {
//            var panel = new Panel
//            {
//                Dock = DockStyle.Fill,
//                BackColor = Color.FromArgb(248, 249, 250),
//                BorderStyle = BorderStyle.FixedSingle,
//                Padding = new Padding(15)
//            };

//            var scrollPanel = new Panel
//            {
//                Dock = DockStyle.Fill,
//                AutoScroll = true
//            };

//            var y = 0;

//            // 标题
//            var titleLabel = new Label
//            {
//                Text = "优化统计报告",
//                Font = new Font("Microsoft YaHei UI", 14F, FontStyle.Bold),
//                ForeColor = Color.FromArgb(52, 73, 94),
//                Location = new Point(0, y),
//                Size = new Size(250, 35),
//                BackColor = Color.FromArgb(236, 240, 245),
//                TextAlign = ContentAlignment.MiddleCenter
//            };
//            scrollPanel.Controls.Add(titleLabel);
//            y += 50;

//            // 总体统计
//            var overallStats = CreateOverallStatsPanel(y);
//            scrollPanel.Controls.AddRange(overallStats.ToArray());
//            y += 200;

//            // 当前板材统计
//            var currentStats = CreateCurrentSheetStatsPanel(y);
//            scrollPanel.Controls.AddRange(currentStats.ToArray());
//            y += 180;

//            // 材料清单
//            var materialList = CreateMaterialListPanel(y);
//            scrollPanel.Controls.AddRange(materialList.ToArray());

//            panel.Controls.Add(scrollPanel);
//            return panel;
//        }

//        private List<Control> CreateOverallStatsPanel(int startY)
//        {
//            var controls = new List<Control>();
//            var y = startY;

//            var groupBox = new GroupBox
//            {
//                Text = "总体优化结果",
//                Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold),
//                ForeColor = Color.FromArgb(52, 73, 94),
//                Location = new Point(0, y),
//                Size = new Size(280, 180),
//                BackColor = Color.White
//            };

//            var totalUtilization = optimizationResult.Layouts.Average(l => l.UtilizationRate);
//            var totalWaste = optimizationResult.Layouts.Sum(l => l.WasteArea);

//            var stats = new[]
//            {
//                ("使用板材数量", $"{optimizationResult.Layouts.Count} 块"),
//                ("平均利用率", $"{totalUtilization:P2}"),
//                ("总浪费面积", $"{totalWaste:F2} mm²"),
//                ("总切割次数", $"{optimizationResult.TotalCuts}"),
//                ("优化时间", $"{optimizationResult.OptimizationTime:F2} 秒")
//            };

//            var itemY = 25;
//            foreach (var (label, value) in stats)
//            {
//                var labelControl = new Label
//                {
//                    Text = label + ":",
//                    Font = new Font("Microsoft YaHei UI", 9F),
//                    Location = new Point(15, itemY),
//                    Size = new Size(120, 20),
//                    ForeColor = Color.FromArgb(102, 102, 102)
//                };
//                groupBox.Controls.Add(labelControl);

//                var valueControl = new Label
//                {
//                    Text = value,
//                    Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold),
//                    Location = new Point(140, itemY),
//                    Size = new Size(120, 20),
//                    ForeColor = Color.FromArgb(52, 73, 94)
//                };
//                groupBox.Controls.Add(valueControl);

//                itemY += 25;
//            }

//            controls.Add(groupBox);
//            return controls;
//        }

//        private List<Control> CreateCurrentSheetStatsPanel(int startY)
//        {
//            var controls = new List<Control>();

//            var groupBox = new GroupBox
//            {
//                Text = "当前板材详情",
//                Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold),
//                ForeColor = Color.FromArgb(52, 73, 94),
//                Location = new Point(0, startY),
//                Size = new Size(280, 160),
//                BackColor = Color.White
//            };

//            utilizationLabel = new Label
//            {
//                Text = "利用率: --",
//                Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold),
//                Location = new Point(15, 30),
//                Size = new Size(250, 20),
//                ForeColor = Color.FromArgb(46, 204, 113)
//            };
//            groupBox.Controls.Add(utilizationLabel);

//            wasteLabel = new Label
//            {
//                Text = "浪费面积: --",
//                Font = new Font("Microsoft YaHei UI", 9F),
//                Location = new Point(15, 55),
//                Size = new Size(250, 20),
//                ForeColor = Color.FromArgb(231, 76, 60)
//            };
//            groupBox.Controls.Add(wasteLabel);

//            pieceCountLabel = new Label
//            {
//                Text = "玻璃片数量: --",
//                Font = new Font("Microsoft YaHei UI", 9F),
//                Location = new Point(15, 80),
//                Size = new Size(250, 20),
//                ForeColor = Color.FromArgb(52, 73, 94)
//            };
//            groupBox.Controls.Add(pieceCountLabel);

//            efficiencyBar = new System.Windows.Forms.ProgressBar
//            {
//                Location = new Point(15, 110),
//                Size = new Size(250, 20),
//                Style = ProgressBarStyle.Continuous,
//                Minimum = 0,
//                Maximum = 100
//            };
//            groupBox.Controls.Add(efficiencyBar);

//            controls.Add(groupBox);
//            return controls;
//        }

//        private List<Control> CreateMaterialListPanel(int startY)
//        {
//            var controls = new List<Control>();

//            var groupBox = new GroupBox
//            {
//                Text = "板材类型统计",
//                Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold),
//                ForeColor = Color.FromArgb(52, 73, 94),
//                Location = new Point(0, startY),
//                Size = new Size(280, 150),
//                BackColor = Color.White
//            };

//            var materialStats = availableSheets
//                .GroupBy(s => s.Name)
//                .Select(g => new {
//                    Name = g.Key,
//                    Count = optimizationResult.Layouts.Count(l => availableSheets.First(s => s.Id == l.SheetId).Name == g.Key)
//                })
//                .Where(s => s.Count > 0);

//            var itemY = 25;
//            foreach (var material in materialStats)
//            {
//                var label = new Label
//                {
//                    Text = $"{material.Name}: {material.Count} 块",
//                    Font = new Font("Microsoft YaHei UI", 9F),
//                    Location = new Point(15, itemY),
//                    Size = new Size(250, 20),
//                    ForeColor = Color.FromArgb(52, 73, 94)
//                };
//                groupBox.Controls.Add(label);
//                itemY += 25;
//            }

//            controls.Add(groupBox);
//            return controls;
//        }

//        private void UpdateStatistics()
//        {
//            if (selectedSheetIndex >= optimizationResult.Layouts.Count) return;

//            var layout = optimizationResult.Layouts[selectedSheetIndex];

//            // 直接使用控件引用更新统计信息
//            if (utilizationLabel != null)
//                utilizationLabel.Text = $"利用率: {layout.UtilizationRate:P2}";

//            if (wasteLabel != null)
//                wasteLabel.Text = $"浪费面积: {layout.WasteArea:F2} mm²";

//            if (pieceCountLabel != null)
//                pieceCountLabel.Text = $"玻璃片数量: {layout.Placements.Count} 片";

//            if (efficiencyBar != null)
//                efficiencyBar.Value = Math.Min(100, Math.Max(0, (int)(layout.UtilizationRate * 100)));
//        }

//        private OptimizationResult OptimizeLayout()
//        {
//            var result = new OptimizationResult
//            {
//                Layouts = new List<SheetLayout>(),
//                OptimizationTime = 1.23,
//                TotalCuts = 0
//            };

//            // 展开所有玻璃片
//            var allPieces = new List<GlassPiece>();
//            foreach (var piece in orderPieces)
//            {
//                for (int i = 0; i < piece.Quantity; i++)
//                {
//                    allPieces.Add(new GlassPiece
//                    {
//                        Id = piece.Id + i * 1000, // 确保每个实例有唯一ID
//                        Width = piece.Width,
//                        Height = piece.Height,
//                        Name = piece.Name,
//                        OrderId = piece.OrderId,
//                        Thickness = piece.Thickness,
//                        Quantity = 1
//                    });
//                }
//            }

//            // 按面积排序（大的优先）
//            allPieces = allPieces.OrderByDescending(p => p.Width * p.Height).ToList();

//            var sheetId = 1;
//            var pieceIndex = 0;

//            while (pieceIndex < allPieces.Count)
//            {
//                // 选择合适的板材类型
//                var selectedSheet = availableSheets[sheetId % availableSheets.Count];

//                var layout = new SheetLayout
//                {
//                    SheetId = selectedSheet.Id,
//                    Placements = new List<PiecePlacement>()
//                };

//                // 使用改进的装箱算法
//                var bins = new List<Bin> { new Bin { X = 0, Y = 0, Width = selectedSheet.Width, Height = selectedSheet.Height } };

//                while (pieceIndex < allPieces.Count && bins.Any())
//                {
//                    var piece = allPieces[pieceIndex];
//                    var bestBin = bins
//                        .Where(b => b.Width >= piece.Width && b.Height >= piece.Height)
//                        .OrderBy(b => b.Width * b.Height) // 选择最小的合适bin
//                        .FirstOrDefault();

//                    if (bestBin == null)
//                    {
//                        // 尝试旋转
//                        bestBin = bins
//                            .Where(b => b.Width >= piece.Height && b.Height >= piece.Width)
//                            .OrderBy(b => b.Width * b.Height)
//                            .FirstOrDefault();

//                        if (bestBin != null)
//                        {
//                            // 旋转玻璃片
//                            var temp = piece.Width;
//                            piece.Width = piece.Height;
//                            piece.Height = temp;
//                        }
//                    }

//                    if (bestBin == null) break; // 当前板材无法放置更多玻璃片

//                    // 放置玻璃片
//                    layout.Placements.Add(new PiecePlacement
//                    {
//                        PieceId = piece.Id,
//                        PieceName = piece.Name,
//                        OrderId = piece.OrderId,
//                        X = bestBin.X,
//                        Y = bestBin.Y,
//                        Width = piece.Width,
//                        Height = piece.Height
//                    });

//                    // 分割bin
//                    bins.Remove(bestBin);

//                    // 右侧剩余空间
//                    if (bestBin.Width > piece.Width)
//                    {
//                        bins.Add(new Bin
//                        {
//                            X = bestBin.X + piece.Width,
//                            Y = bestBin.Y,
//                            Width = bestBin.Width - piece.Width,
//                            Height = piece.Height
//                        });
//                    }

//                    // 上方剩余空间
//                    if (bestBin.Height > piece.Height)
//                    {
//                        bins.Add(new Bin
//                        {
//                            X = bestBin.X,
//                            Y = bestBin.Y + piece.Height,
//                            Width = bestBin.Width,
//                            Height = bestBin.Height - piece.Height
//                        });
//                    }

//                    pieceIndex++;
//                    result.TotalCuts += 2; // 每个玻璃片需要2次切割
//                }

//                if (layout.Placements.Any())
//                {
//                    // 计算利用率
//                    var usedArea = layout.Placements.Sum(p => p.Width * p.Height);
//                    var totalArea = selectedSheet.Width * selectedSheet.Height;
//                    layout.UtilizationRate = usedArea / totalArea;
//                    layout.WasteArea = totalArea - usedArea;

//                    result.Layouts.Add(layout);
//                }

//                sheetId++;
//            }

//            return result;
//        }

//        // 装箱算法辅助类
//        private class Bin
//        {
//            public float X { get; set; }
//            public float Y { get; set; }
//            public float Width { get; set; }
//            public float Height { get; set; }
//        }
//    }

//    // 数据模型类 (扩展)
//    public class GlassSheet
//    {
//        public int Id { get; set; }
//        public float Width { get; set; }
//        public float Height { get; set; }
//        public float Thickness { get; set; }
//        public string Name { get; set; }
//    }

//    public class GlassPiece
//    {
//        public int Id { get; set; }
//        public float Width { get; set; }
//        public float Height { get; set; }
//        public int Quantity { get; set; }
//        public string Name { get; set; }
//        public int OrderId { get; set; }
//        public float Thickness { get; set; }
//    }

//    public class OptimizationResult
//    {
//        public List<SheetLayout> Layouts { get; set; }
//        public int TotalCuts { get; set; }
//        public double OptimizationTime { get; set; }
//    }

//    public class SheetLayout
//    {
//        public int SheetId { get; set; }
//        public List<PiecePlacement> Placements { get; set; }
//        public double UtilizationRate { get; set; }
//        public double WasteArea { get; set; }
//    }

//    public class PiecePlacement
//    {
//        public int PieceId { get; set; }
//        public string PieceName { get; set; }
//        public int OrderId { get; set; }
//        public float X { get; set; }
//        public float Y { get; set; }
//        public float Width { get; set; }
//        public float Height { get; set; }
//    }
//}