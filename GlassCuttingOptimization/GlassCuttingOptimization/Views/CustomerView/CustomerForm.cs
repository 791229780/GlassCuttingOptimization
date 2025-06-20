using AntdUI;
using ExcelDataReader;
using GlassCuttingOptimization.Models;
using GlassCuttingOptimization.Models.UIModels;
using GlassCuttingOptimization.Utils;
using MathNet.Numerics;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace GlassCuttingOptimization.Views.CustomerView
{
    public partial class CustomerForm : AntdUI.Window
    {
        SelectMultiple selectMultiple = new SelectMultiple();
        SqlSugarHelper _db;
        DataTableCollection tableCollection;
        public CustomerForm()
        {
            _db = new SqlSugarHelper();
            InitializeComponent();

            InitData();
            InitTableColumns();
            LoadPagination();
            // 绑定事件
            BindEventHandler();
            Query();
        }
        private async void InitData()
        {
            inputPage.Minimum = 0;
            selectMultiple.Items.AddRange(new object[] { "10", "20", "30", "40", "50", "60", "70", "80", "90", "100" });
        }
        private void BindEventHandler()
        {
            btnDelete.Click += BtnDelete_Click;
            table_base.CellButtonClick += Table_base_CellButtonClick;
            pagination.ValueChanged += Pagination_ValueChanged;
            btnImport.Click += BtnImport_Click;
            btnQuery.Click += BtnQuery_Click;
            btnAdd.Click += BtnAdd_Click;
            table_base.EmptyHeader = true;
            table_base.AutoSizeColumnsMode = ColumnsMode.Fill;
            table_base.EmptyImage = Properties.Resources.编组;
            selectMultiple.SelectedValueChanged += SelectMultiple_SelectedValueChanged;
            //pagination.ValueChanged += Pagination_ValueChanged;
            btnJump.Click += BtnJump_Click;
            this.Text = "客户信息";
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
           List<long> list = new List<long>(); ;
            try
            {

                btnDelete.Enabled = false;
                var Source = table_base.DataSource;
                if (Source is AntList<UICustomerInfoDto> dtoList)
                {


                    foreach (var item in dtoList)
                    {
                        if (item.Selected)
                        {
                            list.Add(item.ID);
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
                            count += _db.Delete<CustomerInfoDto>(item);
                        }

                        if (count > 0)
                        {

                            InitDataTable();
                            AntdUI.Notification.success(
                                 this,
                                 "提示",
                                 $"删除成功！",
                                 autoClose: 3,
                                 align: TAlignFrom.TR
                             );
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

                btnDelete.Enabled = true;
            }
        }

        private void Table_base_CellButtonClick(object sender, TableButtonEventArgs e)
        {
            var buttontext = e.Btn.Text;

            var dto = (UICustomerInfoDto)e.Record;
            switch (buttontext)
            {
                case "编辑":
                    {
                        var form = new CustomerEditControl(this, dto, _db, true) { Size = new System.Drawing.Size(600, 300) };
                        form.OnSuccess_Event -= Form_OnSuccess_Event;
                        form.OnSuccess_Event += Form_OnSuccess_Event;
                        AntdUI.Drawer.open(new AntdUI.Drawer.Config(this, form)
                        {
                            OnLoad = () =>
                            {
                                AntdUI.Message.info(this, "进入编辑", autoClose: 1);
                            },
                            OnClose = () =>
                            {
                                AntdUI.Message.info(this, "结束编辑", autoClose: 1);
                            }
                        });
                    }
                    break;
                case "删除":
                    {
                        var result = AntdUI.Modal.open(this, "删除警告！", "删除后无法恢复！", TType.Warn);
                        if (result != DialogResult.OK)
                        {
                            return;
                        }
                        else
                        {
                            var count = _db.Delete<CustomerInfoDto>(dto.ID);
                            if (count > 0)
                            {

                                InitDataTable();
                                AntdUI.Notification.success(
                                             this,
                                             "提示",
                                             "删除成功！",
                                             autoClose: 3,
                                             align: TAlignFrom.TR
                                         );
                            }
                            else
                            {
                                AntdUI.Notification.error(
                                             this,
                                             "提示",
                                             "删除失败！",
                                             autoClose: 3,
                                             align: TAlignFrom.TR
                                         );
                            }
                        }

                    }
                    break;
            }
        }

        private void Pagination_ValueChanged(object sender, PagePageEventArgs e)
        {
            try
            {
                var items = _db.QueryByPage(e.Current, e.PageSize, ExpressionData()).ToList();

                var lists = new AntList<UICustomerInfoDto>();
                var index = 1;
                foreach (var item in items)
                {
                    var dto = new UICustomerInfoDto();
                    dto.Index = index;
                    dto.Customer_code = item.customer_code;
                    dto.Customer_name = item.company_name;
                    dto.Customer_person = item.contact_person;
                    dto.Customer_type = item.customer_type;
                    dto.Phone = item.phone;
                    dto.Mobile = item.mobile;

                    dto.Email = item.email;
                    dto.Province = item.province;
                    dto.City = item.city;
                    dto.District = item.district;
                    dto.Address = item.address;
                    dto.Remarks = item.remarks;
                    lists.Add(dto);
                }
                tableBinding(lists);
            }
            catch (Exception)
            {

                throw;
            }
        
        }
        private Expression<Func<CustomerInfoDto, bool>> ExpressionData()
        {
            try
            {
                var expression = new ExpressionBuilder<CustomerInfoDto>();

                if (!string.IsNullOrEmpty(inputcustomer_name.Text))
                {
                    expression.And(x => x.company_name.Contains(inputcustomer_name.Text));
                }
                if (!string.IsNullOrEmpty(inputcustomer_code.Text))
                {
                    expression.And(x => x.company_name.Contains(inputcustomer_code.Text));
                }
                var item = expression.Build();
                return item;
            }
            catch (Exception)
            {

                throw;
            }
 
        }
        private void BtnImport_Click(object sender, EventArgs e)
        {

            try
            {

           
            table_base.DataSource = null;
            List<CustomerInfoDto> dtoList = new List<CustomerInfoDto>();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            string filepath = "";

            DataTable table = new DataTable();
            DataTable tablefalse = new DataTable();
            OpenFileDialog dia = new OpenFileDialog();  //显示选择文件对话框
            dia.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop); //用户桌面
            dia.Filter = "Excel文件(*.xls,xlsx)|*.xls;*.xlsx"; //所有excel文件
            dia.FilterIndex = 2;
            dia.RestoreDirectory = true;


            if (dia.ShowDialog() == DialogResult.OK)
            {

                if (string.IsNullOrEmpty(dia.FileName))
                {
                    AntdUI.Notification.warn(this, "盛途科技", "未选择正确的Excel！", autoClose: 3, align: TAlignFrom.TR);
                    return;
                }

                try
                {
                    using (var stream = File.Open(dia.FileName, FileMode.Open, FileAccess.Read))
                    {
                        using (IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream))
                        {
                            System.Data.DataSet result = reader.AsDataSet(new ExcelDataSetConfiguration()
                            {
                                ConfigureDataTable = (_) => new ExcelDataTableConfiguration() { UseHeaderRow = true }
                            });
                            //excel所有数据
                            tableCollection = result.Tables;
                        }
                    }
                }
                catch
                {
                    AntdUI.Notification.warn(this, "盛途科技", "当前文件在其他地方打开或未找到文件！", autoClose: 3, align: TAlignFrom.TR);
                    return;
                }
                if (tableCollection[0].Rows.Count > 0)
                {
                    table = tableCollection[0].Clone();
                    tablefalse = tableCollection[0].Clone();
                    var index = 1;
                    //遍历数据
                    foreach (DataRow row in tableCollection[0].Rows)
                    {
                        var customer_code = row[0].ToString();
                        var company_name = row[1].ToString();
                        var contact_person = row[2].ToString();
                        var customer_type = row[3].ToString();
                        var phone = row[4].ToString();
                        var mobile = row[5].ToString();
                        var email = row[6].ToString();
                        var province = row[7].ToString();
                        var city = row[8].ToString();
                        var district = row[9].ToString();
                        var address = row[10].ToString();
                        var remarks = row[11].ToString();
                        var countList = _db.Query<CustomerInfoDto>().Where(c => c.customer_code == customer_code && c.company_name == company_name).ToList();

                        bool result = countList.Count > 0 ? true : false;
                        if (result)
                        {
                            AntdUI.Notification.success(this, "提示", "导入失败", autoClose: 3, align: TAlignFrom.TR);
                            return;
                        }

                        var dto =   new CustomerInfoDto();
                        dto.customer_code = customer_code;
                        dto.company_name = company_name;
                        dto.contact_person = contact_person;
                        dto.customer_type = customer_type;
                        dto.phone = phone;
                        dto.mobile = mobile;

                        dto.email = email;
                        dto.province = province;
                        dto.city = city;
                        dto.district = district;
                        dto.address = address;
                        dto.remarks = remarks;
                        dtoList.Add(dto);


                    }

                    long indexNumber = 0;
                    foreach (var item in dtoList)
                    {
                        indexNumber +=   _db.Insert(item);
                    }

               
                    if (indexNumber > 0)
                    {
                        AntdUI.Notification.success(this, "提示", "成功导入" + dtoList.Count.ToString() + "条数据", autoClose: 3, align: TAlignFrom.TR);

                        Query();
                    }
                    else
                    {
                        AntdUI.Notification.success(this, "提示", "导入失败", autoClose: 3, align: TAlignFrom.TR);

                    }

                 
                    }
                   }

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            Query();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                var form = new CustomerEditControl(this, new UICustomerInfoDto(), _db) { Size = new System.Drawing.Size(600, 300) };
                form.OnSuccess_Event -= Form_OnSuccess_Event;
                form.OnSuccess_Event += Form_OnSuccess_Event;
                AntdUI.Drawer.open(new AntdUI.Drawer.Config(this, form)
                {
                    OnLoad = () =>
                    {
                        AntdUI.Message.info(this, "进入编辑", autoClose: 1);
                    },
                    OnClose = () =>
                    {
                        AntdUI.Message.info(this, "结束编辑", autoClose: 1);
                    }
                });
            }
            catch (Exception)
            {

                throw;
            }
      
        }
        private void Form_OnSuccess_Event(object sender, Tuple<bool> e)
        {
            try
            {
                if (e.Item1)
                {
                    InitDataTable();
                }
            }
            catch (Exception)
            {

                throw;
            }
          
        }

        private async void InitDataTable()
        {

            Query();

        }
        private void SelectMultiple_SelectedValueChanged(object sender, ObjectsEventArgs e)
        {
            LoadPagination();
        }
        public void Query()
        {
            try
            {

          
            var dtoList = _db.Query<CustomerInfoDto>().ToList();
            dtoList = dtoList.Where(c =>
                    (string.IsNullOrEmpty(inputcustomer_code.Text) || c.customer_code.Contains(inputcustomer_code.Text))
                    && (string.IsNullOrEmpty(inputcustomer_name.Text) || c.company_name.Contains(inputcustomer_name.Text))
                ).ToList();

            var deviceList = new AntList<UICustomerInfoDto>();
            int i = 1;
            foreach (var item in dtoList)
            {

                deviceList.Add(new UICustomerInfoDto
                {
                    Index = i,
                    ID = item.customer_id,
                    Customer_code = item.customer_code,
                    Customer_name = item.company_name,
                    Customer_person = item.contact_person,
                    Customer_type = item.customer_type,
                    Phone = item.phone,
                    Mobile = item.mobile,
                    Email = item.email,

                    Province = item.province,
                    City = item.city,
                    District = item.district,
                    Address = item.address,
                    Remarks = item.remarks,
                    Created_by = DateTime.Now.ToString(),
                    Created_at = DateTime.Now.ToString(),
                    CellLinks = new CellLink[] {
                      new CellButton(Guid.NewGuid().ToString(),"编辑",TTypeMini.Primary),
                       new CellButton(Guid.NewGuid().ToString(),"删除",TTypeMini.Error),
                    }
                });
                i++;
            }

            tableBinding(deviceList);
            }
            catch (Exception)
            {

                throw;
            }
        }
        private async void tableBinding(AntList<UICustomerInfoDto> dtoList)
        {
            table_base.DataSource = null;
            table_base.Binding(dtoList);
        }
        private async void LoadPagination()
        {
            try
            {

         
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(LoadPagination));
                return;
            }
            pagination.Current = 1;
            var total = _db.Query<CustomerInfoDto>().Where(c=>c.is_deleted == 0).ToList().Count();

            pagination.TextDesc = $"共 {total} 条";
            pagination.Total = total;
            pagination.PageSize = 15;
            pagination.MaxPageTotal = 7;
            pagination.Gap = 8;
            pagination.Radius = 6;
            pagination.SizeChangerWidth = 150;


            int[] intarr = new int[selectMultiple.Items.Count];
            for (int i = 0; i < intarr.Length; i++)
            {
                intarr[i] = int.Parse(selectMultiple.Items[i].ToString());
            }
            pagination.PageSizeOptions = intarr;
            pagination.ShowSizeChanger = true;
            pagination.RightToLeft = RightToLeft.Yes;
                //table_base.Bordered = true;
            }
            catch (Exception)
            {

                throw;
            }
        }
        private void BtnJump_Click(object sender, EventArgs e)
        {
            var page = int.Parse(inputPage.Text);
            pagination.Current = page;
        }

        private void InitTableColumns()
        {
            table_base.Columns = new ColumnCollection() {
                 new AntdUI.ColumnCheck("Selected"){Fixed = true,Width="50"},
                 new AntdUI.Column("Index", "序号", ColumnAlign.Center),
                 new AntdUI.Column("Customer_code", "客户编号", ColumnAlign.Center),
                 new AntdUI.Column("Customer_name", "公司名称", ColumnAlign.Center),
                 new AntdUI.Column("Customer_person", "联系人", ColumnAlign.Center),
                 new AntdUI.Column("Customer_type", "客户类型", ColumnAlign.Center),
                 new AntdUI.Column("Phone", "电话", ColumnAlign.Center),
                 new AntdUI.Column("Mobile", "手机", ColumnAlign.Center),
                 new AntdUI.Column("Email", "邮箱", ColumnAlign.Center),
                 new AntdUI.Column("Province", "省份", ColumnAlign.Center),
                 new AntdUI.Column("City", "城市", ColumnAlign.Center),
                 new AntdUI.Column("District", "区县", ColumnAlign.Center),
                 new AntdUI.Column("Address", "详细地址", ColumnAlign.Center),
                 new AntdUI.Column("Remarks", "备注", ColumnAlign.Center),
                 new AntdUI.Column("CellLinks", "链接"){ LineBreak = true}
              };
            table_base.VisibleHeader = true;
        }
    }
}
