﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GlassCuttingOptimization.Models.Dto;
using GlassCuttingOptimization.Models.Optimization;
using GlassCuttingOptimization.Services;
using System.IO;
using GlassCuttingOptimization.Common;

namespace GlassCuttingOptimization.Views.Controls
{
    public partial class OptimizationControlPanel : UserControl
    {
        private OptimizationEngine _optimizationEngine;
        private OptimizationVisualizationControl _visualizationControl;
        private OptimizationResult _currentResult;
        private GCodeGenerator _gCodeGenerator;

        public event EventHandler<OptimizationResult> OptimizationCompleted;

        public OptimizationControlPanel()
        {
            InitializeComponent();
            _optimizationEngine = new OptimizationEngine();
            _gCodeGenerator = new GCodeGenerator();
            InitializeAlgorithmComboBox();
        }
        // 预览G代码
        private void btnPreviewGCode_Click(object sender, EventArgs e)
        {
            if (_currentResult == null)
            {
                MessageBox.Show("请先进行排版优化", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                var gCodeResult = _gCodeGenerator.GenerateGCode(_currentResult);

                // 创建预览窗口
                var previewForm = new Form
                {
                    Text = "G代码预览",
                    Size = new Size(800, 600),
                    StartPosition = FormStartPosition.CenterParent
                };

                var textBox = new TextBox
                {
                    Multiline = true,
                    ScrollBars = ScrollBars.Both,
                    Dock = DockStyle.Fill,
                    Font = new Font("Consolas", 10),
                    ReadOnly = true,
                    Text = gCodeResult.GCode
                };

                previewForm.Controls.Add(textBox);
                previewForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"G代码预览失败:\n{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // 添加G代码生成按钮事件
        private void btnGenerateGCode_Click(object sender, EventArgs e)
        {

            if (_currentResult == null)
            {
                MessageBox.Show("请先进行排版优化", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                btnGenerateGCode.Enabled = false;
                lblStatus.Text = "正在生成G代码...";
                lblStatus.ForeColor = System.Drawing.Color.Blue;

                // 从配置文件读取保存路径
                var configPath = Path.Combine(Application.StartupPath, "config.ini");
                var iniFileHelper = new IniFileHelper(configPath);
                var savePath = iniFileHelper.Read("Settings", "SaveFile", "");

                // 如果没有配置路径，提示用户选择
                if (string.IsNullOrEmpty(savePath) || !Directory.Exists(savePath))
                {
                    using (var folderDialog = new FolderBrowserDialog())
                    {
                        folderDialog.Description = "选择保存G代码文件的根目录";
                        folderDialog.ShowNewFolderButton = true;

                        if (folderDialog.ShowDialog() == DialogResult.OK)
                        {
                            savePath = folderDialog.SelectedPath;
                            // 保存路径到配置文件
                            iniFileHelper.Write("Settings", "SaveFile", savePath);
                        }
                        else
                        {
                            return; // 用户取消了选择
                        }
                    }
                }

                // 生成时间戳作为基础名称和文件夹名称
                var baseName = DateTime.Now.ToString("yyyyMMddHHmmss");

                // 创建以时间戳命名的子文件夹
                var timestampFolder = Path.Combine(savePath, baseName);
                if (!Directory.Exists(timestampFolder))
                {
                    Directory.CreateDirectory(timestampFolder);
                }

                // 生成多个G文件
                var gCodeResults = _gCodeGenerator.GenerateGCodeFiles(_currentResult, baseName);

                // 保存文件到时间戳文件夹中
                _gCodeGenerator.SaveGCodeFiles(gCodeResults, timestampFolder);

                lblStatus.Text = $"G代码生成完成 - 共生成 {gCodeResults.Count} 个文件";
                lblStatus.ForeColor = System.Drawing.Color.Green;

                // 显示生成的文件列表
                var fileList = string.Join("\n", gCodeResults.Select(r => r.FileName));
                var result = MessageBox.Show($"G代码文件已保存到:\n{timestampFolder}\n\n生成的文件:\n{fileList}\n\n是否要打开文件夹？",
                    "生成完成", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start("explorer.exe", timestampFolder);
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"G代码生成失败: {ex.Message}";
                lblStatus.ForeColor = System.Drawing.Color.Red;
                MessageBox.Show($"G代码生成失败:\n{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnGenerateGCode.Enabled = true;
            }

        }
        public void SetVisualizationControl(OptimizationVisualizationControl visualizationControl)
        {
            _visualizationControl = visualizationControl;
        }

        public void RunOptimization(List<OrderDto> orders, List<OriginalDto> originalSheets)
        {
            try
            {
                btnOptimize.Enabled = false;
                btnReset.Enabled = false;
                lblStatus.Text = "正在优化...";
                lblStatus.ForeColor = System.Drawing.Color.Blue;

                var selectedAlgorithm = (OptimizationAlgorithm)cmbAlgorithm.SelectedValue;

                _currentResult = _optimizationEngine.Optimize(orders, originalSheets, selectedAlgorithm);

                UpdateResultDisplay();
                _visualizationControl?.SetOptimizationResult(_currentResult);

                lblStatus.Text = $"优化完成 - 用时: {_currentResult.OptimizationTime.TotalMilliseconds:F0}ms";
                lblStatus.ForeColor = System.Drawing.Color.Green;

                btnReset.Enabled = true;
                OptimizationCompleted?.Invoke(this, _currentResult);
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"优化失败: {ex.Message}";
                lblStatus.ForeColor = System.Drawing.Color.Red;
                btnReset.Enabled = false;
            }
            finally
            {
                btnOptimize.Enabled = true;
            }
        }

        private void InitializeAlgorithmComboBox()
        {
            var algorithms = new List<AlgorithmItem>();

            foreach (OptimizationAlgorithm algorithm in Enum.GetValues(typeof(OptimizationAlgorithm)))
            {
                var description = GetEnumDescription(algorithm);
                algorithms.Add(new AlgorithmItem { Value = algorithm, Text = description });
            }

            cmbAlgorithm.DataSource = algorithms;
            cmbAlgorithm.DisplayMember = "Text";
            cmbAlgorithm.ValueMember = "Value";
            cmbAlgorithm.SelectedIndex = 0;
        }

        private string GetEnumDescription(Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            return attribute?.Description ?? value.ToString();
        }

        private void UpdateResultDisplay()
        {
            if (_currentResult == null) return;

            lblTotalSheets.Text = $"总板材数: {_currentResult.Sheets.Count}";
            lblOverallUtilization.Text = $"整体利用率: {_currentResult.OverallUtilizationRate:F1}%";
            lblAlgorithmUsed.Text = $"算法: {_currentResult.AlgorithmUsed}";

            // 更新板材选择下拉框
            cmbSheetSelector.Items.Clear();
            for (int i = 0; i < _currentResult.Sheets.Count; i++)
            {
                var sheet = _currentResult.Sheets[i];
                cmbSheetSelector.Items.Add($"板材 {i + 1} (利用率: {sheet.UtilizationRate:F1}%)");
            }

            if (cmbSheetSelector.Items.Count > 0)
            {
                cmbSheetSelector.SelectedIndex = 0;
            }
        }

        private void btnOptimize_Click(object sender, EventArgs e)
        {
            // 这个方法将被外部调用
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (_currentResult == null) return;

            try
            {
                // 恢复到原始状态
                _currentResult.RestoreToOriginal();

                // 刷新显示
                _visualizationControl?.SetOptimizationResult(_currentResult);
                UpdateResultDisplay();

                lblStatus.Text = "已重置到初始排版状态";
                lblStatus.ForeColor = System.Drawing.Color.Green;
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"重置失败: {ex.Message}";
                lblStatus.ForeColor = System.Drawing.Color.Red;
            }
        }

        private void btnRotate_Click(object sender, EventArgs e)
        {
            _visualizationControl?.RotateSelectedPiece();
        }

        private void cmbSheetSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSheetSelector.SelectedIndex >= 0)
            {
                _visualizationControl?.ShowSheet(cmbSheetSelector.SelectedIndex);
            }
        }

        private void btnZoomFit_Click(object sender, EventArgs e)
        {
            _visualizationControl?.FitToWindow();
        }

        private void btnZoomIn_Click(object sender, EventArgs e)
        {
            _visualizationControl?.ZoomIn();
        }

        private void btnZoomOut_Click(object sender, EventArgs e)
        {
            _visualizationControl?.ZoomOut();
        }

        private void trackBarZoom_ValueChanged(object sender, EventArgs e)
        {
            float zoomLevel = trackBarZoom.Value / 100.0f;
            _visualizationControl?.SetZoomLevel(zoomLevel);
        }

    }

    // 辅助类
    public class AlgorithmItem
    {
        public OptimizationAlgorithm Value { get; set; }
        public string Text { get; set; }
    }
}
