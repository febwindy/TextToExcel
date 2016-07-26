using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace TextToExcel.Commons.Utils
{
    /// <summary>
    /// Ini配置文件操作工具类,该工具类是个单例类
    /// 作者:李文禾
    /// </summary>
    class IniConfUtil
    {
        private readonly string CURRENT_DIRECTORY;

        private readonly string CONFIG_FILE_NAME;

        private readonly string CONFIG_FILE_PATH;

        private readonly string APPLICATION_NAME;

        private static readonly object LockHelper = new object();

        private static IniConfUtil _instance = null;

        /// <summary>
        /// 初始化配置信息
        /// </summary>
        private IniConfUtil()
        {
            CURRENT_DIRECTORY = System.Environment.CurrentDirectory;
            CONFIG_FILE_NAME = "conf.ini";
            CONFIG_FILE_PATH = CURRENT_DIRECTORY + @"\" + CONFIG_FILE_NAME;
            APPLICATION_NAME = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
        }

        /// <summary>
        /// 单例调用
        /// </summary>
        /// <returns>返回实例化对象</returns>
        public static IniConfUtil getInstance()
        {
            if (_instance == null)
            {
                lock (LockHelper)
                {
                    if (_instance == null)
                        _instance = new IniConfUtil();
                }
            }
            return _instance;
        }

        /// <summary>
        /// 写入配置信息
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns>返回true或false</returns>
        public bool WritePrivateProfileString(string key, string value)
        {
            return WritePrivateProfileString(APPLICATION_NAME, key, value, CONFIG_FILE_PATH);
        }

        /// <summary>
        /// 读取配置信息
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>返回键所对应的值,如果没有相应的键或值不存在,返回空字符串</returns>
        public string GetPrivateProfileString(string key)
        {
            StringBuilder sb = new StringBuilder();
            GetPrivateProfileString(APPLICATION_NAME, key, "", sb, 1024, CONFIG_FILE_PATH);
            return sb.ToString();
        }

        /// <summary>
        /// 判断配置文件是否存在
        /// </summary>
        /// <returns>返回true或false</returns>
        public bool IsExistForConfFile()
        {
            if (File.Exists(CONFIG_FILE_PATH))
                return true;
            return false;
        }

        /// <summary>
        /// 写入配置信息
        /// </summary>
        /// <param name="lpAppName">应用程序名称</param>
        /// <param name="lpKeyName">键</param>
        /// <param name="lpString">值</param>
        /// <param name="lpFileName">文件名称</param>
        /// <returns></returns>
        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool WritePrivateProfileString(
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
        private static extern int GetPrivateProfileString(
            string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString,
            int nSize, string lpFileName);
    }
}
