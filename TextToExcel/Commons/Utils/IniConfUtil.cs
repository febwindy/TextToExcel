using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TextToExcel.Commons.Utils
{
    /// <summary>
    /// Ini配置文件操作工具类
    /// </summary>
    class IniConfUtil
    {
        /// <summary>
        /// 写入配置信息
        /// </summary>
        /// <param name="lpAppName">应用程序名称</param>
        /// <param name="lpKeyName">键</param>
        /// <param name="lpString">值</param>
        /// <param name="lpFileName">文件名称</param>
        /// <returns></returns>
        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool WritePrivateProfileString(
            string lpAppName, string lpKeyName, string lpString, string lpFileName);

        /// <summary>
        /// 读取配置信息
        /// </summary>
        /// <param name="lpAppName">应用程序名称</param>
        /// <param name="lpKeyName">键</param>
        /// <param name="lpDefault">默认值</param>
        /// <param name="lpReturnedString">返回值</param>
        /// <param name="nSize">返回值的长度</param>
        /// <param name="lpFileName">文件名称</param>
        /// <returns></returns>
        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int GetPrivateProfileString(
            string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString,
            int nSize, string lpFileName);
    }
}
