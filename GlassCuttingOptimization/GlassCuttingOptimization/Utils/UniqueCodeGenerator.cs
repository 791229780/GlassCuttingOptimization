using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlassCuttingOptimization.Utils
{
    public static  class UniqueCodeGenerator
    {
        /// <summary>
        /// 生成带前缀的唯一码
        /// 格式: {prefix}{yyyyMMddHHmmss}
        /// </summary>
        /// <param name="prefix">自定义前缀，用于标识材料类型</param>
        /// <returns>带前缀的唯一码</returns>
        public static string Generate(string prefix = "")
        {
            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            return $"{prefix}{timestamp}";
        }

    }
}
