using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlassCuttingOptimization.Models.Optimization
{
    /// <summary>
    /// 排版后的玻璃片
    /// </summary>
    public class OptimizedGlassPiece
    {
        public int OrderID { get; set; }
        public string OrderNumber { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Rotation { get; set; } // 0, 90, 180, 270
        public int Priority { get; set; }
        public int SequenceNumber { get; set; } // 排版序号
        public bool IsSelected { get; set; }
        public string Customer { get; set; }
        public Rectangle Bounds => new Rectangle(X, Y, Width, Height);

        // 添加原始状态备份
        public int OriginalX { get; set; }
        public int OriginalY { get; set; }
        public int OriginalWidth { get; set; }
        public int OriginalHeight { get; set; }
        public int OriginalRotation { get; set; }

        /// <summary>
        /// 保存当前状态为原始状态
        /// </summary>
        public void SaveAsOriginal()
        {
            OriginalX = X;
            OriginalY = Y;
            OriginalWidth = Width;
            OriginalHeight = Height;
            OriginalRotation = Rotation;
        }

        /// <summary>
        /// 恢复到原始状态
        /// </summary>
        public void RestoreToOriginal()
        {
            X = OriginalX;
            Y = OriginalY;
            Width = OriginalWidth;
            Height = OriginalHeight;
            Rotation = OriginalRotation;
        }

        /// <summary>
        /// 创建副本
        /// </summary>
        public OptimizedGlassPiece Clone()
        {
            return new OptimizedGlassPiece
            {
                OrderID = OrderID,
                OrderNumber = OrderNumber,
                Width = Width,
                Height = Height,
                X = X,
                Y = Y,
                Rotation = Rotation,
                Priority = Priority,
                SequenceNumber = SequenceNumber,
                IsSelected = IsSelected,
                Customer = Customer,
                OriginalX = OriginalX,
                OriginalY = OriginalY,
                OriginalWidth = OriginalWidth,
                OriginalHeight = OriginalHeight,
                OriginalRotation = OriginalRotation
            };
        }
    }

    /// <summary>
    /// 排版后的板材
    /// </summary>
    public class OptimizedSheet
    {
        public long OriginalID { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public List<OptimizedGlassPiece> GlassPieces { get; set; } = new List<OptimizedGlassPiece>();

        public double UtilizationRate
        {
            get => CalculateUtilizationRate();
        }

        private double CalculateUtilizationRate()
        {
            if (Width == 0 || Height == 0) return 0;

            double totalGlassArea = 0;
            foreach (var piece in GlassPieces)
            {
                totalGlassArea += piece.Width * piece.Height;
            }

            double sheetArea = Width * Height;
            return sheetArea > 0 ? (totalGlassArea / sheetArea) * 100 : 0;
        }

        /// <summary>
        /// 保存所有玻璃片的原始状态
        /// </summary>
        public void SaveGlassPiecesAsOriginal()
        {
            foreach (var piece in GlassPieces)
            {
                piece.SaveAsOriginal();
            }
        }

        /// <summary>
        /// 恢复所有玻璃片到原始状态
        /// </summary>
        public void RestoreGlassPiecesToOriginal()
        {
            foreach (var piece in GlassPieces)
            {
                piece.RestoreToOriginal();
            }
        }
    }

    /// <summary>
    /// 排版结果
    /// </summary>
    public class OptimizationResult
    {
        public List<OptimizedSheet> Sheets { get; set; } = new List<OptimizedSheet>();
        public double OverallUtilizationRate { get; set; }
        public string AlgorithmUsed { get; set; }
        public TimeSpan OptimizationTime { get; set; }

        public void CalculateOverallUtilization()
        {
            if (Sheets == null || !Sheets.Any())
            {
                OverallUtilizationRate = 0;
                return;
            }

            double totalArea = 0;
            double totalUsedArea = 0;

            foreach (var sheet in Sheets)
            {
                double sheetArea = sheet.Width * sheet.Height;
                double usedArea = 0;

                foreach (var piece in sheet.GlassPieces)
                {
                    usedArea += piece.Width * piece.Height;
                }

                totalArea += sheetArea;
                totalUsedArea += usedArea;
            }

            OverallUtilizationRate = totalArea > 0 ? (totalUsedArea / totalArea) * 100 : 0;
        }

        /// <summary>
        /// 保存所有板材的原始状态
        /// </summary>
        public void SaveAsOriginal()
        {
            foreach (var sheet in Sheets)
            {
                sheet.SaveGlassPiecesAsOriginal();
            }
        }

        /// <summary>
        /// 恢复所有板材到原始状态
        /// </summary>
        public void RestoreToOriginal()
        {
            foreach (var sheet in Sheets)
            {
                sheet.RestoreGlassPiecesToOriginal();
            }
            // 重新计算利用率
            CalculateOverallUtilization();
        }

        public OptimizationStats GetStatistics()
        {
            return new OptimizationStats
            {
                TotalSheets = Sheets?.Count ?? 0,
                TotalGlassPieces = Sheets?.Sum(s => s.GlassPieces.Count) ?? 0,
                AverageUtilizationRate = OverallUtilizationRate,
                BestUtilizationRate = Sheets?.Max(s => s.UtilizationRate) ?? 0,
                WorstUtilizationRate = Sheets?.Min(s => s.UtilizationRate) ?? 0,
                TotalArea = Sheets?.Sum(s => s.Width * s.Height) ?? 0,
                TotalUsedArea = Sheets?.Sum(s => s.GlassPieces.Sum(p => p.Width * p.Height)) ?? 0
            };
        }
    }

    public class OptimizationStats
    {
        public int TotalSheets { get; set; }
        public int TotalGlassPieces { get; set; }
        public double AverageUtilizationRate { get; set; }
        public double BestUtilizationRate { get; set; }
        public double WorstUtilizationRate { get; set; }
        public double TotalArea { get; set; }
        public double TotalUsedArea { get; set; }
    }

    public enum OptimizationAlgorithm
    {
        [System.ComponentModel.Description("智能排版")]
        IntelligentPacking,

        [System.ComponentModel.Description("最佳适应递减")]
        BestFitDecreasing,

        [System.ComponentModel.Description("首次适应递减")]
        FirstFitDecreasing,

        [System.ComponentModel.Description("左下角填充")]
        BottomLeftFill,

        [System.ComponentModel.Description("下次适应递减")]
        NextFitDecreasing
    }
}
