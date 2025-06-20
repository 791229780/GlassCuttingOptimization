using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlassCuttingOptimization.Enums
{
    public enum PowerType
    {
        /// <summary>
        /// 玻璃类型枚举
        /// </summary>
        [Description("单层玻璃")]
        Monolithic = 1,

        /// <summary>
        /// 夹层玻璃
        /// </summary>
        [Description("夹层玻璃")]
        Laminated = 2,

        /// <summary>
        /// 单片镀膜
        /// </summary>
        [Description("单片镀膜")]
        MonolithicCoated = 3,

        /// <summary>
        /// 夹层镀膜
        /// </summary>
        [Description("夹层镀膜")]
        LaminatedCoated = 4,

        /// <summary>
        /// 丝印玻璃
        /// </summary>
        [Description("丝印玻璃")]
        SilkScreen = 5,

        /// <summary>
        /// 带LOW-E膜的单层玻璃
        /// </summary>
        [Description("带LOW-E膜的单层玻璃")]
        MonolithicLowE = 6,

        /// <summary>
        /// TPF Low-E夹胶
        /// </summary>
        [Description("TPF Low-E夹胶")]
        TpfLowELaminated = 7,

        /// <summary>
        /// Monolithic with Film
        /// </summary>
        [Description("Monolithic with Film")]
        MonolithicWithFilm = 8,

        /// <summary>
        /// Monolitich EASYPRO
        /// </summary>
        [Description("Monolitich EASYPRO")]
        MonolithicEasyPro = 9,

        /// <summary>
        /// Laminated EASYPRO
        /// </summary>
        [Description("Laminated EASYPRO")]
        LaminatedEasyPro = 10
    }
}
