using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using GlassCuttingOptimization.Models.Optimization;

namespace GlassCuttingOptimization.Services
{
    public class GCodeGenerator
    {
        private int _lineNumber = 1;

        public GCodeResult GenerateGCode(OptimizationResult optimizationResult)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            var result = new GCodeResult();
            var lines = new List<string>();

            try
            {
                _lineNumber = 1; // 重置行号

                foreach (var sheet in optimizationResult.Sheets)
                {
                    lines.AddRange(GenerateSheetGCode(sheet));
                }

                result.Lines = lines;
                result.GCode = string.Join(Environment.NewLine, lines);
                result.TotalPieces = optimizationResult.Sheets.Sum(s => s.GlassPieces.Count);
                result.FileName = $"GlassCutting_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
            }
            catch (Exception ex)
            {
                throw new Exception($"G代码生成失败: {ex.Message}", ex);
            }
            finally
            {
                stopwatch.Stop();
                result.GenerationTime = stopwatch.Elapsed;
            }

            return result;
        }

        private List<string> GenerateSheetGCode(OptimizedSheet sheet)
        {
            var lines = new List<string>();

            // 1. 生成参数设置部分（带行号）
            lines.AddRange(GenerateParameterSection(sheet));

            // 2. 生成机器初始化指令
            lines.AddRange(GenerateMachineInitialization());

            // 3. 生成切割路径
            lines.AddRange(GenerateCuttingPath(sheet));

            // 4. 程序结束
            lines.Add("M30");
            lines.Add(""); // 空行分隔

            return lines;
        }

        private List<string> GenerateParameterSection(OptimizedSheet sheet)
        {
            var lines = new List<string>();

            // 板材参数（P3000-P3011）
            lines.Add(AddLineNumber($"P3000 = {sheet.Width}"));       // 原片X尺寸
            lines.Add(AddLineNumber($"P3001 = {sheet.Height}"));      // 原片Y尺寸
            lines.Add(AddLineNumber($"P3002 = {sheet.X1}"));          // X1修边
            lines.Add(AddLineNumber($"P3003 = {sheet.X2}"));          // X2修边
            lines.Add(AddLineNumber($"P3004 = {sheet.Y1}"));          // Y1修边
            lines.Add(AddLineNumber($"P3005 = {sheet.Y2}"));          // Y2修边
            lines.Add(AddLineNumber($"P3006 = 0"));                   // 保留参数
            lines.Add(AddLineNumber($"P3007 = {CleanString(sheet.MaterialType)}"));  // 材料类型
            lines.Add(AddLineNumber($"P3008 = 0"));                   // 保留参数
            lines.Add(AddLineNumber($"P3009 = 0"));                   // 保留参数
            lines.Add(AddLineNumber($"P3010 = 0"));                   // 保留参数
            lines.Add(AddLineNumber($"P3011 = {sheet.Thickness}"));   // 厚度

            // 排序玻璃片（按Y坐标，然后按X坐标）
            var sortedPieces = sheet.GlassPieces
                .OrderBy(p => p.Y)
                .ThenBy(p => p.X)
                .ToList();

            // 重新分配序号
            for (int i = 0; i < sortedPieces.Count; i++)
            {
                sortedPieces[i].SequenceNumber = i + 1;
            }

            // 生成每个玻璃片的参数（P4001-P40xx）
            foreach (var piece in sortedPieces)
            {
                var gcodeLine = GeneratePieceGCode(piece);
                lines.Add(AddLineNumber(gcodeLine));
            }

            return lines;
        }

        private List<string> GenerateMachineInitialization()
        {
            var lines = new List<string>();

            // 机器初始化指令
            lines.Add("G17");          // XY平面选择
            lines.Add("G92 X0 Y0");    // 设置当前位置为原点
            lines.Add("G90");          // 绝对坐标模式
            lines.Add("G20");          // 英寸单位（或G21毫米单位，根据机器设定）

            return lines;
        }

        private List<string> GenerateCuttingPath(OptimizedSheet sheet)
        {
            var lines = new List<string>();

            // 获取所有需要切割的线段
            var cuttingLines = GenerateCuttingLines(sheet);

            // 优化切割路径
            var optimizedPath = OptimizeCuttingPath(cuttingLines);

            // 移动到起始位置
            if (optimizedPath.Any())
            {
                var firstLine = optimizedPath.First();
                lines.Add($"G00 X{firstLine.StartX} Y{firstLine.StartY}");  // 快速移动到起始点
                lines.Add("M03");  // 启动主轴
                lines.Add("M09");  // 关闭冷却液
            }

            // 生成切割路径
            foreach (var line in optimizedPath)
            {
                // 切割移动
                lines.Add($"G01 X{line.EndX} Y{line.EndY}");
                lines.Add("M10");  // 开启冷却液（切割完成后）

                // 如果不是最后一条线，且下一条线的起始点不同，需要快速移动
                var nextLine = optimizedPath.Where(l => optimizedPath.IndexOf(l) == optimizedPath.IndexOf(line) + 1).FirstOrDefault();
                if (nextLine != null && (nextLine.StartX != line.EndX || nextLine.StartY != line.EndY))
                {
                    lines.Add("M09");  // 关闭冷却液
                    lines.Add($"G00 X{nextLine.StartX} Y{nextLine.StartY}");  // 快速移动到下一个起始点
                }
            }

            // 关闭工具
            if (optimizedPath.Any())
            {
                lines.Add("M09");  // 关闭冷却液
                lines.Add("M04");  // 停止主轴
            }

            return lines;
        }

        private List<CuttingLine> GenerateCuttingLines(OptimizedSheet sheet)
        {
            var lines = new List<CuttingLine>();
            var allPieces = sheet.GlassPieces.OrderBy(p => p.Y).ThenBy(p => p.X).ToList();

            // 生成水平切割线
            var horizontalLines = GenerateHorizontalCuttingLines(allPieces, sheet);
            lines.AddRange(horizontalLines);

            // 生成垂直切割线
            var verticalLines = GenerateVerticalCuttingLines(allPieces, sheet);
            lines.AddRange(verticalLines);

            // 去除重复的切割线
            return RemoveDuplicateLines(lines);
        }

        private List<CuttingLine> GenerateHorizontalCuttingLines(List<OptimizedGlassPiece> pieces, OptimizedSheet sheet)
        {
            var lines = new List<CuttingLine>();
            var yCoordinates = new HashSet<int>();

            // 收集所有需要水平切割的Y坐标
            foreach (var piece in pieces)
            {
                yCoordinates.Add(piece.Y);          // 玻璃片顶部
                yCoordinates.Add(piece.Y + piece.Height); // 玻璃片底部
            }

            // 为每个Y坐标生成水平切割线
            foreach (var y in yCoordinates.OrderBy(y => y))
            {
                // 找到在这个Y坐标上需要切割的X范围
                var xRanges = GetHorizontalCuttingRanges(y, pieces, sheet);

                foreach (var range in xRanges)
                {
                    lines.Add(new CuttingLine
                    {
                        StartX = range.Start,
                        StartY = y,
                        EndX = range.End,
                        EndY = y,
                        Direction = CuttingDirection.Horizontal
                    });
                }
            }

            return lines;
        }

        private List<CuttingLine> GenerateVerticalCuttingLines(List<OptimizedGlassPiece> pieces, OptimizedSheet sheet)
        {
            var lines = new List<CuttingLine>();
            var xCoordinates = new HashSet<int>();

            // 收集所有需要垂直切割的X坐标
            foreach (var piece in pieces)
            {
                xCoordinates.Add(piece.X);          // 玻璃片左侧
                xCoordinates.Add(piece.X + piece.Width);  // 玻璃片右侧
            }

            // 为每个X坐标生成垂直切割线
            foreach (var x in xCoordinates.OrderBy(x => x))
            {
                // 找到在这个X坐标上需要切割的Y范围
                var yRanges = GetVerticalCuttingRanges(x, pieces, sheet);

                foreach (var range in yRanges)
                {
                    lines.Add(new CuttingLine
                    {
                        StartX = x,
                        StartY = range.Start,
                        EndX = x,
                        EndY = range.End,
                        Direction = CuttingDirection.Vertical
                    });
                }
            }

            return lines;
        }

        private List<CuttingRange> GetHorizontalCuttingRanges(int y, List<OptimizedGlassPiece> pieces, OptimizedSheet sheet)
        {
            var ranges = new List<CuttingRange>();
            var intersectingPieces = pieces.Where(p => p.Y <= y && p.Y + p.Height >= y).OrderBy(p => p.X).ToList();

            if (!intersectingPieces.Any()) return ranges;

            int startX = intersectingPieces.First().X;
            int endX = intersectingPieces.Last().X + intersectingPieces.Last().Width;

            ranges.Add(new CuttingRange { Start = startX, End = endX });

            return ranges;
        }

        private List<CuttingRange> GetVerticalCuttingRanges(int x, List<OptimizedGlassPiece> pieces, OptimizedSheet sheet)
        {
            var ranges = new List<CuttingRange>();
            var intersectingPieces = pieces.Where(p => p.X <= x && p.X + p.Width >= x).OrderBy(p => p.Y).ToList();

            if (!intersectingPieces.Any()) return ranges;

            int startY = intersectingPieces.First().Y;
            int endY = intersectingPieces.Last().Y + intersectingPieces.Last().Height;

            ranges.Add(new CuttingRange { Start = startY, End = endY });

            return ranges;
        }

        private List<CuttingLine> OptimizeCuttingPath(List<CuttingLine> lines)
        {
            if (!lines.Any()) return lines;

            var optimizedPath = new List<CuttingLine>();
            var remainingLines = new List<CuttingLine>(lines);

            // 从左下角开始
            var currentPosition = new { X = 0, Y = 0 };

            while (remainingLines.Any())
            {
                // 找到最近的切割线
                var nearestLine = remainingLines
                    .OrderBy(line => Math.Sqrt(Math.Pow(line.StartX - currentPosition.X, 2) + Math.Pow(line.StartY - currentPosition.Y, 2)))
                    .First();

                optimizedPath.Add(nearestLine);
                remainingLines.Remove(nearestLine);

                // 更新当前位置
                currentPosition = new { X = nearestLine.EndX, Y = nearestLine.EndY };
            }

            return optimizedPath;
        }

        private List<CuttingLine> RemoveDuplicateLines(List<CuttingLine> lines)
        {
            var uniqueLines = new List<CuttingLine>();

            foreach (var line in lines)
            {
                bool isDuplicate = uniqueLines.Any(existing =>
                    (existing.StartX == line.StartX && existing.StartY == line.StartY &&
                     existing.EndX == line.EndX && existing.EndY == line.EndY) ||
                    (existing.StartX == line.EndX && existing.StartY == line.EndY &&
                     existing.EndX == line.StartX && existing.EndY == line.StartY));

                if (!isDuplicate)
                {
                    uniqueLines.Add(line);
                }
            }

            return uniqueLines;
        }

        private string GeneratePieceGCode(OptimizedGlassPiece piece)
        {
            // G代码格式: P4xxx=长_宽_X坐标_Y坐标_显示长_显示宽_客户名_定位号_货架_订单号_注释_磨边信息
            var parts = new List<string>
            {
                piece.Width.ToString(),     // 长
                piece.Height.ToString(),   // 宽
                piece.X.ToString(),        // X坐标
                piece.Y.ToString(),        // Y坐标
                piece.Width.ToString(),    // 显示长
                piece.Height.ToString(),   // 显示宽
                CleanString(piece.Customer), // 客户名
                piece.SequenceNumber.ToString(), // 定位号
                "1",                       // 货架（默认1）
                CleanString(piece.OrderNumber), // 订单号
                CleanString(piece.Note),   // 注释
                GenerateEdgeInfo(piece)    // 磨边信息
            };

            var parameterNumber = 4000 + piece.SequenceNumber;
            return $"P{parameterNumber}= {string.Join("_", parts)}";
        }

        private string GenerateEdgeInfo(OptimizedGlassPiece piece)
        {
            // 磨边信息格式：底_顶_左_右 (1表示磨边，0表示不磨边)
            return $"{(piece.BottomEdge ? 1 : 0)}{(piece.TopEdge ? 1 : 0)}{(piece.LeftEdge ? 1 : 0)}{(piece.RightEdge ? 1 : 0)}";
        }

        private string CleanString(string input)
        {
            if (string.IsNullOrEmpty(input)) return "";

            // 移除特殊字符，避免G代码解析错误
            return input.Replace("_", "-").Replace(" ", "").Replace("\r", "").Replace("\n", "");
        }

        private string AddLineNumber(string line)
        {
            return $"N{_lineNumber++:D2}  {line}";
        }

        /// <summary>
        /// 保存G代码到文件
        /// </summary>
        public void SaveGCodeToFile(GCodeResult gCodeResult, string filePath)
        {
            try
            {
                File.WriteAllText(filePath, gCodeResult.GCode, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                throw new Exception($"保存G代码文件失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 生成切割路径优化的G代码（高级功能）
        /// </summary>
        public GCodeResult GenerateOptimizedCuttingPath(OptimizationResult optimizationResult)
        {
            // 使用完整的G代码生成，包含切割路径优化
            return GenerateGCode(optimizationResult);
        }
    }

    /// <summary>
    /// 切割线
    /// </summary>
    public class CuttingLine
    {
        public int StartX { get; set; }
        public int StartY { get; set; }
        public int EndX { get; set; }
        public int EndY { get; set; }
        public CuttingDirection Direction { get; set; }
    }

    /// <summary>
    /// 切割范围
    /// </summary>
    public class CuttingRange
    {
        public int Start { get; set; }
        public int End { get; set; }
    }
}