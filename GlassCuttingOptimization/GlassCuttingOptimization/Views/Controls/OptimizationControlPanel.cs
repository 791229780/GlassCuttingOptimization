using System;
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

namespace GlassCuttingOptimization.Views.Controls
{
    public partial class OptimizationControlPanel : UserControl
    {
        private OptimizationEngine _optimizationEngine;
        private OptimizationVisualizationControl _visualizationControl;
        private OptimizationResult _currentResult;

        public event EventHandler<OptimizationResult> OptimizationCompleted;

        public OptimizationControlPanel()
        {
            InitializeComponent();
            _optimizationEngine = new OptimizationEngine();
            InitializeAlgorithmComboBox();
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
