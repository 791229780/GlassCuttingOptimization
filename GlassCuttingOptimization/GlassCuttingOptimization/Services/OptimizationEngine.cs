using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Drawing;
using GlassCuttingOptimization.Models.Dto;
using GlassCuttingOptimization.Models.Optimization;
using GlassCuttingOptimization.Models.Model;

namespace GlassCuttingOptimization.Services
{
    public class OptimizationEngine
    {
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
                if (orders == null || !orders.Any())
                    throw new ArgumentException("订单列表不能为空");

                if (originalSheets == null || !originalSheets.Any())
                    throw new ArgumentException("原片列表不能为空");

                var sortedOrders = orders.OrderByDescending(o => o.Rriority).ToList();
                var expandedPieces = ExpandOrders(sortedOrders);

                if (!expandedPieces.Any())
                    throw new InvalidOperationException("没有有效的玻璃片需要排版");

                // 根据算法类型选择排版策略
                switch (algorithm)
                {
                    case OptimizationAlgorithm.IntelligentPacking:
                        result.Sheets = IntelligentPackingAlgorithm(expandedPieces, originalSheets);
                        break;
                    case OptimizationAlgorithm.BestFitDecreasing:
                        result.Sheets = BestFitDecreasingAlgorithm(expandedPieces, originalSheets);
                        break;
                    case OptimizationAlgorithm.FirstFitDecreasing:
                        result.Sheets = FirstFitDecreasingAlgorithm(expandedPieces, originalSheets);
                        break;
                    case OptimizationAlgorithm.BottomLeftFill:
                        result.Sheets = BottomLeftFillAlgorithm(expandedPieces, originalSheets);
                        break;
                    case OptimizationAlgorithm.NextFitDecreasing:
                        result.Sheets = NextFitDecreasingAlgorithm(expandedPieces, originalSheets);
                        break;
                    default:
                        result.Sheets = IntelligentPackingAlgorithm(expandedPieces, originalSheets);
                        break;
                }

                result.CalculateOverallUtilization();
                // 保存原始状态
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

        private string GetAlgorithmDescription(OptimizationAlgorithm algorithm)
        {
            var field = algorithm.GetType().GetField(algorithm.ToString());
            var attribute = (System.ComponentModel.DescriptionAttribute)
                Attribute.GetCustomAttribute(field, typeof(System.ComponentModel.DescriptionAttribute));
            return attribute?.Description ?? algorithm.ToString();
        }

        private List<OptimizedGlassPiece> ExpandOrders(List<OrderDto> orders)
        {
            var pieces = new List<OptimizedGlassPiece>();
            int sequenceNumber = 1;

            foreach (var order in orders)
            {
                if (order.X <= 0 || order.Y <= 0) continue;

                for (int i = 0; i < order.Number; i++)
                {
                    pieces.Add(new OptimizedGlassPiece
                    {
                        OrderID = order.OrderID,
                        OrderNumber = order.Order ?? string.Empty,
                        Width = order.X,
                        Height = order.Y,
                        Priority = order.Rriority,
                        SequenceNumber = sequenceNumber++,
                        Customer = order.Customer ?? string.Empty,
                        Rotation = 0
                    });
                }
            }

            return pieces;
        }

        #region 智能排版算法
        private List<OptimizedSheet> IntelligentPackingAlgorithm(List<OptimizedGlassPiece> pieces, List<OriginalDto> originalSheets)
        {
            var result = new List<OptimizedSheet>();
            var remainingPieces = pieces.OrderByDescending(p => p.Width * p.Height).ToList();

            foreach (var original in originalSheets)
            {
                if (!int.TryParse(original.X1, out int sheetWidth) ||
                    !int.TryParse(original.Y1, out int sheetHeight) ||
                    sheetWidth <= 0 || sheetHeight <= 0) continue;

                for (int sheetCount = 0; sheetCount < original.Number; sheetCount++)
                {
                    if (!remainingPieces.Any()) break;

                    var sheet = new OptimizedSheet
                    {
                        OriginalID = original.ID,
                        Width = sheetWidth,
                        Height = sheetHeight
                    };

                    IntelligentPackSheet(sheet, remainingPieces);

                    if (sheet.GlassPieces.Any())
                        result.Add(sheet);
                    else
                        break;
                }
            }

            return result;
        }

        private void IntelligentPackSheet(OptimizedSheet sheet, List<OptimizedGlassPiece> remainingPieces)
        {
            var freeRectangles = new List<Rectangle> { new Rectangle(0, 0, sheet.Width, sheet.Height) };
            int maxIterations = remainingPieces.Count * 2;
            int iterations = 0;

            while (remainingPieces.Any() && freeRectangles.Any() && iterations < maxIterations)
            {
                iterations++;
                var bestPlacement = FindBestPlacement(remainingPieces, freeRectangles);
                if (bestPlacement == null) break;

                PlacePiece(sheet, bestPlacement, remainingPieces);
                UpdateFreeRectangles(freeRectangles, bestPlacement);
            }
        }
        #endregion

        #region 最佳适应递减算法
        private List<OptimizedSheet> BestFitDecreasingAlgorithm(List<OptimizedGlassPiece> pieces, List<OriginalDto> originalSheets)
        {
            var result = new List<OptimizedSheet>();
            var remainingPieces = pieces.OrderByDescending(p => p.Width * p.Height).ToList();

            foreach (var original in originalSheets)
            {
                if (!int.TryParse(original.X1, out int sheetWidth) ||
                    !int.TryParse(original.Y1, out int sheetHeight) ||
                    sheetWidth <= 0 || sheetHeight <= 0) continue;

                for (int sheetCount = 0; sheetCount < original.Number; sheetCount++)
                {
                    if (!remainingPieces.Any()) break;

                    var sheet = new OptimizedSheet
                    {
                        OriginalID = original.ID,
                        Width = sheetWidth,
                        Height = sheetHeight
                    };

                    BestFitPackSheet(sheet, remainingPieces);

                    if (sheet.GlassPieces.Any())
                        result.Add(sheet);
                    else
                        break;
                }
            }

            return result;
        }

        private void BestFitPackSheet(OptimizedSheet sheet, List<OptimizedGlassPiece> remainingPieces)
        {
            var placedPieces = new List<OptimizedGlassPiece>();

            foreach (var piece in remainingPieces.ToList())
            {
                var bestPosition = FindBestFitPosition(sheet, piece, placedPieces);
                if (bestPosition.HasValue)
                {
                    piece.X = bestPosition.Value.X;
                    piece.Y = bestPosition.Value.Y;
                    sheet.GlassPieces.Add(piece);
                    placedPieces.Add(piece);
                    remainingPieces.Remove(piece);
                }
                else
                {
                    // 尝试旋转
                    var rotatedPiece = piece.Clone();
                    rotatedPiece.Width = piece.Height;
                    rotatedPiece.Height = piece.Width;
                    rotatedPiece.Rotation = 90;

                    bestPosition = FindBestFitPosition(sheet, rotatedPiece, placedPieces);
                    if (bestPosition.HasValue)
                    {
                        piece.X = bestPosition.Value.X;
                        piece.Y = bestPosition.Value.Y;
                        piece.Width = rotatedPiece.Width;
                        piece.Height = rotatedPiece.Height;
                        piece.Rotation = 90;
                        sheet.GlassPieces.Add(piece);
                        placedPieces.Add(piece);
                        remainingPieces.Remove(piece);
                    }
                }
            }
        }

        private Point? FindBestFitPosition(OptimizedSheet sheet, OptimizedGlassPiece piece, List<OptimizedGlassPiece> placedPieces)
        {
            Point? bestPosition = null;
            int minWasteArea = int.MaxValue;

            for (int y = 0; y <= sheet.Height - piece.Height; y++)
            {
                for (int x = 0; x <= sheet.Width - piece.Width; x++)
                {
                    var testBounds = new Rectangle(x, y, piece.Width, piece.Height);

                    if (!HasOverlap(testBounds, placedPieces))
                    {
                        // 计算浪费的面积
                        int wasteArea = CalculateWasteArea(sheet, testBounds, placedPieces);
                        if (wasteArea < minWasteArea)
                        {
                            minWasteArea = wasteArea;
                            bestPosition = new Point(x, y);
                        }
                    }
                }
            }

            return bestPosition;
        }

        private int CalculateWasteArea(OptimizedSheet sheet, Rectangle newPiece, List<OptimizedGlassPiece> placedPieces)
        {
            // 简化计算：计算到边界的距离
            int rightWaste = sheet.Width - newPiece.Right;
            int bottomWaste = sheet.Height - newPiece.Bottom;
            return rightWaste * newPiece.Height + bottomWaste * newPiece.Width;
        }
        #endregion

        #region 首次适应递减算法
        private List<OptimizedSheet> FirstFitDecreasingAlgorithm(List<OptimizedGlassPiece> pieces, List<OriginalDto> originalSheets)
        {
            var result = new List<OptimizedSheet>();
            var remainingPieces = pieces.OrderByDescending(p => p.Width * p.Height).ToList();

            foreach (var original in originalSheets)
            {
                if (!int.TryParse(original.X1, out int sheetWidth) ||
                    !int.TryParse(original.Y1, out int sheetHeight) ||
                    sheetWidth <= 0 || sheetHeight <= 0) continue;

                for (int sheetCount = 0; sheetCount < original.Number; sheetCount++)
                {
                    if (!remainingPieces.Any()) break;

                    var sheet = new OptimizedSheet
                    {
                        OriginalID = original.ID,
                        Width = sheetWidth,
                        Height = sheetHeight
                    };

                    FirstFitPackSheet(sheet, remainingPieces);

                    if (sheet.GlassPieces.Any())
                        result.Add(sheet);
                    else
                        break;
                }
            }

            return result;
        }

        private void FirstFitPackSheet(OptimizedSheet sheet, List<OptimizedGlassPiece> remainingPieces)
        {
            var placedPieces = new List<OptimizedGlassPiece>();

            foreach (var piece in remainingPieces.ToList())
            {
                var position = FindFirstFitPosition(sheet, piece, placedPieces);
                if (position.HasValue)
                {
                    piece.X = position.Value.X;
                    piece.Y = position.Value.Y;
                    sheet.GlassPieces.Add(piece);
                    placedPieces.Add(piece);
                    remainingPieces.Remove(piece);
                }
                else
                {
                    // 尝试旋转
                    var rotatedPiece = piece.Clone();
                    rotatedPiece.Width = piece.Height;
                    rotatedPiece.Height = piece.Width;

                    position = FindFirstFitPosition(sheet, rotatedPiece, placedPieces);
                    if (position.HasValue)
                    {
                        piece.X = position.Value.X;
                        piece.Y = position.Value.Y;
                        piece.Width = rotatedPiece.Width;
                        piece.Height = rotatedPiece.Height;
                        piece.Rotation = 90;
                        sheet.GlassPieces.Add(piece);
                        placedPieces.Add(piece);
                        remainingPieces.Remove(piece);
                    }
                }
            }
        }

        private Point? FindFirstFitPosition(OptimizedSheet sheet, OptimizedGlassPiece piece, List<OptimizedGlassPiece> placedPieces)
        {
            for (int y = 0; y <= sheet.Height - piece.Height; y++)
            {
                for (int x = 0; x <= sheet.Width - piece.Width; x++)
                {
                    var testBounds = new Rectangle(x, y, piece.Width, piece.Height);

                    if (!HasOverlap(testBounds, placedPieces))
                    {
                        return new Point(x, y);
                    }
                }
            }
            return null;
        }
        #endregion

        #region 左下角填充算法
        private List<OptimizedSheet> BottomLeftFillAlgorithm(List<OptimizedGlassPiece> pieces, List<OriginalDto> originalSheets)
        {
            var result = new List<OptimizedSheet>();
            var remainingPieces = pieces.OrderByDescending(p => p.Width * p.Height).ToList();

            foreach (var original in originalSheets)
            {
                if (!int.TryParse(original.X1, out int sheetWidth) ||
                    !int.TryParse(original.Y1, out int sheetHeight) ||
                    sheetWidth <= 0 || sheetHeight <= 0) continue;

                for (int sheetCount = 0; sheetCount < original.Number; sheetCount++)
                {
                    if (!remainingPieces.Any()) break;

                    var sheet = new OptimizedSheet
                    {
                        OriginalID = original.ID,
                        Width = sheetWidth,
                        Height = sheetHeight
                    };

                    BottomLeftPackSheet(sheet, remainingPieces);

                    if (sheet.GlassPieces.Any())
                        result.Add(sheet);
                    else
                        break;
                }
            }

            return result;
        }

        private void BottomLeftPackSheet(OptimizedSheet sheet, List<OptimizedGlassPiece> remainingPieces)
        {
            var placedPieces = new List<OptimizedGlassPiece>();

            foreach (var piece in remainingPieces.ToList())
            {
                var position = FindBottomLeftPosition(sheet, piece, placedPieces);
                if (position.HasValue)
                {
                    piece.X = position.Value.X;
                    piece.Y = position.Value.Y;
                    sheet.GlassPieces.Add(piece);
                    placedPieces.Add(piece);
                    remainingPieces.Remove(piece);
                }
                else
                {
                    // 尝试旋转
                    var rotatedPiece = piece.Clone();
                    rotatedPiece.Width = piece.Height;
                    rotatedPiece.Height = piece.Width;

                    position = FindBottomLeftPosition(sheet, rotatedPiece, placedPieces);
                    if (position.HasValue)
                    {
                        piece.X = position.Value.X;
                        piece.Y = position.Value.Y;
                        piece.Width = rotatedPiece.Width;
                        piece.Height = rotatedPiece.Height;
                        piece.Rotation = 90;
                        sheet.GlassPieces.Add(piece);
                        placedPieces.Add(piece);
                        remainingPieces.Remove(piece);
                    }
                }
            }
        }

        private Point? FindBottomLeftPosition(OptimizedSheet sheet, OptimizedGlassPiece piece, List<OptimizedGlassPiece> placedPieces)
        {
            // 从底部左侧开始寻找位置
            for (int x = 0; x <= sheet.Width - piece.Width; x++)
            {
                for (int y = sheet.Height - piece.Height; y >= 0; y--)
                {
                    var testBounds = new Rectangle(x, y, piece.Width, piece.Height);

                    if (!HasOverlap(testBounds, placedPieces))
                    {
                        return new Point(x, y);
                    }
                }
            }
            return null;
        }
        #endregion

        #region 下次适应递减算法
        private List<OptimizedSheet> NextFitDecreasingAlgorithm(List<OptimizedGlassPiece> pieces, List<OriginalDto> originalSheets)
        {
            var result = new List<OptimizedSheet>();
            var remainingPieces = pieces.OrderByDescending(p => p.Width * p.Height).ToList();

            foreach (var original in originalSheets)
            {
                if (!int.TryParse(original.X1, out int sheetWidth) ||
                    !int.TryParse(original.Y1, out int sheetHeight) ||
                    sheetWidth <= 0 || sheetHeight <= 0) continue;

                for (int sheetCount = 0; sheetCount < original.Number; sheetCount++)
                {
                    if (!remainingPieces.Any()) break;

                    var sheet = new OptimizedSheet
                    {
                        OriginalID = original.ID,
                        Width = sheetWidth,
                        Height = sheetHeight
                    };

                    NextFitPackSheet(sheet, remainingPieces);

                    if (sheet.GlassPieces.Any())
                        result.Add(sheet);
                    else
                        break;
                }
            }

            return result;
        }

        private void NextFitPackSheet(OptimizedSheet sheet, List<OptimizedGlassPiece> remainingPieces)
        {
            var strips = new List<Strip>();
            int currentY = 0;

            while (remainingPieces.Any() && currentY < sheet.Height)
            {
                var strip = new Strip { Y = currentY, Height = 0, Width = sheet.Width };
                var stripPieces = new List<OptimizedGlassPiece>();

                // 在当前条带中尽可能多地放置玻璃片
                int currentX = 0;
                foreach (var piece in remainingPieces.ToList())
                {
                    if (currentX + piece.Width <= sheet.Width &&
                        currentY + piece.Height <= sheet.Height)
                    {
                        piece.X = currentX;
                        piece.Y = currentY;
                        stripPieces.Add(piece);
                        currentX += piece.Width;
                        strip.Height = Math.Max(strip.Height, piece.Height);
                    }
                    else
                    {
                        // 尝试旋转
                        if (currentX + piece.Height <= sheet.Width &&
                            currentY + piece.Width <= sheet.Height)
                        {
                            piece.X = currentX;
                            piece.Y = currentY;
                            piece.Width = piece.Height;
                            piece.Height = piece.Width;
                            piece.Rotation = 90;
                            stripPieces.Add(piece);
                            currentX += piece.Width;
                            strip.Height = Math.Max(strip.Height, piece.Height);
                        }
                    }
                }

                // 移除已放置的玻璃片
                foreach (var piece in stripPieces)
                {
                    sheet.GlassPieces.Add(piece);
                    remainingPieces.Remove(piece);
                }

                strips.Add(strip);
                currentY += strip.Height;
            }
        }

        private class Strip
        {
            public int Y { get; set; }
            public int Height { get; set; }
            public int Width { get; set; }
        }
        #endregion

        #region 通用辅助方法
        private PlacementResult FindBestPlacement(List<OptimizedGlassPiece> pieces, List<Rectangle> freeRectangles)
        {
            PlacementResult bestPlacement = null;
            double bestScore = double.MinValue;

            foreach (var piece in pieces)
            {
                foreach (var rect in freeRectangles)
                {
                    var placement1 = TryPlacement(piece, rect, piece.Width, piece.Height, 0);
                    if (placement1 != null)
                    {
                        double score = CalculatePlacementScore(placement1, rect);
                        if (score > bestScore)
                        {
                            bestScore = score;
                            bestPlacement = placement1;
                        }
                    }

                    if (piece.Width != piece.Height)
                    {
                        var placement2 = TryPlacement(piece, rect, piece.Height, piece.Width, 90);
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

        private PlacementResult TryPlacement(OptimizedGlassPiece piece, Rectangle freeRect, int width, int height, int rotation)
        {
            if (width <= freeRect.Width && height <= freeRect.Height)
            {
                return new PlacementResult
                {
                    Piece = piece,
                    Position = new Point(freeRect.X, freeRect.Y),
                    FinalWidth = width,
                    FinalHeight = height,
                    Rotation = rotation,
                    FreeRect = freeRect
                };
            }
            return null;
        }

        private double CalculatePlacementScore(PlacementResult placement, Rectangle freeRect)
        {
            double areaUtilization = (double)(placement.FinalWidth * placement.FinalHeight) / (freeRect.Width * freeRect.Height);
            double positionScore = 1.0 / (Math.Max(1, placement.Position.X + placement.Position.Y + 1));
            double priorityScore = placement.Piece.Priority / 10.0;
            double wasteArea = (freeRect.Width * freeRect.Height) - (placement.FinalWidth * placement.FinalHeight);
            double wasteScore = 1.0 / (wasteArea + 1);

            return areaUtilization * 0.4 + positionScore * 0.2 + priorityScore * 0.2 + wasteScore * 0.2;
        }

        private void PlacePiece(OptimizedSheet sheet, PlacementResult placement, List<OptimizedGlassPiece> remainingPieces)
        {
            placement.Piece.X = placement.Position.X;
            placement.Piece.Y = placement.Position.Y;
            placement.Piece.Width = placement.FinalWidth;
            placement.Piece.Height = placement.FinalHeight;
            placement.Piece.Rotation = placement.Rotation;

            sheet.GlassPieces.Add(placement.Piece);
            remainingPieces.Remove(placement.Piece);
        }

        private void UpdateFreeRectangles(List<Rectangle> freeRectangles, PlacementResult placement)
        {
            var placedRect = new Rectangle(placement.Position.X, placement.Position.Y,
                                         placement.FinalWidth, placement.FinalHeight);

            var newFreeRects = new List<Rectangle>();

            foreach (var rect in freeRectangles)
            {
                if (rect.IntersectsWith(placedRect))
                {
                    var splitRects = SplitRectangle(rect, placedRect);
                    newFreeRects.AddRange(splitRects);
                }
                else
                {
                    newFreeRects.Add(rect);
                }
            }

            freeRectangles.Clear();
            freeRectangles.AddRange(RemoveOverlappingRectangles(newFreeRects));
        }

        private List<Rectangle> SplitRectangle(Rectangle freeRect, Rectangle placedRect)
        {
            var result = new List<Rectangle>();

            if (placedRect.X > freeRect.X)
                result.Add(new Rectangle(freeRect.X, freeRect.Y, placedRect.X - freeRect.X, freeRect.Height));

            if (placedRect.Right < freeRect.Right)
                result.Add(new Rectangle(placedRect.Right, freeRect.Y, freeRect.Right - placedRect.Right, freeRect.Height));

            if (placedRect.Y > freeRect.Y)
                result.Add(new Rectangle(freeRect.X, freeRect.Y, freeRect.Width, placedRect.Y - freeRect.Y));

            if (placedRect.Bottom < freeRect.Bottom)
                result.Add(new Rectangle(freeRect.X, placedRect.Bottom, freeRect.Width, freeRect.Bottom - placedRect.Bottom));

            return result.Where(r => r.Width > 0 && r.Height > 0).ToList();
        }

        private List<Rectangle> RemoveOverlappingRectangles(List<Rectangle> rectangles)
        {
            var result = new List<Rectangle>();

            foreach (var rect in rectangles)
            {
                bool isContained = false;
                foreach (var existing in result)
                {
                    if (existing.Contains(rect))
                    {
                        isContained = true;
                        break;
                    }
                }

                if (!isContained && rect.Width > 0 && rect.Height > 0)
                {
                    result.RemoveAll(existing => rect.Contains(existing));
                    result.Add(rect);
                }
            }

            return result;
        }

        private bool HasOverlap(Rectangle bounds, List<OptimizedGlassPiece> placedPieces)
        {
            foreach (var placed in placedPieces)
            {
                if (bounds.IntersectsWith(placed.Bounds))
                    return true;
            }
            return false;
        }
        #endregion
    }

    public class PlacementResult
    {
        public OptimizedGlassPiece Piece { get; set; }
        public Point Position { get; set; }
        public int FinalWidth { get; set; }
        public int FinalHeight { get; set; }
        public int Rotation { get; set; }
        public Rectangle FreeRect { get; set; }
    }
}