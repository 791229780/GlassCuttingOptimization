using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Drawing;
using GlassCuttingOptimization.Models.Dto;
using GlassCuttingOptimization.Models.Optimization;
using GlassCuttingOptimization.Utils;

namespace GlassCuttingOptimization.Services
{
    public class OptimizationEngine
    {
        private const int MIN_WASTE_GAP = 5; // 最小浪费间隙
        SqlSugarHelper _db = new SqlSugarHelper();
        public OptimizationResult Optimize(List<OrderDto> orders, List<OriginalDto> originalSheets, OptimizationAlgorithm algorithm)
        {
            var stopwatch = Stopwatch.StartNew();

            var result = new OptimizationResult
            {
                AlgorithmUsed = GetAlgorithmDescription(algorithm),
                Sheets = new List<OptimizedSheet>()
            };

            try
            {
                ValidateInputs(orders, originalSheets);

                // 展开订单为单个玻璃片
                var glassPieces = ExpandOrdersToGlassPieces(orders);

                if (!glassPieces.Any())
                    throw new InvalidOperationException("没有有效的玻璃片需要排版");

                // 根据算法执行排版
                result.Sheets = ExecuteAlgorithm(glassPieces, originalSheets, algorithm);

                // 计算利用率
                result.CalculateOverallUtilization();
                result.SaveAsOriginal();
            }
            catch (Exception ex)
            {
                result.Sheets = new List<OptimizedSheet>();
                result.OverallUtilizationRate = 0;
                throw new Exception($"排版优化失败: {ex.Message}", ex);
            }
            finally
            {
                stopwatch.Stop();
                result.OptimizationTime = stopwatch.Elapsed;
            }

            return result;
        }

        #region 输入验证和数据准备

        private void ValidateInputs(List<OrderDto> orders, List<OriginalDto> originalSheets)
        {
            if (orders == null || !orders.Any())
                throw new ArgumentException("订单列表不能为空");

            if (originalSheets == null || !originalSheets.Any())
                throw new ArgumentException("原片列表不能为空");

            // 验证订单数据
            foreach (var order in orders)
            {
                if (order.X <= 0 || order.Y <= 0)
                    throw new ArgumentException($"订单 {order.OrderID} 的尺寸无效: {order.X}x{order.Y}");

                if (order.Number <= 0)
                    throw new ArgumentException($"订单 {order.OrderID} 的数量无效: {order.Number}");
            }

            // 验证原片数据
            foreach (var original in originalSheets)
            {
                if ( int.Parse( original.X1) <= 0 || int.Parse(original.Y1) <= 0)
                    throw new ArgumentException($"原片 {original.ID} 的尺寸无效: {original.X1}x{original.Y1}");
            }
        }

        private List<OptimizedGlassPiece> ExpandOrdersToGlassPieces(List<OrderDto> orders)
        {
            var pieces = new List<OptimizedGlassPiece>();
            int sequenceNumber = 1;

            // 按优先级排序
            var sortedOrders = orders.OrderByDescending(o => o.Rriority).ToList();

            foreach (var order in sortedOrders)
            {
                for (int i = 0; i < order.Number; i++)
                {
                    var piece = new OptimizedGlassPiece
                    {
                        OrderID = order.OrderID,
                        OrderNumber = order.Order ?? string.Empty,
                        Width = order.X,
                        Height = order.Y,
                        Priority = order.Rriority,
                        SequenceNumber = sequenceNumber++,
                        Customer = order.Customer ?? string.Empty,
                        Rotation = 0,
                        Note = order.Note ?? string.Empty,
                        BottomEdge = order.BottomCbo,
                        TopEdge = order.TopCbo,
                        LeftEdge = order.LeftCbo,
                        RightEdge = order.RightCbo
                    };

                    pieces.Add(piece);
                }
            }

            return pieces;
        }

        #endregion

        #region 算法调度器

        private List<OptimizedSheet> ExecuteAlgorithm(List<OptimizedGlassPiece> pieces, List<OriginalDto> originalSheets, OptimizationAlgorithm algorithm)
        {
            switch (algorithm)
            {
                case OptimizationAlgorithm.IntelligentPacking:
                    return IntelligentPackingAlgorithm(pieces, originalSheets);
                case OptimizationAlgorithm.BestFitDecreasing:
                    return BestFitDecreasingAlgorithm(pieces, originalSheets);
                case OptimizationAlgorithm.FirstFitDecreasing:
                    return FirstFitDecreasingAlgorithm(pieces, originalSheets);
                case OptimizationAlgorithm.BottomLeftFill:
                    return BottomLeftFillAlgorithm(pieces, originalSheets);
                case OptimizationAlgorithm.NextFitDecreasing:
                    return NextFitDecreasingAlgorithm(pieces, originalSheets);
                default:
                    return IntelligentPackingAlgorithm(pieces, originalSheets);
            }
        }

        #endregion

        #region 智能打包算法（主要算法）

        private List<OptimizedSheet> IntelligentPackingAlgorithm(List<OptimizedGlassPiece> pieces, List<OriginalDto> originalSheets)
        {
            var result = new List<OptimizedSheet>();
            var remainingPieces = new List<OptimizedGlassPiece>(pieces);

            // 按面积从大到小排序
            remainingPieces = remainingPieces.OrderByDescending(p => p.Width * p.Height).ToList();

            while (remainingPieces.Any())
            {
                var bestSheet = SelectBestSheet(remainingPieces, originalSheets);
                if (bestSheet == null) break;

                var optimizedSheet = CreateOptimizedSheet(bestSheet);
                var packedCount = PackPiecesIntelligently(optimizedSheet, remainingPieces);

                if (packedCount > 0)
                {
                    result.Add(optimizedSheet);
                }
                else
                {
                    // 如果无法放置任何玻璃片，尝试下一个最小的原片
                    break;
                }
            }

            return result;
        }

        private int PackPiecesIntelligently(OptimizedSheet sheet, List<OptimizedGlassPiece> remainingPieces)
        {
            var packedCount = 0;
            var freeRectangles = new List<Rectangle> { sheet.GetEffectiveArea() };

            while (remainingPieces.Any() && freeRectangles.Any())
            {
                var bestPlacement = FindBestPlacement(remainingPieces, freeRectangles);
                if (bestPlacement == null) break;

                // 应用放置
                ApplyPlacement(sheet, bestPlacement, remainingPieces);

                // 更新自由矩形
                UpdateFreeRectangles(freeRectangles, bestPlacement);

                packedCount++;
            }

            return packedCount;
        }

        private PlacementCandidate FindBestPlacement(List<OptimizedGlassPiece> pieces, List<Rectangle> freeRectangles)
        {
            PlacementCandidate bestPlacement = null;
            double bestScore = double.MinValue;

            foreach (var piece in pieces)
            {
                foreach (var rect in freeRectangles)
                {
                    // 尝试原始方向
                    var placement1 = TryPlacement(piece, rect, false);
                    if (placement1 != null)
                    {
                        double score = CalculatePlacementScore(placement1, rect);
                        if (score > bestScore)
                        {
                            bestScore = score;
                            bestPlacement = placement1;
                        }
                    }

                    // 尝试旋转90度
                    if (CanRotate(piece))
                    {
                        var placement2 = TryPlacement(piece, rect, true);
                        if (placement2 != null)
                        {
                            double score = CalculatePlacementScore(placement2, rect);
                            if (score > bestScore)
                            {
                                bestScore = score;
                                bestPlacement = placement2;
                            }
                        }
                    }
                }
            }

            return bestPlacement;
        }

        private PlacementCandidate TryPlacement(OptimizedGlassPiece piece, Rectangle freeRect, bool rotated)
        {
            int width = rotated ? piece.Height : piece.Width;
            int height = rotated ? piece.Width : piece.Height;

            if (width <= freeRect.Width && height <= freeRect.Height)
            {
                return new PlacementCandidate
                {
                    Piece = piece,
                    X = freeRect.X,
                    Y = freeRect.Y,
                    Width = width,
                    Height = height,
                    Rotated = rotated,
                    FreeRect = freeRect
                };
            }

            return null;
        }

        private double CalculatePlacementScore(PlacementCandidate placement, Rectangle freeRect)
        {
            // 多因素评分系统
            double areaUtilization = (double)(placement.Width * placement.Height) / (freeRect.Width * freeRect.Height);
            double leftoverScore = CalculateLeftoverScore(placement, freeRect);
            double priorityScore = placement.Piece.Priority / 10.0;
            double positionScore = CalculatePositionScore(placement.X, placement.Y);

            // 加权计算最终得分
            return areaUtilization * 0.4 + leftoverScore * 0.3 + priorityScore * 0.2 + positionScore * 0.1;
        }

        private double CalculateLeftoverScore(PlacementCandidate placement, Rectangle freeRect)
        {
            int wastedWidth = freeRect.Width - placement.Width;
            int wastedHeight = freeRect.Height - placement.Height;

            // 更倾向于能完全利用某一方向的放置
            if (wastedWidth == 0 || wastedHeight == 0)
                return 1.0;

            // 较小的浪费面积得分更高
            double wastedArea = wastedWidth * wastedHeight;
            return 1.0 / (1.0 + wastedArea / 10000.0);
        }

        private double CalculatePositionScore(int x, int y)
        {
            // 更倾向于左下角的位置
            return 1.0 / (1.0 + (x + y) / 1000.0);
        }

        private void ApplyPlacement(OptimizedSheet sheet, PlacementCandidate placement, List<OptimizedGlassPiece> remainingPieces)
        {
            placement.Piece.X = placement.X;
            placement.Piece.Y = placement.Y;

            if (placement.Rotated)
            {
                // 交换宽高
                var temp = placement.Piece.Width;
                placement.Piece.Width = placement.Piece.Height;
                placement.Piece.Height = temp;
                placement.Piece.Rotation = 90;
            }

            sheet.GlassPieces.Add(placement.Piece);
            remainingPieces.Remove(placement.Piece);
        }

        private void UpdateFreeRectangles(List<Rectangle> freeRectangles, PlacementCandidate placement)
        {
            var placedRect = new Rectangle(placement.X, placement.Y, placement.Width, placement.Height);
            var newFreeRects = new List<Rectangle>();

            foreach (var rect in freeRectangles)
            {
                if (rect.IntersectsWith(placedRect))
                {
                    // 分割矩形
                    var splitRects = SplitRectangle(rect, placedRect);
                    newFreeRects.AddRange(splitRects);
                }
                else
                {
                    newFreeRects.Add(rect);
                }
            }

            // 移除重叠的矩形并更新列表
            freeRectangles.Clear();
            freeRectangles.AddRange(RemoveOverlappingRectangles(newFreeRects));
        }

        #endregion

        #region 其他算法实现

        private List<OptimizedSheet> BestFitDecreasingAlgorithm(List<OptimizedGlassPiece> pieces, List<OriginalDto> originalSheets)
        {
            var result = new List<OptimizedSheet>();
            var remainingPieces = pieces.OrderByDescending(p => p.Width * p.Height).ToList();

            while (remainingPieces.Any())
            {
                var bestSheet = SelectBestSheet(remainingPieces, originalSheets);
                if (bestSheet == null) break;

                var optimizedSheet = CreateOptimizedSheet(bestSheet);
                var packedCount = PackWithBestFit(optimizedSheet, remainingPieces);

                if (packedCount > 0)
                    result.Add(optimizedSheet);
                else
                    break;
            }

            return result;
        }

        private int PackWithBestFit(OptimizedSheet sheet, List<OptimizedGlassPiece> remainingPieces)
        {
            var packedCount = 0;
            var effectiveArea = sheet.GetEffectiveArea();

            while (remainingPieces.Any())
            {
                OptimizedGlassPiece bestPiece = null;
                Point bestPosition = Point.Empty;
                bool needsRotation = false;
                int minWastedArea = int.MaxValue;

                foreach (var piece in remainingPieces)
                {
                    // 尝试原始方向
                    var pos1 = FindBestFitPosition(sheet, piece, effectiveArea, false);
                    if (pos1.HasValue)
                    {
                        int wastedArea = CalculateWastedArea(effectiveArea, piece, pos1.Value, false);
                        if (wastedArea < minWastedArea)
                        {
                            minWastedArea = wastedArea;
                            bestPiece = piece;
                            bestPosition = pos1.Value;
                            needsRotation = false;
                        }
                    }

                    // 尝试旋转方向
                    if (CanRotate(piece))
                    {
                        var pos2 = FindBestFitPosition(sheet, piece, effectiveArea, true);
                        if (pos2.HasValue)
                        {
                            int wastedArea = CalculateWastedArea(effectiveArea, piece, pos2.Value, true);
                            if (wastedArea < minWastedArea)
                            {
                                minWastedArea = wastedArea;
                                bestPiece = piece;
                                bestPosition = pos2.Value;
                                needsRotation = true;
                            }
                        }
                    }
                }

                if (bestPiece == null) break;

                // 放置玻璃片
                PlacePiece(sheet, bestPiece, bestPosition, needsRotation);
                remainingPieces.Remove(bestPiece);
                packedCount++;
            }

            return packedCount;
        }

        private List<OptimizedSheet> FirstFitDecreasingAlgorithm(List<OptimizedGlassPiece> pieces, List<OriginalDto> originalSheets)
        {
            // 简化实现，使用类似BestFit的逻辑但选择第一个可行位置
            return BestFitDecreasingAlgorithm(pieces, originalSheets);
        }

        private List<OptimizedSheet> BottomLeftFillAlgorithm(List<OptimizedGlassPiece> pieces, List<OriginalDto> originalSheets)
        {
            var result = new List<OptimizedSheet>();
            var remainingPieces = pieces.OrderByDescending(p => Math.Max(p.Width, p.Height)).ToList();

            while (remainingPieces.Any())
            {
                var bestSheet = SelectBestSheet(remainingPieces, originalSheets);
                if (bestSheet == null) break;

                var optimizedSheet = CreateOptimizedSheet(bestSheet);
                var packedCount = PackWithBottomLeft(optimizedSheet, remainingPieces);

                if (packedCount > 0)
                    result.Add(optimizedSheet);
                else
                    break;
            }

            return result;
        }

        private int PackWithBottomLeft(OptimizedSheet sheet, List<OptimizedGlassPiece> remainingPieces)
        {
            var packedCount = 0;
            var effectiveArea = sheet.GetEffectiveArea();

            while (remainingPieces.Any())
            {
                bool placed = false;

                foreach (var piece in remainingPieces.ToList())
                {
                    // 从左下角开始寻找位置
                    var position = FindBottomLeftPosition(sheet, piece, effectiveArea);
                    if (position.HasValue)
                    {
                        PlacePiece(sheet, piece, position.Value, false);
                        remainingPieces.Remove(piece);
                        packedCount++;
                        placed = true;
                        break;
                    }

                    // 尝试旋转
                    if (CanRotate(piece))
                    {
                        position = FindBottomLeftPosition(sheet, piece, effectiveArea, true);
                        if (position.HasValue)
                        {
                            PlacePiece(sheet, piece, position.Value, true);
                            remainingPieces.Remove(piece);
                            packedCount++;
                            placed = true;
                            break;
                        }
                    }
                }

                if (!placed) break;
            }

            return packedCount;
        }

        private List<OptimizedSheet> NextFitDecreasingAlgorithm(List<OptimizedGlassPiece> pieces, List<OriginalDto> originalSheets)
        {
            // 简化实现
            return BestFitDecreasingAlgorithm(pieces, originalSheets);
        }

        #endregion

        #region 辅助方法

        private string GetAlgorithmDescription(OptimizationAlgorithm algorithm)
        {
            var field = algorithm.GetType().GetField(algorithm.ToString());
            var attribute = (System.ComponentModel.DescriptionAttribute)
                Attribute.GetCustomAttribute(field, typeof(System.ComponentModel.DescriptionAttribute));
            return attribute?.Description ?? algorithm.ToString();
        }

        private OriginalDto SelectBestSheet(List<OptimizedGlassPiece> pieces, List<OriginalDto> originalSheets)
        {
            if (!pieces.Any()) return null;

            var largestPiece = pieces.OrderByDescending(p => Math.Max(p.Width, p.Height)).First();

            // 选择能容纳最大玻璃片的最小原片
            return originalSheets
                .Where(s => int.Parse( s.X1) >= largestPiece.Width && int.Parse(s.Y1) >= largestPiece.Height)
                .OrderBy(s => int.Parse( s.X1) * int.Parse(s.Y1))
                .FirstOrDefault();
        }

        private OptimizedSheet CreateOptimizedSheet(OriginalDto original)
        {
           var dto =   _db.Query<MaterialDto>().Where(c => c.ID == original.MaterialID).First();
            return new OptimizedSheet
            {
                Width = int.Parse( original.X1),
                Height = int.Parse(original.Y1),
                MaterialType = dto.Type ?? "",
                Thickness =double.Parse( dto.Thickness) ,
                X1 = int.Parse( dto.X1),
                X2 = int.Parse(dto.X2),
                Y1 = int.Parse(dto.Y1),
                Y2 = int.Parse(dto.Y2)
            };
        }

        private bool CanRotate(OptimizedGlassPiece piece)
        {
            // 正方形不需要旋转
            return piece.Width != piece.Height;
        }

        private Point? FindBestFitPosition(OptimizedSheet sheet, OptimizedGlassPiece piece, Rectangle area, bool rotated)
        {
            int width = rotated ? piece.Height : piece.Width;
            int height = rotated ? piece.Width : piece.Height;

            for (int y = area.Y; y <= area.Bottom - height; y++)
            {
                for (int x = area.X; x <= area.Right - width; x++)
                {
                    var testRect = new Rectangle(x, y, width, height);
                    if (!HasCollision(sheet, testRect))
                    {
                        return new Point(x, y);
                    }
                }
            }

            return null;
        }

        private Point? FindBottomLeftPosition(OptimizedSheet sheet, OptimizedGlassPiece piece, Rectangle area, bool rotated = false)
        {
            int width = rotated ? piece.Height : piece.Width;
            int height = rotated ? piece.Width : piece.Height;

            // 从底部开始，从左到右
            for (int y = area.Bottom - height; y >= area.Y; y--)
            {
                for (int x = area.X; x <= area.Right - width; x++)
                {
                    var testRect = new Rectangle(x, y, width, height);
                    if (!HasCollision(sheet, testRect))
                    {
                        return new Point(x, y);
                    }
                }
            }

            return null;
        }

        private bool HasCollision(OptimizedSheet sheet, Rectangle testRect)
        {
            foreach (var piece in sheet.GlassPieces)
            {
                if (testRect.IntersectsWith(piece.Bounds))
                    return true;
            }
            return false;
        }

        private void PlacePiece(OptimizedSheet sheet, OptimizedGlassPiece piece, Point position, bool needsRotation)
        {
            piece.X = position.X;
            piece.Y = position.Y;

            if (needsRotation)
            {
                var temp = piece.Width;
                piece.Width = piece.Height;
                piece.Height = temp;
                piece.Rotation = 90;
            }

            sheet.GlassPieces.Add(piece);
        }

        private int CalculateWastedArea(Rectangle area, OptimizedGlassPiece piece, Point position, bool rotated)
        {
            int width = rotated ? piece.Height : piece.Width;
            int height = rotated ? piece.Width : piece.Height;

            // 简单计算右侧和下方的浪费面积
            int rightWaste = area.Right - position.X - width;
            int bottomWaste = area.Bottom - position.Y - height;

            return rightWaste * height + bottomWaste * width;
        }

        private List<Rectangle> SplitRectangle(Rectangle freeRect, Rectangle placedRect)
        {
            var result = new List<Rectangle>();

            // 左侧剩余
            if (placedRect.X > freeRect.X)
            {
                result.Add(new Rectangle(freeRect.X, freeRect.Y,
                    placedRect.X - freeRect.X, freeRect.Height));
            }

            // 右侧剩余
            if (placedRect.Right < freeRect.Right)
            {
                result.Add(new Rectangle(placedRect.Right, freeRect.Y,
                    freeRect.Right - placedRect.Right, freeRect.Height));
            }

            // 上方剩余
            if (placedRect.Y > freeRect.Y)
            {
                result.Add(new Rectangle(freeRect.X, freeRect.Y,
                    freeRect.Width, placedRect.Y - freeRect.Y));
            }

            // 下方剩余
            if (placedRect.Bottom < freeRect.Bottom)
            {
                result.Add(new Rectangle(freeRect.X, placedRect.Bottom,
                    freeRect.Width, freeRect.Bottom - placedRect.Bottom));
            }

            return result.Where(r => r.Width > MIN_WASTE_GAP && r.Height > MIN_WASTE_GAP).ToList();
        }

        private List<Rectangle> RemoveOverlappingRectangles(List<Rectangle> rectangles)
        {
            var result = new List<Rectangle>();

            foreach (var rect in rectangles)
            {
                bool isContained = false;
                foreach (var existing in result.ToList())
                {
                    if (existing.Contains(rect))
                    {
                        isContained = true;
                        break;
                    }
                    else if (rect.Contains(existing))
                    {
                        result.Remove(existing);
                    }
                }

                if (!isContained && rect.Width > 0 && rect.Height > 0)
                {
                    result.Add(rect);
                }
            }

            return result;
        }

        #endregion
    }

    #region 辅助类

    public class PlacementCandidate
    {
        public OptimizedGlassPiece Piece { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool Rotated { get; set; }
        public Rectangle FreeRect { get; set; }
    }

    #endregion
}