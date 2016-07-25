using EnterpriseDT.Net.Ftp;
using System;
using System.IO;
using System.Text;
using TextToExcel.Commons.Utils;
using TextToExcel.Model;

namespace TextToExcel.ViewModel
{
    /// <summary>
    /// ConfViewModel
    /// 作者:李文禾
    /// </summary>
    class ConfViewModel
    {
        private readonly string CURRENT_DIRECTORY;

        private readonly string CONFIG_FILE_NAME;

        private readonly string CONFIG_FILE_PATH;

        private readonly string APPLICATION_NAME;

        public ConfModel _ConfModel { get; set; }

        /// <summary>
        /// 初始化配置信息
        /// </summary>
        public ConfViewModel()
        {
            // 初始化相应数据
            CURRENT_DIRECTORY = System.Environment.CurrentDirectory;
            CONFIG_FILE_NAME = "conf.ini";
            CONFIG_FILE_PATH = CURRENT_DIRECTORY + @"\" + CONFIG_FILE_NAME;
            APPLICATION_NAME = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;

            if (File.Exists(CONFIG_FILE_PATH))
            {
                // 解析数据
                StringBuilder stringBuilderForAddress = new StringBuilder();
                StringBuilder stringBuilderForUsername = new StringBuilder();
                StringBuilder stringBuilderForPath = new StringBuilder();
                StringBuilder stringBuilderForPort = new StringBuilder();
                StringBuilder stringBuilderForAnonymous = new StringBuilder();
                
                IniConfUtil.GetPrivateProfileString(APPLICATION_NAME, "Address", "", stringBuilderForAddress, 1024, CONFIG_FILE_PATH);
                IniConfUtil.GetPrivateProfileString(APPLICATION_NAME, "Username", "", stringBuilderForUsername, 1024, CONFIG_FILE_PATH);
                IniConfUtil.GetPrivateProfileString(APPLICATION_NAME, "Path", "", stringBuilderForPath, 1024, CONFIG_FILE_PATH);
                IniConfUtil.GetPrivateProfileString(APPLICATION_NAME, "Port", "", stringBuilderForPort, 1024, CONFIG_FILE_PATH);
                IniConfUtil.GetPrivateProfileString(APPLICATION_NAME, "Anonymous", "", stringBuilderForAnonymous, 1024, CONFIG_FILE_PATH);

                _ConfModel = new ConfModel();
                _ConfModel.Address = stringBuilderForAddress.ToString();
                _ConfModel.Port = stringBuilderForPort.ToString();
                _ConfModel.Path = stringBuilderForPath.ToString();

                if (stringBuilderForAnonymous.Length != 0 && stringBuilderForAnonymous.ToString() != "true")
                    _ConfModel.Username = stringBuilderForUsername.ToString();
            }
        }

        /// <summary>
        /// 连接测试
        /// </summary>
        /// <param name="addr">地址</param>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="path">服务器目录</param>
        /// <param name="port">端口</param>
        /// <returns></returns>
        public bool connect(string addr, string username, string password, string path, string port)
        {
            try
            {
                FTPConnection connection = new FTPConnection();
                connection.UserName = username;
                connection.Password = password;
                connection.ServerAddress = addr;
                connection.ServerDirectory = path;
                connection.ServerPort = Convert.ToInt32(port);
                connection.CloseStreamsAfterTransfer = false;
                connection.Timeout = 3000;
                connection.Connect();

                IniConfUtil.WritePrivateProfileString(APPLICATION_NAME, "Address", addr, CONFIG_FILE_PATH);
                IniConfUtil.WritePrivateProfileString(APPLICATION_NAME, "Port", port, CONFIG_FILE_PATH);
                IniConfUtil.WritePrivateProfileString(APPLICATION_NAME, "Username", username, CONFIG_FILE_PATH);
                IniConfUtil.WritePrivateProfileString(APPLICATION_NAME, "Password", password, CONFIG_FILE_PATH);
                IniConfUtil.WritePrivateProfileString(APPLICATION_NAME, "Path", path, CONFIG_FILE_PATH);
                IniConfUtil.WritePrivateProfileString(APPLICATION_NAME, "Anonymous", "false", CONFIG_FILE_PATH);

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// 连接测试
        /// </summary>
        /// <param name="addr">地址</param>
        /// <param name="port">端口</param>
        /// <returns></returns>
        public bool connect(string addr, string port, string path)
        {
            try
            {
                FTPConnection connection = new FTPConnection();
                connection.ServerAddress = addr;
                connection.ServerPort = Convert.ToInt32(port);
                connection.ServerDirectory = path.Length != 0 ? path : @"/";
                connection.Connect();

                IniConfUtil.WritePrivateProfileString(APPLICATION_NAME, "Address", addr, CONFIG_FILE_PATH);
                IniConfUtil.WritePrivateProfileString(APPLICATION_NAME, "Port", port, CONFIG_FILE_PATH);
                IniConfUtil.WritePrivateProfileString(APPLICATION_NAME, "Anonymous", "true", CONFIG_FILE_PATH);

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
