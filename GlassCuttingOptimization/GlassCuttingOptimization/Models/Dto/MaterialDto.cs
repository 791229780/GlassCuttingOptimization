using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlassCuttingOptimization.Models.Dto
{

    /// <summary>
    /// 材料信息表
    /// </summary>
    [SugarTable("material")]
    public class MaterialDto
    {

        /// <summary>
        /// ID - 主键
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public long ID { get; set; }

        /// <summary>
        /// 材料名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 材料类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 完整描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 厚度
        /// </summary>
        public string Grinding { get; set; }
        /// <summary>
        /// X1
        /// </summary>
        public string X1 { get; set; }

        /// <summary>
        /// X2
        /// </summary>
        public string X2 { get; set; }

        /// <summary>
        /// Y1
        /// </summary>
        public string Y1 { get; set; }

        /// <summary>
        /// Y2
        /// </summary>
        public string Y2 { get; set; }

        /// <summary>
        /// 最小距离
        /// </summary>
        public string Minimum { get; set; }

        /// <summary>
        /// 研磨
        /// </summary>
        public string Thickness { get; set; }
    }
}
