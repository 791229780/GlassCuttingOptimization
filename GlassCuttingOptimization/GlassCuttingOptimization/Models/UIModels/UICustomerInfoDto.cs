using AntdUI;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlassCuttingOptimization.Models.UIModels
{

    public class UICustomerInfoDto : NotifyProperty
    {
        /// <summary>
        /// 选择
        /// </summary>
        private bool selected = false;
        public bool Selected
        {
            get { return selected; }
            set
            {
                if (selected == value) return;
                selected = value;
                OnPropertyChanged(nameof(Selected));
            }
        }
        /// <summary>
        /// 序号
        /// </summary>
        private long index { get; set; }
        public long Index
        {
            get { return index; }
            set
            {
                if (index == value) return;
                index = value;
                OnPropertyChanged(nameof(Index));
            }
        }

        /// <summary>
        /// 主键
        /// </summary>
        private long id { get; set; }
        public long ID
        {
            get { return id; }
            set
            {
                if (id == value) return;
                id = value;
                OnPropertyChanged(nameof(ID));
            }
        }

        private CellLink[] cellLinks;
        public CellLink[] CellLinks
        {
            get { return cellLinks; }
            set
            {
                if (cellLinks == value) return;
                cellLinks = value;
                OnPropertyChanged(nameof(CellLinks));
            }
        }
   
        /// <summary>
        /// 客户编号
        /// </summary>
        public string customer_code { get; set; }

        public string Customer_code
        {
            get { return customer_code; }
            set
            {
                if (customer_code == value) return;
                customer_code = value;
                OnPropertyChanged(nameof(Customer_code));
            }
        }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string company_name { get; set; }
        public string Customer_name
        {
            get { return company_name; }
            set
            {
                if (company_name == value) return;
                company_name = value;
                OnPropertyChanged(nameof(Customer_name));
            }
        }
        /// <summary>
        /// 联系人
        /// </summary>
        public string contact_person { get; set; }
        public string Customer_person
        {
            get { return contact_person; }
            set
            {
                if (contact_person == value) return;
                contact_person = value;
                OnPropertyChanged(nameof(Customer_person));
            }
        }
        /// <summary>
        /// 客户类型：企业/个人/经销商
        /// </summary>
        public string customer_type { get; set; }
        public string Customer_type
        {
            get { return customer_type; }
            set
            {
                if (customer_type == value) return;
                customer_type = value;
                OnPropertyChanged(nameof(Customer_type));
            }
        }
        /// <summary>
        /// 电话
        /// </summary>
        public string phone { get; set; }
        public string Phone
        {
            get { return phone; }
            set
            {
                if (phone == value) return;
                phone = value;
                OnPropertyChanged(nameof(Phone));
            }
        }
        /// <summary>
        /// 手机
        /// </summary>
        public string mobile { get; set; }
        public string Mobile
        {
            get { return mobile; }
            set
            {
                if (mobile == value) return;
                mobile = value;
                OnPropertyChanged(nameof(Mobile));
            }
        }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string email { get; set; }
        public string Email
        {
            get { return email; }
            set
            {
                if (email == value) return;
                email = value;
                OnPropertyChanged(nameof(Email));
            }
        }
        /// <summary>
        /// 省份
        /// </summary>
        public string province { get; set; }
        public string Province
        {
            get { return province; }
            set
            {
                if (province == value) return;
                province = value;
                OnPropertyChanged(nameof(Province));
            }
        }
        /// <summary>
        /// 城市
        /// </summary>
        public string city { get; set; }
        public string City
        {
            get { return city; }
            set
            {
                if (city == value) return;
                city = value;
                OnPropertyChanged(nameof(City));
            }
        }
        /// <summary>
        /// 区县
        /// </summary>
        public string district { get; set; }
        public string District
        {
            get { return district; }
            set
            {
                if (district == value) return;
                district = value;
                OnPropertyChanged(nameof(District));
            }
        }
        /// <summary>
        /// 详细地址
        /// </summary>
        public string address { get; set; }
        public string Address
        {
            get { return address; }
            set
            {
                if (address == value) return;
                address = value;
                OnPropertyChanged(nameof(Address));
            }
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string remarks { get; set; }
        public string Remarks
        {
            get { return remarks; }
            set
            {
                if (remarks == value) return;
                remarks = value;
                OnPropertyChanged(nameof(Remarks));
            }
        }
        /// <summary>
        /// 创建人
        /// </summary>
        public string created_by { get; set; }
        public string Created_by
        {
            get { return created_by; }
            set
            {
                if (created_by == value) return;
                created_by = value;
                OnPropertyChanged(nameof(Created_by));
            }
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string created_at { get; set; }
        public string Created_at
        {
            get { return created_at; }
            set
            {
                if (created_at == value) return;
                created_at = value;
                OnPropertyChanged(nameof(Created_at));
            }
        }
        /// <summary>
        /// 更新人
        /// </summary>
        public string updated_by { get; set; }
        public string Updated_by
        {
            get { return updated_by; }
            set
            {
                if (updated_by == value) return;
                updated_by = value;
                OnPropertyChanged(nameof(Updated_by));
            }
        }
        /// <summary>
        /// 更新时间
        /// </summary>
        public string updated_at { get; set; }
        public string Updated_at
        {
            get { return updated_at; }
            set
            {
                if (updated_at == value) return;
                updated_at = value;
                OnPropertyChanged(nameof(Updated_at));
            }
        }
        /// <summary>
        /// 是否删除：0-否 1-是
        /// </summary>
        public int is_deleted { get; set; }
        public int Is_deleted
        {
            get { return is_deleted; }
            set
            {
                if (is_deleted == value) return;
                is_deleted = value;
                OnPropertyChanged(nameof(Is_deleted));
            }
        }
    }
}
