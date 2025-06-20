using AntdUI;
using GlassCuttingOptimization.Enums;
using GlassCuttingOptimization.Models;
using GlassCuttingOptimization.Models.Dto;
using GlassCuttingOptimization.Models.Model;
using GlassCuttingOptimization.Models.UIItem;
using GlassCuttingOptimization.Models.UIModels;
using GlassCuttingOptimization.Utils;
using GlassCuttingOptimization.Views.GlassView;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GlassCuttingOptimization.Views.OrderView
{
    public partial class OrderForm : AntdUI.Window
    {
        NoteModel _noteModel;
        private GlassPanel glassPanel;
        SqlSugarHelper _db;
        List<MaterialDto> _listMaterial;

        List<OrderDto> _listOrder;
        OrderDto _uiOrder;
        public OrderForm()
        {
            _noteModel = new NoteModel();
            _db = new SqlSugarHelper();
            _listMaterial = new List<MaterialDto>();
            _listOrder = new List<OrderDto>();
            _uiOrder = new OrderDto();
            InitializeComponent();

            InitData();
            
            LoadMenu();
            
            InitTableColumns();
            
            //绑定事件
            BindEventHandler();

            InitializeGlassPanel();

        }

        private void InitializeGlassPanel()
        {
            glassPanel = new GlassPanel();
            glassPanel.Dock = DockStyle.Fill;  // 填充整个panel3
            this.panel3.Controls.Add(glassPanel);
            glassPanel.SetGlassSize(1000, 600);
        }

        private void ChangeSize()
        {

            var width = inputX.Value;

            var height = inputY.Value;

            if (cboBottom.Checked)
            {
                width += inputBottom.Value;
            }
            if (cboTop.Checked)
            {
                width += inputTop.Value;
            }
            if (cboLeft.Checked)
            {
                height += inputLeft.Value;
            }
            if (cboRight.Checked)
            {
                height += inputRight.Value;
            }
            ChangeGlassSize(Convert.ToInt16(width), Convert.ToInt16(height));
        }

        private List<int> GetDimensions()
        {
            var list = new List<int>();
            var width = inputX.Value;

            var height = inputY.Value;

            if (cboBottom.Checked)
            {
                width += inputBottom.Value;
            }
            if (cboTop.Checked)
            {
                width += inputTop.Value;
            }
            if (cboLeft.Checked)
            {
                height += inputLeft.Value;
            }
            if (cboRight.Checked)
            {
                height += inputRight.Value;
            }
            list.AddRange(new int[] { Convert.ToInt16(width), Convert.ToInt16(height) });

            return list;
        }

        private void ChangeGlassSize(int width, int height)
        {
            glassPanel.SetGlassSize(width, height);
        }
        private void BindEventHandler()
        {
            btnNote.Click += BtnNote_Click;
            inputX.Leave += InputY_Leave;
            inputY.Leave += InputY_Leave;
            inputBottom.Leave += InputY_Leave;
            inputTop.Leave += InputY_Leave;
            inputRight.Leave += InputY_Leave;
            inputLeft.Leave += InputY_Leave;
            btnNewLine.Click += BtnNewLine_Click;
            selectMaterial.SelectedIndexChanged += SelectMaterial_SelectedIndexChanged;
            table_base.CellClick += Table_base_CellClick;

            inputNumber.Leave += InputNumber_Leave;
            inputClient.Leave += InputNumber_Leave;
            inputOrder.Leave += InputNumber_Leave;
            dateShipment.Leave += InputNumber_Leave;
            selectRotation.Leave += InputNumber_Leave;
            inputNote.Leave += InputNumber_Leave;

            cboBottom.CheckedChanged += InputNumber_Leave;
            cboTop.CheckedChanged += InputNumber_Leave;
            cboLeft.CheckedChanged += InputNumber_Leave;
            cboRight.CheckedChanged += InputNumber_Leave;


            inputBottom.Leave += InputNumber_Leave;
            inputTop.Leave += InputNumber_Leave;
            inputLeft.Leave += InputNumber_Leave;
            inputRight.Leave += InputNumber_Leave;
            inputX.Leave += InputNumber_Leave;
            inputY.Leave += InputNumber_Leave;

            btnDeleLine.Click += BtnDeleLine_Click;
        }

        private void BtnDeleLine_Click(object sender, EventArgs e)
        {

            DeleLine();
          
        }

        private void DeleLine()
        {
            List<int> list = new List<int>(); ;
            try
            {

                btnDeleLine.Enabled = false;
                var Source = table_base.DataSource;
                if (Source is AntList<UIOrderDto> dtoList)
                {


                    foreach (var item in dtoList)
                    {
                        if (item.Selected)
                        {
                            list.Add(item.orderID);
                        }

                    }
                    if (list.Count == 0)
                    {
                        AntdUI.Notification.success(this, "提示", $"未选中有效数据!", autoClose: 3, align: TAlignFrom.TR);

                    }

                    var result = AntdUI.Modal.open(this, "删除警告！", "删除后无法恢复！", TType.Warn);
                    if (result != DialogResult.OK)
                    {
                        return;
                    }
                    else
                    {
                        int count = 0;
                        foreach (var item in list)
                        {
                            _listOrder.RemoveAll(o => o.OrderID == item);
                            count++;
                        }

                        if (count > 0)
                        {


                            AntdUI.Notification.success(
                                 this,
                                 "提示",
                                 $"删除成功！",
                                 autoClose: 3,
                                 align: TAlignFrom.TR
                             );
                        }

                        tableBinding(_listOrder);

                        if (_listOrder.Count > 0)
                        {
                            table_base.SelectedIndex = 1;

                            table_base.SetRowEnable(1);
                        }
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {

                btnDeleLine.Enabled = true;
            }
        }

        private void InputNumber_Leave(object sender, EventArgs e)
        {
            if (_listOrder.Count>0)
            {
                var index = _listOrder.FindIndex(c => c.OrderID == _uiOrder.OrderID);
                if (index >= 0)
                {
                    _listOrder[index].Number = Convert.ToInt32( inputNumber.Value);

                    _listOrder[index].Customer = inputClient.Text;

                    _listOrder[index].Order = inputOrder.Text;

                    _listOrder[index].SendDate = dateShipment.Value.ToString();

                    _listOrder[index].Rotation = selectRotation.Text;

                    _listOrder[index].Note = inputNote.Text;

                    _listOrder[index].BottomCbo = cboBottom.Checked;

                    _listOrder[index].TopCbo = cboTop.Checked;

                    _listOrder[index].LeftCbo = cboLeft.Checked;

                    _listOrder[index].RightCbo = cboRight.Checked;


                    _listOrder[index].BottomNumber = Convert.ToInt32(inputBottom.Value);

                    _listOrder[index].TopNumber = Convert.ToInt32(inputTop.Value);

                    _listOrder[index].LeftNumber = Convert.ToInt32(inputLeft.Value);

                    _listOrder[index].RightNumber = Convert.ToInt32(inputRight.Value);

                    var x = Convert.ToInt32(inputX.Value);
                    var y = Convert.ToInt32(inputY.Value);
                    if (_listOrder[index].BottomCbo)
                    {
                        x += _listOrder[index].BottomNumber;
                    }
                    if (_listOrder[index].TopCbo)
                    {
                        x += _listOrder[index].TopNumber;
                    }

                    if (_listOrder[index].LeftCbo)
                    {
                        y += _listOrder[index].LeftNumber;
                    }
                    if (_listOrder[index].RightCbo)
                    {
                        x += _listOrder[index].RightNumber;
                    }
                    _listOrder[index].X = x;

                    _listOrder[index].Y = y;

                }
                tableBinding(_listOrder);
            }
        }

        private void Table_base_CellClick(object sender, TableClickEventArgs e)
        {
            var record = e.Record;
            if (record is UIOrderDto user)
            {
                var dto =  _listOrder.Where(c => c.OrderID == user.OrderID).First();
                _uiOrder = dto;
                inputNumber.Value = dto.Number;
                inputClient.Text = dto.Customer;
                inputOrder.Text = dto.Order;
                inputRriority.Value = dto.Rriority;
                dateShipment.Value = DateTime.Parse(dto.SendDate);
                inputNote.Text = dto.Note;
                var x = dto.X;
                var y = dto.Y;
                if (dto.BottomCbo)
                {
                    x -= dto.BottomNumber;

                   
                }
                if (dto.TopCbo)
                {
                    x -= dto.TopNumber;
                }
                if (dto.RightCbo)
                {
                    y -= dto.RightNumber;
                }
                if (dto.LeftCbo)
                {
                    y -= dto.LeftNumber;
                }
                inputX.Value = x;
                inputY.Value = y;
                inputBottom.Value = dto.BottomNumber;
                inputTop.Value = dto.TopNumber;

                inputRight.Value = dto.RightNumber;
                inputLeft.Value = dto.LeftNumber;
                cboBottom.Checked = dto.BottomCbo;
                cboTop.Checked = dto.TopCbo;
                cboRight.Checked = dto.RightCbo;
                cboLeft.Checked = dto.LeftCbo;
                selectMaterial.SelectedIndex = dto.Materials.Index;
                selectRotation.Text = dto.Rotation;
            }
        }

        private void BtnNewLine_Click(object sender, EventArgs e)
        {
            NewLine();
     
        }

        private void NewLine()
        {
            btnOptimization.Enabled = true;
            btnDeleLine.Enabled = true;
            if (string.IsNullOrEmpty(inputName.Text))
            {
                inputName.Text = UniqueCodeGenerator.Generate("Mr.");
            }


            var dto = new OrderDto();
            dto.OrderID = _listOrder.Count;
            var list = GetDimensions();
            dto.X = list[0];
            dto.Y = list[1];
            dto.Customer = inputClient.Text;
            dto.Order = inputOrder.Text;
            dto.Note = inputNote.Text;
            dto.SendDate = dateShipment.Value.ToString();
            dto.Rotation = selectRotation.Text;
            dto.Rriority = Convert.ToInt32(inputRriority.Value);
            dto.Number = Convert.ToInt32(inputNumber.Value);
            dto.modelNote = _noteModel;
            dto.BottomNumber = Convert.ToInt32(inputBottom.Value);
            dto.BottomCbo = cboBottom.Checked;
            dto.TopNumber = Convert.ToInt32(inputTop.Value);
            dto.TopCbo = cboTop.Checked;
            dto.RightNumber = Convert.ToInt32(inputRight.Value);
            dto.RightCbo = cboRight.Checked;
            dto.LeftNumber = Convert.ToInt32(inputLeft.Value);
            dto.LeftCbo = cboLeft.Checked;
            if (selectMaterial.Items[0] is ComboboxItem cboItem)
            {
                dto.Materials = cboItem;
            }
            selectMaterial.SelectedIndex = 0;
            _listOrder.Add(dto);
            tableBinding(_listOrder);
            _uiOrder = dto;
            table_base.SelectedIndex = _listOrder.Count;
        }

        private void tableBinding(List<OrderDto> dtoList)
        {
            table_base.DataSource = null;
            var index = 1;
            var antList = new AntList<UIOrderDto>();
            foreach (var dto in dtoList)
            {

                antList.Add(new UIOrderDto
                {
                    Index = index,
                    X = dto.X,
                    Y = dto.Y,
                    Number = dto.Number,
                    Customer = dto.Customer,
                    Order = dto.Order,
                    Note = dto.Note,
                    Riority = dto.Rriority,
                    modelNote = dto.modelNote,
                    OrderID = dto.OrderID,
                    SendDate = dto.SendDate,
                    Rotation = dto.Rotation,

                });
                index++;
            }
            table_base.Binding(antList);

        }


        private void SelectMaterial_SelectedIndexChanged(object sender, IntEventArgs e)
        {
            if (selectMaterial.SelectedValue is ComboboxItem dto)
            {
                inputMaterialType.Text = dto.Value.Type;

                inputThickness.Text = dto.Value.Grinding;

                inputDescription.Text = dto.Value.Description;
            }

    
            var index = _listOrder.FindIndex(c => c.OrderID == _uiOrder.OrderID);
            if (index >= 0)
            {
                if (selectMaterial.SelectedValue is ComboboxItem item)
                {
                    _listOrder[index].Materials = item;
                }
            }


        }

        private void InputY_Leave(object sender, EventArgs e)
        {
            ChangeSize();
        }


        private void BtnNote_Click(object sender, EventArgs e)
        {
            var form = new NoteForm(_noteModel);
            form.OnModel_Event += Form_OnModel_Event;
            form.ShowDialog();
        }

        private void Form_OnModel_Event(object sender, Tuple<Models.Model.NoteModel> e)
        {
            _noteModel = e.Item1;
            inputNote.Text = "";
        }

        private void InitData()
        {
            _listMaterial = _db.Query<MaterialDto>();
            this.windowBar.Text = "插入切割订单";
            this.windowBar.DividerShow = true;
            this.windowBar.DividerThickness = 2;
            this.windowBar.ShowIcon = true;
            this.windowBar.ShowButton = true;
            this.StartPosition = FormStartPosition.CenterScreen;

            this.menu.Mode = TMenuMode.Horizontal;
            menu.SelectChanged += Menu_SelectChanged;
            dateShipment.Value = DateTime.Now;
            selectRotation.Items.AddRange(new object[] { "NO", "YES" });
            selectRotation.SelectedIndex = 0;

            selectMaterial.Items.Clear();

            var items = _listMaterial.Select((x, i) => new ComboboxItem
            {
                Text = x.Name,
                Value = x,
                Index = i
            }).ToList();
            selectMaterial.Items.AddRange(items.ToArray());
            btnNewLine.Shape = TShape.Round;
            btnOptimization.Shape = TShape.Round;
            btnDeleLine.Shape = TShape.Round;
            btnOptimization.Enabled = false;
            btnDeleLine.Enabled = false;
        }
        
        private void Menu_SelectChanged(object sender, MenuSelectEventArgs e)
        {
            var name = e.Value.Tag;
            switch (name)
            {
                case GlassCuttingOptimization.Enums.NavigationMenu.ExitSystem:
                    {
                        this.Close();
                    }
                    break;
                case NavigationMenu.AddOrder:
                    {
                        NewLine();
                    }
                    break;
                case NavigationMenu.DelOrder:
                    {
                        DeleLine();
                    }
                    break;
                case NavigationMenu.Optimization:
                    {

                    }
                    break;
        
                default:
                    break;
            }
        }
        
        private void LoadMenu(string filter = "")
        {
            menu.Items.Clear();

            foreach (var rootItem in DataOrderUtil.MenuItems)
            {
                var rootKey = rootItem.Key.ToLower();
                var rootMenu = new AntdUI.MenuItem { Text = rootItem.Key };
                bool rootVisible = false; // 用于标记是否显示根节点


                foreach (var item in rootItem.Value)
                {
                    var childText = item.Text.ToLower();

                    // 如果子节点包含搜索文本
                    if (childText.Contains(filter))
                    {
                        var menuItem = new AntdUI.MenuItem
                        {
                            Text = item.Text,
                            IconSvg = item.IconSvg,
                            Tag = item.Tag,
                        };
                        rootMenu.Sub.Add(menuItem);
                        rootVisible = true; // 如果有子节点包含，则显示根节点
                    }
                }

                // 如果根节点包含搜索文本，或有可见的子节点，则显示根节点
                if (rootKey.Contains(filter) || rootVisible)
                {
                    menu.Items.Add(rootMenu);
                }
            }
        }

        private void InitTableColumns()
        {
            table_base.Columns = new ColumnCollection() {
                 new AntdUI.ColumnCheck("Selected"){Fixed = true,Width="50"},
                 new AntdUI.Column("Index", "序号", ColumnAlign.Center){Width="80" },
                      new AntdUI.Column("Number", "数量", ColumnAlign.Center),
                 new AntdUI.Column("X", "X尺寸", ColumnAlign.Center),
                 new AntdUI.Column("Y", "Y尺寸", ColumnAlign.Center),
                 new AntdUI.Column("Customer", "客户", ColumnAlign.Center),
                 new AntdUI.Column("Order", "订单", ColumnAlign.Center),
                            new AntdUI.Column("SendDate", "发货日期", ColumnAlign.Center),
                                       new AntdUI.Column("Rotation", "是否旋转", ColumnAlign.Center),
                 new AntdUI.Column("Note", "注释", ColumnAlign.Center),
                 new AntdUI.Column("Riority", "优先级", ColumnAlign.Center),
              };
            table_base.VisibleHeader = true;
            table_base.AutoSizeColumnsMode = ColumnsMode.Fill;
            table_base.EmptyHeader = true;
        }

    }
}
