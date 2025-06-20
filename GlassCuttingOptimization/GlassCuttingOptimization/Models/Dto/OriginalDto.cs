using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlassCuttingOptimization.Models.Dto
{
    /// <summary>
    /// 原片信息表
    /// </summary>
    [SugarTable("original")]
    public class OriginalDto
    {
        /// <summary>
        /// ID - 主键
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public long ID { get; set; }


        /// <summary>
        /// 材料外键
        /// </summary>
        public long MaterialID { get; set; }


        /// <summary>
        /// 数量
        /// </summary>
        public long Number { get; set; }


        /// <summary>
        /// X尺寸
        /// </summary>
        public string X1 { get; set; }

        /// <summary>
        /// Y尺寸
        /// </summary>
        public string Y1 { get; set; }


        /// <summary>
        /// 优先级
        /// </summary>
        public string Priority { get; set; }

        /// <summary>
        /// 横切类型
        /// </summary>
        public string Type { get; set; }
    }
}
