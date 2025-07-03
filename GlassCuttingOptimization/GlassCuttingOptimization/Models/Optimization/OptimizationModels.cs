using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlassCuttingOptimization.Models.Optimization
{

    public class GCodeFileResult
    {
        public string FileName { get; set; }
        public string GCode { get; set; }
        public int SheetIndex { get; set; }
        public int PieceCount { get; set; }
        public string SheetSize { get; set; }
    }
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
        public int Priority { get; set; }
        public int SequenceNumber { get; set; }
        public string Customer { get; set; }
        public int Rotation { get; set; } // 0, 90, 180, 270
        public string Note { get; set; }
        //public int OrderID { get; set; }
        //public string OrderNumber { get; set; }
        //public int Width { get; set; }
        //public int Height { get; set; }
        //public int X { get; set; }
        //public int Y { get; set; }
        //public int Rotation { get; set; } // 0, 90, 180, 270
        //public int Priority { get; set; }
        //public int SequenceNumber { get; set; } // 排版序号
        public bool IsSelected { get; set; }
        //public string Customer { get; set; }
        public Rectangle Bounds => new Rectangle(X, Y, Width, Height);
        // 磨边信息
        public bool BottomEdge { get; set; }
        public bool TopEdge { get; set; }
        public bool LeftEdge { get; set; }
        public bool RightEdge { get; set; }

        // 添加原始状态备份
        public int OriginalX { get; set; }
        public int OriginalY { get; set; }
        public int OriginalWidth { get; set; }
        public int OriginalHeight { get; set; }
        public int OriginalRotation { get; set; }
        /// <summary>
        /// 检查是否与其他玻璃片重叠
        /// </summary>
        public bool IntersectsWith(OptimizedGlassPiece other)
        {
            return Bounds.IntersectsWith(other.Bounds);
        }

        /// <summary>
        /// 检查是否在指定区域内
        /// </summary>
        public bool IsWithinBounds(Rectangle area)
        {
            return area.Contains(Bounds);
        }
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
    /// 切割路径点
    /// </summary>
    public class CuttingPoint
    {
        public int X { get; set; }
        public int Y { get; set; }
        public CuttingDirection Direction { get; set; }
        public int SequenceNumber { get; set; }
    }

    /// <summary>
    /// 切割方向
    /// </summary>
    public enum CuttingDirection
    {
        Horizontal, // 水平切割
        Vertical    // 垂直切割
    }

    /// <summary>
    /// G代码生成结果
    /// </summary>
    public class GCodeResult
    {
        public string GCode { get; set; }
        public List<string> Lines { get; set; } = new List<string>();
        public int TotalPieces { get; set; }
        public TimeSpan GenerationTime { get; set; }
        public string FileName { get; set; }
    }
    /// <summary>
    /// 优化后的板材
    /// </summary>
    public class OptimizedSheet
    {


        public int Width { get; set; }
        public int Height { get; set; }
        public string MaterialType { get; set; }
        public double Thickness { get; set; }

        // 修边参数
        public int X1 { get; set; }
        public int X2 { get; set; }
        public int Y1 { get; set; }
        public int Y2 { get; set; }

        public List<OptimizedGlassPiece> GlassPieces { get; set; } = new List<OptimizedGlassPiece>();

        public double UtilizationRate
        {
            get
            {
                double totalArea = (double)Width * Height;
                double usedArea = GlassPieces.Sum(p => (double)p.Width * p.Height);
                return totalArea > 0 ? (usedArea / totalArea) * 100 : 0;
            }
        }

        /// <summary>
        /// 获取有效切割区域（考虑修边）
        /// </summary>
        public Rectangle GetEffectiveArea()
        {
            return new Rectangle(X1, Y1, Width - X1 - X2, Height - Y1 - Y2);
        }
    }

    /// <summary>
    /// 排版后的板材
    /// </summary>
    //public class OptimizedSheet
    //{
    //    public long OriginalID { get; set; }
    //    public int Width { get; set; }
    //    public int Height { get; set; }
    //    public List<OptimizedGlassPiece> GlassPieces { get; set; } = new List<OptimizedGlassPiece>();

    //    public double UtilizationRate
    //    {
    //        get => CalculateUtilizationRate();
    //    }

    //    private double CalculateUtilizationRate()
    //    {
    //        if (Width == 0 || Height == 0) return 0;

    //        double totalGlassArea = 0;
    //        foreach (var piece in GlassPieces)
    //        {
    //            totalGlassArea += piece.Width * piece.Height;
    //        }

    //        double sheetArea = Width * Height;
    //        return sheetArea > 0 ? (totalGlassArea / sheetArea) * 100 : 0;
    //    }

    //    /// <summary>
    //    /// 保存所有玻璃片的原始状态
    //    /// </summary>
    //    public void SaveGlassPiecesAsOriginal()
    //    {
    //        foreach (var piece in GlassPieces)
    //        {
    //            piece.SaveAsOriginal();
    //        }
    //    }

    //    /// <summary>
    //    /// 恢复所有玻璃片到原始状态
    //    /// </summary>
    //    public void RestoreGlassPiecesToOriginal()
    //    {
    //        foreach (var piece in GlassPieces)
    //        {
    //            piece.RestoreToOriginal();
    //        }
    //    }
    //}

    ///// <summary>
    ///// 排版结果
    ///// </summary>
    //public class OptimizationResult
    //{
    //    public List<OptimizedSheet> Sheets { get; set; } = new List<OptimizedSheet>();
    //    public double OverallUtilizationRate { get; set; }
    //    public string AlgorithmUsed { get; set; }
    //    public TimeSpan OptimizationTime { get; set; }

    //    public void CalculateOverallUtilization()
    //    {
    //        if (Sheets == null || !Sheets.Any())
    //        {
    //            OverallUtilizationRate = 0;
    //            return;
    //        }

    //        double totalArea = 0;
    //        double totalUsedArea = 0;

    //        foreach (var sheet in Sheets)
    //        {
    //            double sheetArea = sheet.Width * sheet.Height;
    //            double usedArea = 0;

    //            foreach (var piece in sheet.GlassPieces)
    //            {
    //                usedArea += piece.Width * piece.Height;
    //            }

    //            totalArea += sheetArea;
    //            totalUsedArea += usedArea;
    //        }

    //        OverallUtilizationRate = totalArea > 0 ? (totalUsedArea / totalArea) * 100 : 0;
    //    }

    //    /// <summary>
    //    /// 保存所有板材的原始状态
    //    /// </summary>
    //    public void SaveAsOriginal()
    //    {
    //        foreach (var sheet in Sheets)
    //        {
    //            sheet.SaveGlassPiecesAsOriginal();
    //        }
    //    }

    //    /// <summary>
    //    /// 恢复所有板材到原始状态
    //    /// </summary>
    //    public void RestoreToOriginal()
    //    {
    //        foreach (var sheet in Sheets)
    //        {
    //            sheet.RestoreGlassPiecesToOriginal();
    //        }
    //        // 重新计算利用率
    //        CalculateOverallUtilization();
    //    }

    //    public OptimizationStats GetStatistics()
    //    {
    //        return new OptimizationStats
    //        {
    //            TotalSheets = Sheets?.Count ?? 0,
    //            TotalGlassPieces = Sheets?.Sum(s => s.GlassPieces.Count) ?? 0,
    //            AverageUtilizationRate = OverallUtilizationRate,
    //            BestUtilizationRate = Sheets?.Max(s => s.UtilizationRate) ?? 0,
    //            WorstUtilizationRate = Sheets?.Min(s => s.UtilizationRate) ?? 0,
    //            TotalArea = Sheets?.Sum(s => s.Width * s.Height) ?? 0,
    //            TotalUsedArea = Sheets?.Sum(s => s.GlassPieces.Sum(p => p.Width * p.Height)) ?? 0
    //        };
    //    }
    //}




    /// <summary>
    /// 排版结果
    /// </summary>
    public class OptimizationResult
    {
        public List<OptimizedSheet> Sheets { get; set; } = new List<OptimizedSheet>();
        public string AlgorithmUsed { get; set; }
        public double OverallUtilizationRate { get; set; }
        public TimeSpan OptimizationTime { get; set; }

        // 用于重置功能的原始状态
        private List<OptimizedSheet> _originalSheets;

        public void CalculateOverallUtilization()
        {
            if (!Sheets.Any())
            {
                OverallUtilizationRate = 0;
                return;
            }

            double totalSheetArea = Sheets.Sum(s => (double)s.Width * s.Height);
            double totalUsedArea = Sheets.Sum(s => s.GlassPieces.Sum(p => (double)p.Width * p.Height));

            OverallUtilizationRate = totalSheetArea > 0 ? (totalUsedArea / totalSheetArea) * 100 : 0;
        }

        public void SaveAsOriginal()
        {
            _originalSheets = DeepCopySheets(Sheets);
        }

        public void RestoreToOriginal()
        {
            if (_originalSheets != null)
            {
                Sheets = DeepCopySheets(_originalSheets);
            }
        }

        private List<OptimizedSheet> DeepCopySheets(List<OptimizedSheet> sheets)
        {
            return sheets.Select(s => new OptimizedSheet
            {
                Width = s.Width,
                Height = s.Height,
                MaterialType = s.MaterialType,
                Thickness = s.Thickness,
                X1 = s.X1,
                X2 = s.X2,
                Y1 = s.Y1,
                Y2 = s.Y2,
                GlassPieces = s.GlassPieces.Select(p => new OptimizedGlassPiece
                {
                    OrderID = p.OrderID,
                    OrderNumber = p.OrderNumber,
                    Width = p.Width,
                    Height = p.Height,
                    X = p.X,
                    Y = p.Y,
                    Priority = p.Priority,
                    SequenceNumber = p.SequenceNumber,
                    Customer = p.Customer,
                    Rotation = p.Rotation,
                    Note = p.Note,
                    BottomEdge = p.BottomEdge,
                    TopEdge = p.TopEdge,
                    LeftEdge = p.LeftEdge,
                    RightEdge = p.RightEdge
                }).ToList()
            }).ToList();
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
        [Description("智能打包算法")]
        IntelligentPacking,
        [Description("最佳适应递减算法")]
        BestFitDecreasing,
        [Description("首次适应递减算法")]
        FirstFitDecreasing,
        [Description("左下角填充算法")]
        BottomLeftFill,
        [Description("下次适应递减算法")]
        NextFitDecreasing
    }
}
