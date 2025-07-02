using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GlassCuttingOptimization.Models.Optimization;
using System.Drawing.Drawing2D;

namespace GlassCuttingOptimization.Views.Controls
{
    public partial class OptimizationVisualizationControl : UserControl
    {
        private OptimizationResult _optimizationResult;
        private int _currentSheetIndex = 0;
        private OptimizedGlassPiece _selectedPiece;
        private bool _isDragging = false;
        private Point _dragOffset;
        private Point _dragStartPosition;

        // 绘制参数 - 调整边距让板材显示更大
        private const int MARGIN = 30; // 减小边距，让板材更大
        private float _scale = 1.0f;
        private Point _panOffset = Point.Empty;
        private bool _autoFit = true;

        // 颜色定义
        private readonly Color SheetColor = Color.FromArgb(240, 240, 240);
        private readonly Color GlassColor = Color.Gray;
        private readonly Color SelectionColor = Color.Blue;

        // 增大固定字体大小
        private readonly Font FixedTextFont = new Font("Arial", 11, FontStyle.Bold); // 从9增加到11
        private readonly Font FixedLargeTextFont = new Font("Arial", 13, FontStyle.Bold); // 从11增加到13
        private readonly Font FixedSmallTextFont = new Font("Arial", 9, FontStyle.Regular); // 从8增加到9

        public OptimizationVisualizationControl()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.DoubleBuffer |
                     ControlStyles.ResizeRedraw, true);
        }

        #region 公共方法

        public void SetOptimizationResult(OptimizationResult result)
        {
            _optimizationResult = result;
            _currentSheetIndex = 0;
            _selectedPiece = null;
            CalculateOptimalScale();
            Invalidate();
        }

        public void ShowSheet(int sheetIndex)
        {
            if (_optimizationResult?.Sheets != null &&
                sheetIndex >= 0 &&
                sheetIndex < _optimizationResult.Sheets.Count)
            {
                _currentSheetIndex = sheetIndex;
                _selectedPiece = null;
                CalculateOptimalScale();
                Invalidate();
            }
        }

        public void RotateSelectedPiece()
        {
            if (_selectedPiece == null) return;

            var currentSheet = GetCurrentSheet();
            if (currentSheet == null) return;

            // 计算旋转后的尺寸
            int newWidth = _selectedPiece.Height;
            int newHeight = _selectedPiece.Width;
            int newRotation = (_selectedPiece.Rotation + 90) % 360;

            // 检查旋转后是否有足够空间
            var testBounds = new Rectangle(_selectedPiece.X, _selectedPiece.Y, newWidth, newHeight);

            if (CanPlacePiece(testBounds, currentSheet, _selectedPiece))
            {
                _selectedPiece.Width = newWidth;
                _selectedPiece.Height = newHeight;
                _selectedPiece.Rotation = newRotation;
                Invalidate();
            }
        }

        public void FitToWindow()
        {
            _autoFit = true;
            CalculateOptimalScale();
            Invalidate();
        }

        public void ZoomIn()
        {
            _autoFit = false;
            _scale = Math.Min(_scale * 1.2f, 5.0f);
            Invalidate();
        }

        public void ZoomOut()
        {
            _autoFit = false;
            _scale = Math.Max(_scale / 1.2f, 0.1f);
            Invalidate();
        }

        public void SetZoomLevel(float zoomLevel)
        {
            _autoFit = false;
            _scale = Math.Max(0.1f, Math.Min(zoomLevel, 5.0f));
            Invalidate();
        }

        public float GetZoomLevel()
        {
            return _scale;
        }

        #endregion

        /// <summary>
        /// 计算最优缩放比例 - 调整为显示更大的板材
        /// </summary>
        private void CalculateOptimalScale()
        {
            var currentSheet = GetCurrentSheet();
            if (currentSheet == null)
            {
                _scale = 1.0f;
                return;
            }

            // 可用绘制区域 - 减少预留空间，让板材更大
            int availableWidth = Width - (MARGIN * 2);
            int availableHeight = Height - (MARGIN * 2) - 60; // 减少顶部预留空间从100到60

            if (availableWidth <= 0 || availableHeight <= 0)
            {
                _scale = 0.5f;
                return;
            }

            // 计算缩放比例
            float scaleX = (float)availableWidth / currentSheet.Width;
            float scaleY = (float)availableHeight / currentSheet.Height;

            // 取较小的比例，确保完整显示
            _scale = Math.Min(scaleX, scaleY);

            // 调整最小缩放比例，让板材显示更大
            _scale = Math.Max(0.2f, Math.Min(_scale, 8.0f)); // 提高最大缩放比例从5.0到8.0

            // 计算居中偏移
            float scaledSheetWidth = currentSheet.Width * _scale;
            float scaledSheetHeight = currentSheet.Height * _scale;

            _panOffset.X = (int)((availableWidth - scaledSheetWidth) / 2);
            _panOffset.Y = (int)((availableHeight - scaledSheetHeight) / 2) + 30; // 减少顶部偏移从50到30
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (_autoFit)
            {
                CalculateOptimalScale();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (_optimizationResult?.Sheets == null || !_optimizationResult.Sheets.Any())
            {
                DrawEmptyState(e.Graphics);
                return;
            }

            var currentSheet = GetCurrentSheet();
            if (currentSheet == null) return;

            // 设置图形质量
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            // 绘制顶部信息
            DrawTopInformation(e.Graphics, currentSheet);

            // 保存原始变换
            var originalTransform = e.Graphics.Transform.Clone();

            // 应用缩放和平移
            e.Graphics.TranslateTransform(MARGIN + _panOffset.X, MARGIN + _panOffset.Y);
            e.Graphics.ScaleTransform(_scale, _scale);

            // 绘制板材
            DrawSheet(e.Graphics, currentSheet);

            // 绘制玻璃片
            foreach (var piece in currentSheet.GlassPieces)
            {
                DrawGlassPiece(e.Graphics, piece);
            }

            // 绘制选中状态
            if (_selectedPiece != null)
            {
                DrawSelection(e.Graphics, _selectedPiece);
            }

            // 恢复原始变换
            e.Graphics.Transform = originalTransform;
        }

        private void DrawTopInformation(Graphics g, OptimizedSheet sheet)
        {
            // 背景 - 减少高度
            using (var brush = new SolidBrush(Color.FromArgb(250, 250, 250)))
            {
                g.FillRectangle(brush, 0, 0, Width, 35); // 从45减少到35
            }

            // 绘制信息
            string sheetInfo = $"板材 {_currentSheetIndex + 1}/{_optimizationResult.Sheets.Count} - 尺寸: {sheet.Width}×{sheet.Height}mm";
            string utilizationInfo = $"利用率: {sheet.UtilizationRate:F1}% | 缩放: {_scale * 100:F0}% | 玻璃片: {sheet.GlassPieces.Count}";

            using (var titleFont = new Font("微软雅黑", 10, FontStyle.Bold)) // 稍微增大字体
            using (var infoFont = new Font("微软雅黑", 9, FontStyle.Regular))
            {
                g.DrawString(sheetInfo, titleFont, Brushes.Black, 10, 3);
                g.DrawString(utilizationInfo, infoFont, Brushes.Blue, 10, 20);
            }

            // 分割线
            using (var pen = new Pen(Color.LightGray))
            {
                g.DrawLine(pen, 0, 35, Width, 35); // 调整位置
            }
        }

        private void DrawEmptyState(Graphics g)
        {
            string message = "暂无排版结果\n请先进行排版优化";
            var messageSize = g.MeasureString(message, FixedLargeTextFont);
            var messagePoint = new PointF(
                (Width - messageSize.Width) / 2,
                (Height - messageSize.Height) / 2
            );

            g.DrawString(message, FixedLargeTextFont, Brushes.Gray, messagePoint);
        }

        private void DrawSheet(Graphics g, OptimizedSheet sheet)
        {
            var sheetRect = new Rectangle(0, 0, sheet.Width, sheet.Height);

            // 绘制板材背景（带纹理效果）
            using (var textureBrush = CreateTextureBrush())
            {
                g.FillRectangle(textureBrush, sheetRect);
            }

            // 绘制板材边框 - 稍微加粗
            using (var pen = new Pen(Color.Black, 3 / _scale)) // 从2增加到3
            {
                g.DrawRectangle(pen, sheetRect);
            }
        }

        private void DrawGlassPiece(Graphics g, OptimizedGlassPiece piece)
        {
            var pieceRect = new Rectangle(piece.X, piece.Y, piece.Width, piece.Height);

            // 绘制玻璃片背景
            using (var brush = new SolidBrush(GlassColor))
            {
                g.FillRectangle(brush, pieceRect);
            }

            // 绘制边框 - 只有黑色细线
            using (var pen = new Pen(Color.Black, 1.5f / _scale)) // 稍微加粗从1到1.5
            {
                g.DrawRectangle(pen, pieceRect);
            }

            // 绘制文字信息 - 使用固定大小字体
            DrawGlassPieceText(g, piece, pieceRect);
        }

        /// <summary>
        /// 绘制玻璃片文字 - 尺寸显示在右下角
        /// </summary>
        private void DrawGlassPieceText(Graphics g, OptimizedGlassPiece piece, Rectangle bounds)
        {
            // 计算屏幕上的实际大小来判断是否显示文字
            var scaledWidth = bounds.Width * _scale;
            var scaledHeight = bounds.Height * _scale;

            // 尺寸信息（右下角） - 更大的字体，更低的显示阈值
            if (scaledWidth > 50 && scaledHeight > 35) // 降低显示阈值
            {
                string sizeText = $"{piece.Width}×{piece.Height}";
                var sizeTextSize = g.MeasureString(sizeText, FixedTextFont);

                // 计算右下角位置
                var sizePoint = new PointF(
                    bounds.Right - sizeTextSize.Width / _scale - 5 / _scale, // 右下角，留5像素边距
                    bounds.Bottom - sizeTextSize.Height / _scale - 3 / _scale  // 底部，留3像素边距
                );

                // 保存当前变换
                var savedTransform = g.Transform.Clone();

                // 重置缩放来绘制固定大小文字
                g.ResetTransform();
                g.TranslateTransform(MARGIN + _panOffset.X, MARGIN + _panOffset.Y);

                var screenPoint = new PointF(
                    sizePoint.X * _scale,
                    sizePoint.Y * _scale
                );

                // 添加半透明背景 - 更大的背景
                using (var bgBrush = new SolidBrush(Color.FromArgb(220, Color.White)))
                {
                    var bgRect = new RectangleF(
                        screenPoint.X - 4,
                        screenPoint.Y - 2,
                        sizeTextSize.Width + 8,
                        sizeTextSize.Height + 4
                    );
                    g.FillRectangle(bgBrush, bgRect);

                    // 添加边框
                    using (var borderPen = new Pen(Color.Gray, 1))
                    {
                        g.DrawRectangle(borderPen, bgRect.X, bgRect.Y, bgRect.Width, bgRect.Height);
                    }
                }

                g.DrawString(sizeText, FixedTextFont, Brushes.Black, screenPoint);

                // 恢复变换
                g.Transform = savedTransform;
            }

            // 序号（右上角）
            if (scaledWidth > 25)
            {
                string sequenceText = piece.SequenceNumber.ToString();
                var sequenceSize = g.MeasureString(sequenceText, FixedSmallTextFont);

                // 保存当前变换
                var savedTransform = g.Transform.Clone();

                // 重置缩放来绘制固定大小文字
                g.ResetTransform();
                g.TranslateTransform(MARGIN + _panOffset.X, MARGIN + _panOffset.Y);

                var screenPoint = new PointF(
                    (bounds.Right - 18 / _scale) * _scale,
                    (bounds.Y + 3 / _scale) * _scale
                );

                using (var bgBrush = new SolidBrush(Color.FromArgb(220, Color.Yellow)))
                {
                    var bgRect = new RectangleF(
                        screenPoint.X - 3, screenPoint.Y - 1,
                        sequenceSize.Width + 6, sequenceSize.Height + 2
                    );
                    g.FillRectangle(bgBrush, bgRect);
                }

                g.DrawString(sequenceText, FixedSmallTextFont, Brushes.Red, screenPoint);

                // 恢复变换
                g.Transform = savedTransform;
            }

            // 订单编号（左上角）
            if (scaledWidth > 70 && !string.IsNullOrEmpty(piece.OrderNumber))
            {
                string orderText = piece.OrderNumber;

                // 保存当前变换
                var savedTransform = g.Transform.Clone();

                // 重置缩放来绘制固定大小文字
                g.ResetTransform();
                g.TranslateTransform(MARGIN + _panOffset.X, MARGIN + _panOffset.Y);

                var screenPoint = new PointF(
                    (bounds.X + 3 / _scale) * _scale,
                    (bounds.Y + 3 / _scale) * _scale
                );

                var orderTextSize = g.MeasureString(orderText, FixedSmallTextFont);

                using (var bgBrush = new SolidBrush(Color.FromArgb(220, Color.LightBlue)))
                {
                    var bgRect = new RectangleF(
                        screenPoint.X - 2, screenPoint.Y - 1,
                        orderTextSize.Width + 4, orderTextSize.Height + 2
                    );
                    g.FillRectangle(bgBrush, bgRect);
                }

                g.DrawString(orderText, FixedSmallTextFont, Brushes.DarkBlue, screenPoint);

                // 恢复变换
                g.Transform = savedTransform;
            }
        }

        private void DrawSelection(Graphics g, OptimizedGlassPiece piece)
        {
            var selectionRect = new Rectangle(
                piece.X - 3, // 稍微扩大选中框
                piece.Y - 3,
                piece.Width + 6,
                piece.Height + 6
            );

            using (var pen = new Pen(SelectionColor, 4 / _scale)) // 加粗选中线
            {
                pen.DashStyle = DashStyle.Dash;
                g.DrawRectangle(pen, selectionRect);
            }
        }

        private Brush CreateTextureBrush()
        {
            // 创建类似edit-way的纹理效果 - 更细腻的纹理
            var bitmap = new Bitmap(30, 30); // 增大纹理尺寸
            using (var g = Graphics.FromImage(bitmap))
            {
                g.Clear(SheetColor);
                using (var pen = new Pen(Color.FromArgb(25, Color.Gray))) // 更淡的纹理
                {
                    // 创建细密的网格纹理
                    for (int i = 0; i < 30; i += 6) // 调整网格密度
                    {
                        g.DrawLine(pen, i, 0, i, 30);
                        g.DrawLine(pen, 0, i, 30, i);
                    }

                    // 添加对角线纹理
                    using (var diagPen = new Pen(Color.FromArgb(15, Color.Gray)))
                    {
                        g.DrawLine(diagPen, 0, 0, 30, 30);
                        g.DrawLine(diagPen, 0, 15, 15, 30);
                        g.DrawLine(diagPen, 15, 0, 30, 15);
                    }
                }
            }
            return new TextureBrush(bitmap);
        }

        // 鼠标事件处理保持不变...
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button == MouseButtons.Left)
            {
                var clickedPiece = GetPieceAtPoint(e.Location);
                if (clickedPiece != null)
                {
                    _selectedPiece = clickedPiece;
                    _isDragging = true;

                    var boardPoint = ScreenToBoardCoordinates(e.Location);
                    _dragOffset = new Point(
                        (int)(boardPoint.X - clickedPiece.X),
                        (int)(boardPoint.Y - clickedPiece.Y)
                    );
                    _dragStartPosition = new Point(clickedPiece.X, clickedPiece.Y);
                    Cursor = Cursors.SizeAll;
                }
                else
                {
                    _selectedPiece = null;
                }
                Invalidate();
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (_isDragging && _selectedPiece != null)
            {
                var currentSheet = GetCurrentSheet();
                if (currentSheet == null) return;

                var boardPoint = ScreenToBoardCoordinates(e.Location);
                int newX = (int)(boardPoint.X - _dragOffset.X);
                int newY = (int)(boardPoint.Y - _dragOffset.Y);

                var testBounds = new Rectangle(newX, newY, _selectedPiece.Width, _selectedPiece.Height);

                if (CanPlacePiece(testBounds, currentSheet, _selectedPiece))
                {
                    _selectedPiece.X = newX;
                    _selectedPiece.Y = newY;
                    Cursor = Cursors.SizeAll;
                }
                else
                {
                    Cursor = Cursors.No;
                }

                Invalidate();
            }
            else
            {
                var hoveredPiece = GetPieceAtPoint(e.Location);
                Cursor = hoveredPiece != null ? Cursors.Hand : Cursors.Default;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (_isDragging)
            {
                _isDragging = false;
                Cursor = Cursors.Default;

                if (_selectedPiece != null)
                {
                    var currentSheet = GetCurrentSheet();
                    var testBounds = new Rectangle(_selectedPiece.X, _selectedPiece.Y, _selectedPiece.Width, _selectedPiece.Height);

                    if (!CanPlacePiece(testBounds, currentSheet, _selectedPiece))
                    {
                        _selectedPiece.X = _dragStartPosition.X;
                        _selectedPiece.Y = _dragStartPosition.Y;
                    }
                }

                Invalidate();
            }
        }

        private PointF ScreenToBoardCoordinates(Point screenPoint)
        {
            float x = (screenPoint.X - MARGIN - _panOffset.X) / _scale;
            float y = (screenPoint.Y - MARGIN - _panOffset.Y) / _scale;
            return new PointF(x, y);
        }

        private OptimizedGlassPiece GetPieceAtPoint(Point point)
        {
            var currentSheet = GetCurrentSheet();
            if (currentSheet == null) return null;

            var boardPoint = ScreenToBoardCoordinates(point);

            for (int i = currentSheet.GlassPieces.Count - 1; i >= 0; i--)
            {
                var piece = currentSheet.GlassPieces[i];
                var pieceRect = new RectangleF(piece.X, piece.Y, piece.Width, piece.Height);

                if (pieceRect.Contains(boardPoint))
                {
                    return piece;
                }
            }

            return null;
        }

        private bool CanPlacePiece(Rectangle bounds, OptimizedSheet sheet, OptimizedGlassPiece excludePiece)
        {
            if (bounds.X < 0 || bounds.Y < 0 ||
                bounds.Right > sheet.Width || bounds.Bottom > sheet.Height)
            {
                return false;
            }

            foreach (var piece in sheet.GlassPieces)
            {
                if (piece == excludePiece) continue;

                if (bounds.IntersectsWith(piece.Bounds))
                {
                    return false;
                }
            }

            return true;
        }

        private OptimizedSheet GetCurrentSheet()
        {
            if (_optimizationResult?.Sheets == null ||
                _currentSheetIndex < 0 ||
                _currentSheetIndex >= _optimizationResult.Sheets.Count)
            {
                return null;
            }

            return _optimizationResult.Sheets[_currentSheetIndex];
        }

     


    }
}

