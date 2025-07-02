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

        private List<string> GenerateSheetGCode(OptimizedSheet sheet)
        {
            var lines = new List<string>();

            // 板材头部信息
            lines.Add("G54");
            lines.Add($"P3000={sheet.Width}");      // 原片X尺寸
            lines.Add($"P3001={sheet.Height}");     // 原片Y尺寸
            lines.Add($"P3002={sheet.X1}");        // X1修边
            lines.Add($"P3003={sheet.X2}");        // X2修边
            lines.Add($"P3004={sheet.Y1}");        // Y1修边
            lines.Add($"P3005={sheet.Y2}");        // Y2修边
            lines.Add($"P3007={sheet.MaterialType}"); // 材料类型
            lines.Add($"P3011={sheet.Thickness}");    // 厚度(Grinding)

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

            // 生成每个玻璃片的G代码
            foreach (var piece in sortedPieces)
            {
                var gcodeLine = GeneratePieceGCode(piece);
                lines.Add(gcodeLine);
            }

            // 板材结束标记
            lines.Add("M30");
            lines.Add(""); // 空行分隔

            return lines;
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
            return $"P{parameterNumber}={string.Join("_", parts)}";
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
            // 这里可以实现更复杂的切割路径优化算法
            // 比如最短路径、减少刀具抬起次数等
            return GenerateGCode(optimizationResult);
        }
    }
}