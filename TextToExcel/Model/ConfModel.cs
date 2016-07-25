using System;

namespace TextToExcel.Model
{
    /// <summary>
    /// 配置信息
    /// 作者:李文禾
    /// </summary>
    class ConfModel : BaseModel
    {
        public String Address { get; set; }

        public String Port { get; set; }

        public String Username { get; set; }

        public String Password { get; set; }

        public String Path { get; set; }
    }
}
