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
using System.IO;

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
                // 获取项目名称用于预览
                string projectName = PromptForProjectName();
                if (string.IsNullOrEmpty(projectName))
                {
                    return;
                }

                var gCodeResult = _gCodeGenerator.GenerateGCode(_currentResult, projectName);

                // 如果是多文件，显示文件选择器
                if (gCodeResult.IsMultiFile)
                {
                    ShowMultiFilePreview(gCodeResult);
                }
                else
                {
                    // 单文件预览（兼容性）
                    ShowSingleFilePreview(gCodeResult.GCode);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"G代码预览失败:\n{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowSingleFilePreview(string gCode)
        {
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
                Text = gCode
            };

            previewForm.Controls.Add(textBox);
            previewForm.ShowDialog();
        }

        private void ShowMultiFilePreview(GCodeResult gCodeResult)
        {
            // 创建多文件预览窗口
            var previewForm = new Form
            {
                Text = "G代码预览 (多文件)",
                Size = new Size(900, 700),
                StartPosition = FormStartPosition.CenterParent
            };

            // 左侧文件列表
            var splitContainer = new SplitContainer
            {
                Dock = DockStyle.Fill,
                SplitterDistance = 250
            };

            var fileListBox = new ListBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Microsoft YaHei UI", 9)
            };

            // 添加文件到列表
            foreach (var fileInfo in gCodeResult.Files)
            {
                fileListBox.Items.Add($"{fileInfo.FileName} ({fileInfo.PieceCount}片)");
            }

            // 右侧内容显示
            var textBox = new TextBox
            {
                Multiline = true,
                ScrollBars = ScrollBars.Both,
                Dock = DockStyle.Fill,
                Font = new Font("Consolas", 9),
                ReadOnly = true
            };

            // 文件选择事件
            fileListBox.SelectedIndexChanged += (s, e) =>
            {
                if (fileListBox.SelectedIndex >= 0)
                {
                    var selectedFile = gCodeResult.Files[fileListBox.SelectedIndex];
                    textBox.Text = selectedFile.GCode;
                }
            };

            // 默认选择第一个文件
            if (fileListBox.Items.Count > 0)
            {
                fileListBox.SelectedIndex = 0;
            }

            splitContainer.Panel1.Controls.Add(fileListBox);
            splitContainer.Panel2.Controls.Add(textBox);
            previewForm.Controls.Add(splitContainer);

            previewForm.ShowDialog();
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

                // 弹出项目名称输入对话框
                string projectName = PromptForProjectName();
                if (string.IsNullOrEmpty(projectName))
                {
                    lblStatus.Text = "G代码生成已取消";
                    lblStatus.ForeColor = System.Drawing.Color.Orange;
                    return;
                }

                var gCodeResult = _gCodeGenerator.GenerateGCode(_currentResult, projectName);

                // 如果是多文件，使用文件夹选择对话框
                if (gCodeResult.IsMultiFile)
                {
                    using (var folderDialog = new FolderBrowserDialog())
                    {
                        folderDialog.Description = "请选择G代码文件保存文件夹";
                        folderDialog.ShowNewFolderButton = true;

                        if (folderDialog.ShowDialog() == DialogResult.OK)
                        {
                            var savedFiles = _gCodeGenerator.SaveGCodeToFiles(gCodeResult, folderDialog.SelectedPath);

                            lblStatus.Text = $"G代码生成完成 - 共生成 {savedFiles.Count} 个文件 - 总玻璃片数: {gCodeResult.TotalPieces}";
                            lblStatus.ForeColor = System.Drawing.Color.Green;

                            // 显示生成的文件列表
                            ShowGeneratedFilesList(savedFiles, gCodeResult);
                        }
                    }
                }
                else
                {
                    // 兼容原有的单文件模式
                    using (var saveDialog = new SaveFileDialog())
                    {
                        saveDialog.Filter = "G代码文件 (*.g)|*.g|所有文件 (*.*)|*.*";
                        saveDialog.FileName = gCodeResult.FileName;
                        saveDialog.Title = "保存G代码文件";

                        if (saveDialog.ShowDialog() == DialogResult.OK)
                        {
                            _gCodeGenerator.SaveGCodeToFile(gCodeResult, saveDialog.FileName);

                            lblStatus.Text = $"G代码生成完成 - 文件: {Path.GetFileName(saveDialog.FileName)} - 玻璃片数: {gCodeResult.TotalPieces}";
                            lblStatus.ForeColor = System.Drawing.Color.Green;

                            // 询问是否打开文件
                            var result = MessageBox.Show($"G代码文件已保存到:\n{saveDialog.FileName}\n\n是否要打开查看？",
                                "生成完成", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                            if (result == DialogResult.Yes)
                            {
                                System.Diagnostics.Process.Start("notepad.exe", saveDialog.FileName);
                            }
                        }
                    }
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

        private string PromptForProjectName()
        {
            string projectName = "M0000031"; // 默认值

            // 创建输入对话框
            using (var form = new Form())
            {
                form.Text = "项目名称设置";
                form.Size = new Size(400, 150);
                form.StartPosition = FormStartPosition.CenterParent;
                form.FormBorderStyle = FormBorderStyle.FixedDialog;
                form.MaximizeBox = false;
                form.MinimizeBox = false;

                var label = new Label
                {
                    Text = "请输入项目名称:",
                    Location = new Point(20, 20),
                    Size = new Size(120, 23)
                };

                var textBox = new TextBox
                {
                    Text = projectName,
                    Location = new Point(150, 20),
                    Size = new Size(200, 23)
                };

                var btnOK = new Button
                {
                    Text = "确定",
                    DialogResult = DialogResult.OK,
                    Location = new Point(200, 60),
                    Size = new Size(75, 23)
                };

                var btnCancel = new Button
                {
                    Text = "取消",
                    DialogResult = DialogResult.Cancel,
                    Location = new Point(285, 60),
                    Size = new Size(75, 23)
                };

                form.Controls.AddRange(new Control[] { label, textBox, btnOK, btnCancel });
                form.AcceptButton = btnOK;
                form.CancelButton = btnCancel;

                if (form.ShowDialog() == DialogResult.OK)
                {
                    return textBox.Text.Trim();
                }
            }

            return null;
        }

        private void ShowGeneratedFilesList(List<string> savedFiles, GCodeResult gCodeResult)
        {
            // 创建文件列表显示窗口
            using (var form = new Form())
            {
                form.Text = "G代码文件生成完成";
                form.Size = new Size(600, 400);
                form.StartPosition = FormStartPosition.CenterParent;

                var listBox = new ListBox
                {
                    Dock = DockStyle.Fill,
                    Font = new Font("Consolas", 9)
                };

                // 添加汇总信息
                listBox.Items.Add($"项目: {Path.GetFileNameWithoutExtension(savedFiles.FirstOrDefault())?.Split('_')[0]}");
                listBox.Items.Add($"生成时间: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                listBox.Items.Add($"总文件数: {savedFiles.Count}");
                listBox.Items.Add($"总玻璃片数: {gCodeResult.TotalPieces}");
                listBox.Items.Add($"生成耗时: {gCodeResult.GenerationTime.TotalMilliseconds:F0}ms");
                listBox.Items.Add("");
                listBox.Items.Add("生成的文件列表:");
                listBox.Items.Add(new string('-', 50));

                // 添加文件列表
                for (int i = 0; i < savedFiles.Count; i++)
                {
                    var filePath = savedFiles[i];
                    var fileInfo = gCodeResult.Files[i];
                    listBox.Items.Add($"{Path.GetFileName(filePath)} (板材{fileInfo.SheetIndex + 1}, {fileInfo.PieceCount}个玻璃片, {fileInfo.SheetWidth}×{fileInfo.SheetHeight}mm)");
                }

                var panel = new Panel
                {
                    Dock = DockStyle.Bottom,
                    Height = 40
                };

                var btnOpenFolder = new Button
                {
                    Text = "打开文件夹",
                    Location = new Point(10, 8),
                    Size = new Size(100, 25)
                };
                btnOpenFolder.Click += (s, e) =>
                {
                    var folderPath = Path.GetDirectoryName(savedFiles.FirstOrDefault());
                    if (!string.IsNullOrEmpty(folderPath))
                    {
                        System.Diagnostics.Process.Start("explorer.exe", folderPath);
                    }
                };

                var btnClose = new Button
                {
                    Text = "关闭",
                    Location = new Point(120, 8),
                    Size = new Size(75, 25),
                    DialogResult = DialogResult.OK
                };

                panel.Controls.AddRange(new Control[] { btnOpenFolder, btnClose });
                form.Controls.AddRange(new Control[] { listBox, panel });
                form.AcceptButton = btnClose;

                form.ShowDialog();
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
