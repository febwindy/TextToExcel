using System;
using System.Globalization;

namespace TextToExcel.Commons.Utils
{     
    /// <summary>
    /// 日期转换工具类
    /// 作者:李文禾
    /// </summary>
    class DateTimeUtil
    {
        public const string DATE_FORMAT = "yyyy/MM/dd";

        public const string DATE_TIME_FORMAT = "yyyy/MM/dd HH:mm:ss";

        /// <summary>
        /// 日期转换
        /// </summary>
        /// <param name="s">传入的日期</param>
        /// <param name="format">日期转换格式,该format默认为ShortDatePattern的</param>
        /// <returns>返回DateTime,如果传入的日期不正确或转换的格式不正确,返回DateTime.MinValue</returns>
        public static DateTime ConvertDateTime(string s, string format)
        {
            DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
            dtFormat.ShortDatePattern = format;
            return ConvertDateTime(s, dtFormat);        
        }

        /// <summary>
        /// 日期转换
        /// </summary>
        /// <param name="s">传入的日期</param>
        /// <param name="provider">自定义所要转换的日期格式</param>
        /// <returns>返回DateTime,如果传入的日期不正确或转换的格式不正确,返回DateTime.MinValue</returns>
        public static DateTime ConvertDateTime(string s, IFormatProvider provider)
        {
            try
            {
                return Convert.ToDateTime(s, provider);
            }
            catch (FormatException)
            {
                return DateTime.MinValue;
            }
        }

    }
}
