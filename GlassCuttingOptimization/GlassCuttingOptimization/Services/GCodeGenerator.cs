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



        public List<GCodeFileResult> GenerateGCodeFiles(OptimizationResult optimizationResult, string baseName = "M0000031")
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            var results = new List<GCodeFileResult>();

            try
            {
                for (int i = 0; i < optimizationResult.Sheets.Count; i++)
                {
                    var sheet = optimizationResult.Sheets[i];
                    var sheetGCode = GenerateSheetGCode(sheet);

                    // 生成文件名：名称+序号+原片使用数量+原片的大小+文件格式
                    var fileName = $"{baseName}_{(i + 1)}_{sheet.GlassPieces.Count}_{sheet.Width}_{sheet.Height}.g";

                    var result = new GCodeFileResult
                    {
                        FileName = fileName,
                        GCode = string.Join(Environment.NewLine, sheetGCode),
                        SheetIndex = i + 1,
                        PieceCount = sheet.GlassPieces.Count,
                        SheetSize = $"{sheet.Width}x{sheet.Height}"
                    };

                    results.Add(result);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"G代码生成失败: {ex.Message}", ex);
            }
            finally
            {
                stopwatch.Stop();
            }

            return results;
        }
        public GCodeResult GenerateGCode(OptimizationResult optimizationResult)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            var result = new GCodeResult();
            var lines = new List<string>();

            try
            {
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
        public void SaveGCodeFiles(List<GCodeFileResult> gCodeResults, string folderPath)
        {
            try
            {
                // 确保文件夹存在
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                foreach (var result in gCodeResults)
                {
                    var filePath = Path.Combine(folderPath, result.FileName);
                    File.WriteAllText(filePath, result.GCode, Encoding.UTF8);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"保存G代码文件失败: {ex.Message}", ex);
            }
        }
        private List<string> GenerateSheetGCode(OptimizedSheet sheet)
        {
            var lines = new List<string>();

            // 添加行号计数器
            int lineNumber = 1;

            // 板材头部信息 - 添加行号
            lines.Add($"N{lineNumber:D2}  P3000 = {sheet.Width}"); lineNumber++;      // 原片X尺寸
            lines.Add($"N{lineNumber:D2}  P3001 = {sheet.Height}"); lineNumber++;     // 原片Y尺寸
            lines.Add($"N{lineNumber:D2}  P3002 = {sheet.X1}"); lineNumber++;        // X1修边
            lines.Add($"N{lineNumber:D2}  P3003 = {sheet.X2}"); lineNumber++;        // X2修边
            lines.Add($"N{lineNumber:D2}  P3004 = {sheet.Y1}"); lineNumber++;        // Y1修边
            lines.Add($"N{lineNumber:D2}  P3005 = {sheet.Y2}"); lineNumber++;        // Y2修边
            lines.Add($"N{lineNumber:D2}  P3006 = 1"); lineNumber++;                 // 参数6
            lines.Add($"N{lineNumber:D2}  P3007 = {GetMaterialTypeCode(sheet.MaterialType)}"); lineNumber++; // 材料类型
            lines.Add($"N{lineNumber:D2}  P3008 = 1"); lineNumber++;                 // 参数8
            lines.Add($"N{lineNumber:D2}  P3009 = 2"); lineNumber++;                 // 参数9
            lines.Add($"N{lineNumber:D2}  P3010 = "); lineNumber++;                  // 参数10 (空)
            lines.Add($"N{lineNumber:D2}  P3011 = {sheet.Thickness}"); lineNumber++; // 厚度(Grinding)

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

            // 生成每个玻璃片的参数信息
            foreach (var piece in sortedPieces)
            {
                var gcodeLine = GeneratePieceGCode(piece, lineNumber);
                lines.Add(gcodeLine);
                lineNumber++;
            }

            // 添加空行分隔
            lines.Add("");

            // 生成切割路径部分
            lines.AddRange(GenerateCuttingPath(sortedPieces, sheet));

            return lines;
        }

        private string GeneratePieceGCode(OptimizedGlassPiece piece, int lineNumber)
        {
            // G代码格式: P4xxx=长_宽_X坐标_Y坐标_显示长_显示宽_客户名_定位号_货架_订单号_注释1_注释2_...注释10_磨边信息
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
                "",                        // 货架（空）
                CleanString(piece.OrderNumber), // 订单号
            };

            // 添加10个注释字段
            for (int i = 0; i < 10; i++)
            {
                if (i == 0)
                    parts.Add(CleanString(piece.Note)); // 第一个注释使用piece的Note
                else
                    parts.Add(""); // 其他注释为空
            }

            // 添加磨边信息（可能需要多个字段）
            for (int i = 0; i < 10; i++)
            {
                parts.Add(""); // 磨边信息字段，根据需要填充
            }

            var parameterNumber = 4000 + piece.SequenceNumber;
            return $"N{lineNumber:D2}  P{parameterNumber}= {string.Join("_", parts)}";
        }

        private List<string> GenerateCuttingPath(List<OptimizedGlassPiece> pieces, OptimizedSheet sheet)
        {
            var lines = new List<string>();

            // G代码初始化
            lines.Add("G17");           // XY平面选择
            lines.Add("G92 X0 Y0");     // 坐标系设置
            lines.Add("G90");           // 绝对坐标模式
            lines.Add("G20");           // 英制单位

            // 生成切割路径
            var cuttingLines = GenerateOptimizedCuttingLines(pieces, sheet);

            foreach (var line in cuttingLines)
            {
                lines.AddRange(GenerateCutForLine(line));
            }

            // 结束代码
            lines.Add("M04");           // 主轴停止
            lines.Add("G90G00X0Y0Z0");  // 回到原点
            lines.Add("M23");
            lines.Add("M24");
            lines.Add("M30");           // 程序结束

            return lines;
        }

        private List<CuttingLine> GenerateOptimizedCuttingLines(List<OptimizedGlassPiece> pieces, OptimizedSheet sheet)
        {
            var lines = new List<CuttingLine>();

            // 收集所有需要切割的线条
            var horizontalLines = new HashSet<(int y, int x1, int x2)>();
            var verticalLines = new HashSet<(int x, int y1, int y2)>();

            // 添加外边界切割线
            foreach (var piece in pieces)
            {
                int x1 = piece.X;
                int y1 = piece.Y;
                int x2 = piece.X + piece.Width;
                int y2 = piece.Y + piece.Height;

                // 水平线（上下边）
                horizontalLines.Add((y1, x1, x2));
                horizontalLines.Add((y2, x1, x2));

                // 垂直线（左右边）
                verticalLines.Add((x1, y1, y2));
                verticalLines.Add((x2, y1, y2));
            }

            // 合并相邻的水平线
            var mergedHorizontalLines = MergeHorizontalLines(horizontalLines.ToList());
            foreach (var line in mergedHorizontalLines)
            {
                lines.Add(new CuttingLine
                {
                    X1 = line.x1,
                    Y1 = line.y,
                    X2 = line.x2,
                    Y2 = line.y,
                    IsHorizontal = true
                });
            }

            // 合并相邻的垂直线
            var mergedVerticalLines = MergeVerticalLines(verticalLines.ToList());
            foreach (var line in mergedVerticalLines)
            {
                lines.Add(new CuttingLine
                {
                    X1 = line.x,
                    Y1 = line.y1,
                    X2 = line.x,
                    Y2 = line.y2,
                    IsHorizontal = false
                });
            }

            // 优化切割顺序（减少移动距离）
            return OptimizeCuttingOrder(lines);
        }

        private List<(int y, int x1, int x2)> MergeHorizontalLines(List<(int y, int x1, int x2)> lines)
        {
            var merged = new List<(int y, int x1, int x2)>();
            var groupedByY = lines.GroupBy(l => l.y).ToList();

            foreach (var group in groupedByY)
            {
                var sortedLines = group.OrderBy(l => l.x1).ToList();
                var currentLine = sortedLines[0];

                for (int i = 1; i < sortedLines.Count; i++)
                {
                    var nextLine = sortedLines[i];

                    // 如果线条相邻或重叠，合并它们
                    if (nextLine.x1 <= currentLine.x2)
                    {
                        currentLine = (currentLine.y, currentLine.x1, Math.Max(currentLine.x2, nextLine.x2));
                    }
                    else
                    {
                        merged.Add(currentLine);
                        currentLine = nextLine;
                    }
                }
                merged.Add(currentLine);
            }

            return merged;
        }

        private List<(int x, int y1, int y2)> MergeVerticalLines(List<(int x, int y1, int y2)> lines)
        {
            var merged = new List<(int x, int y1, int y2)>();
            var groupedByX = lines.GroupBy(l => l.x).ToList();

            foreach (var group in groupedByX)
            {
                var sortedLines = group.OrderBy(l => l.y1).ToList();
                var currentLine = sortedLines[0];

                for (int i = 1; i < sortedLines.Count; i++)
                {
                    var nextLine = sortedLines[i];

                    // 如果线条相邻或重叠，合并它们
                    if (nextLine.y1 <= currentLine.y2)
                    {
                        currentLine = (currentLine.x, currentLine.y1, Math.Max(currentLine.y2, nextLine.y2));
                    }
                    else
                    {
                        merged.Add(currentLine);
                        currentLine = nextLine;
                    }
                }
                merged.Add(currentLine);
            }

            return merged;
        }

        private List<CuttingLine> OptimizeCuttingOrder(List<CuttingLine> lines)
        {
            // 简单的最近邻算法优化切割顺序
            var optimized = new List<CuttingLine>();
            var remaining = new List<CuttingLine>(lines);

            if (remaining.Count == 0) return optimized;

            // 从(0,0)最近的线开始
            var current = remaining.OrderBy(l => Math.Sqrt(l.X1 * l.X1 + l.Y1 * l.Y1)).First();
            optimized.Add(current);
            remaining.Remove(current);

            // 贪心算法选择最近的下一条线
            while (remaining.Count > 0)
            {
                var lastEnd = (current.X2, current.Y2);
                current = remaining.OrderBy(l =>
                    Math.Min(
                        Math.Sqrt(Math.Pow(l.X1 - lastEnd.X2, 2) + Math.Pow(l.Y1 - lastEnd.Y2, 2)),
                        Math.Sqrt(Math.Pow(l.X2 - lastEnd.X2, 2) + Math.Pow(l.Y2 - lastEnd.Y2, 2))
                    )).First();

                optimized.Add(current);
                remaining.Remove(current);
            }

            return optimized;
        }

        private List<string> GenerateCutForLine(CuttingLine line)
        {
            var lines = new List<string>();

            // 移动到起始点
            lines.Add($"G00 X{line.X1} Y{line.Y1}");
            lines.Add("M03");  // 主轴启动
            lines.Add("M09");  // 冷却关闭

            // 切割到终点
            lines.Add($"G01 X{line.X2} Y{line.Y2}");
            lines.Add("M10");  // 冷却开启

            return lines;
        }

        private int GetMaterialTypeCode(string materialType)
        {
            // 根据材料类型返回对应的数字代码
            switch (materialType?.ToLower())
            {
                case "普通玻璃": return 1;
                case "钢化玻璃": return 2;
                case "夹层玻璃": return 3;
                case "中空玻璃": return 4;
                default: return 1;
            }
        }

        private string CleanString(string input)
        {
            if (string.IsNullOrEmpty(input)) return "";
            return input.Replace("_", "-").Replace(" ", "").Replace("\r", "").Replace("\n", "");
        }

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

        public GCodeResult GenerateOptimizedCuttingPath(OptimizationResult optimizationResult)
        {
            return GenerateGCode(optimizationResult);
        }
    }

    // 辅助类
    public class CuttingLine
    {
        public int X1 { get; set; }
        public int Y1 { get; set; }
        public int X2 { get; set; }
        public int Y2 { get; set; }
        public bool IsHorizontal { get; set; }
    }
}