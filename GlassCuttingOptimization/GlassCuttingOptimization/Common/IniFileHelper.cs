using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GlassCuttingOptimization.Common
{
    public class IniFileHelper
    {


        public string Path;

        // 构造函数，指定 INI 文件路径
        public IniFileHelper(string iniPath)
        {
            Path = iniPath;
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern long WritePrivateProfileString(string section, string key, string value, string filePath);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int GetPrivateProfileString(string section, string key, string defaultValue, StringBuilder returnValue, int size, string filePath);

        // 写入 INI 文件
        public void Write(string section, string key, string value)
        {
            WritePrivateProfileString(section, key, value, Path);
        }

        // 读取 INI 文件
        public string Read(string section, string key, string defaultValue = "")
        {
            var result = new StringBuilder(1024);
            GetPrivateProfileString(section, key, defaultValue, result, result.Capacity, Path);
            return result.ToString();
        }
    }
}
