using EnterpriseDT.Net.Ftp;
using System;
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
        private IniConfUtil _IniConfUtil { get; set; }

        public ConfModel _ConfModel { get; set; }

        /// <summary>
        /// 初始化配置信息
        /// </summary>
        public ConfViewModel()
        {
            _IniConfUtil = IniConfUtil.getInstance();
            if (_IniConfUtil.IsExistForConfFile())
            {
                _ConfModel = new ConfModel();
                _ConfModel.Address = _IniConfUtil.GetPrivateProfileString("Address");
                _ConfModel.Port = _IniConfUtil.GetPrivateProfileString("Port");
                _ConfModel.Path = _IniConfUtil.GetPrivateProfileString("Path");
                _ConfModel.Username = _IniConfUtil.GetPrivateProfileString("Username");
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
        /// <returns>返回true或false</returns>
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
                connection.Timeout = 1000;
                connection.Connect();

                _IniConfUtil.WritePrivateProfileString("Address", addr);
                _IniConfUtil.WritePrivateProfileString("Port", port);
                _IniConfUtil.WritePrivateProfileString("Username", username);
                _IniConfUtil.WritePrivateProfileString("Password", password);
                _IniConfUtil.WritePrivateProfileString("Path", path);
                _IniConfUtil.WritePrivateProfileString("Anonymous", "false");

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
        /// <returns>返回true或false</returns>
        public bool connect(string addr, string port, string path)
        {
            try
            {
                FTPConnection connection = new FTPConnection();
                connection.ServerAddress = addr;
                connection.ServerPort = Convert.ToInt32(port);
                connection.ServerDirectory = path.Length != 0 ? path : @"/";
                connection.Connect();

                _IniConfUtil.WritePrivateProfileString("Address", addr);
                _IniConfUtil.WritePrivateProfileString("Port", port);
                _IniConfUtil.WritePrivateProfileString("Anonymous", "true");

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
