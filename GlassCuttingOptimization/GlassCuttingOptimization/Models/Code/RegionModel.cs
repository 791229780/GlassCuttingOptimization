using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlassCuttingOptimization.Models.Code
{
    public class RegionModel
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public List<RegionModel> List { get; set; } = new List<RegionModel>();
    }
}
