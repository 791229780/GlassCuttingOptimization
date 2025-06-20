using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlassCuttingOptimization.Models
{
    /// <summary>
    /// 客户信息
    /// </summary>
    [SugarTable("customer_info")]
    public class CustomerInfoDto
    {
        /// <summary>
        /// ID - 主键
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        /// <summary>
        /// 主键
        /// </summary>
        public long customer_id { get; set; }

        /// <summary>
        /// 客户编号
        /// </summary>
        public string customer_code { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        public string company_name { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string contact_person { get; set; }

        /// <summary>
        /// 客户类型：企业/个人/经销商
        /// </summary>
        public string customer_type { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string phone { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        public string mobile { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// 省份
        /// </summary>
        public string province { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        public string city { get; set; }

        /// <summary>
        /// 区县
        /// </summary>
        public string district { get; set; }

        /// <summary>
        /// 详细地址
        /// </summary>
        public string address { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string remarks { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string created_by { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string created_at { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        public string updated_by { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public string updated_at { get; set; }

        /// <summary>
        /// 是否删除：0-否 1-是
        /// </summary>
        public int is_deleted { get; set; }
    }
}
