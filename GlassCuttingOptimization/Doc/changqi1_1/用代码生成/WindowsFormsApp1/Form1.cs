using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp1
{
    // 单段G代码数据封装（起点，终点，可选圆弧参数I、J）
    public class GCodeSegment
    {
        public float StartX { get; set; }
        public float StartY { get; set; }
        public float EndX { get; set; }
        public float EndY { get; set; }
        public float? I { get; set; }
        public float? J { get; set; }

        public GCodeSegment(float sx, float sy, float ex, float ey, float? i = null, float? j = null)
        {
            StartX = sx;
            StartY = sy;
            EndX = ex;
            EndY = ey;
            I = i;
            J = j;
        }

        // 生成G代码文本，isFirstSegment决定是否加初始化代码
        public string ToGCode(bool isFirstSegment = false)
        {
            var sb = new StringBuilder();

            if (isFirstSegment)
            {
                sb.AppendLine("G21"); // 单位: 毫米
                sb.AppendLine("G90"); // 绝对坐标
                sb.AppendLine($"G0 X{StartX} Y{StartY}"); // 快速到起点
                sb.AppendLine("M03"); // 刀具开
            }

            // 如果有I和J，输出圆弧；否则输出直线
            if (I.HasValue && J.HasValue)
                sb.AppendLine($"G2 X{EndX} Y{EndY} I{I.Value} J{J.Value} F1000");
            else
                sb.AppendLine($"G1 X{EndX} Y{EndY} F1000");

            return sb.ToString();
        }
    }

    // 一组G代码（件单）封装
    public class GCodeJob
    {
        public List<GCodeSegment> Segments { get; set; } = new List<GCodeSegment>();

        public void AddSegment(GCodeSegment seg)
        {
            Segments.Add(seg);
        }

        // 生成完整G代码文本
        public string GenerateGCode()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < Segments.Count; i++)
            {
                sb.Append(Segments[i].ToGCode(isFirstSegment: i == 0));
            }
            sb.AppendLine("M05"); // 刀具关
            sb.AppendLine("G0 X0 Y0"); // 回原点
            return sb.ToString();
        }
    }

    public partial class Form1 : Form
    {
        // 件单数据（多段也可用）
        private GCodeJob job = new GCodeJob();

        public Form1()
        {
            InitializeComponent();
        }

        // 按钮点击事件：生成G代码
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. 读取输入框
                float sx = float.Parse(textBox1.Text); // 起点X
                float sy = float.Parse(textBox2.Text); // 起点Y
                float ex = float.Parse(textBox3.Text); // 终点X
                float ey = float.Parse(textBox4.Text); // 终点Y

                float? i = null, j = null;
                // 2. 判断I、J是否有输入（支持为空=直线）
                if (!string.IsNullOrWhiteSpace(textBox5.Text) && !string.IsNullOrWhiteSpace(textBox6.Text))
                {
                    i = float.Parse(textBox5.Text);
                    j = float.Parse(textBox6.Text);
                }

                // 3. 生成G代码段并加入件单
                var segment = new GCodeSegment(sx, sy, ex, ey, i, j);
                job = new GCodeJob(); // 如果只要单段就每次新建，后续多段可以删这一句
                job.AddSegment(segment);

                // 4. 导出G代码到文件
                string gcode = job.GenerateGCode();
                File.WriteAllText("output.nc", gcode);

                // 5. 提示信息
                MessageBox.Show("G代码已生成！\n\n" + gcode);
            }
            catch (Exception ex)
            {
                MessageBox.Show("输入错误：" + ex.Message);
            }
        }
    }
}
