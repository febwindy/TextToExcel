using System;
using System.Globalization;

namespace TextToExcel.Commons.Utils
{
    class DateTimeUtil
    {
        public const string DATE_FORMAT = "yyyy/MM/dd";

        public const string DATE_TIME_FORMAT = "yyyy/MM/dd HH:mm:ss";

        //
        // 摘要:
        //     日期转换
        //
        // 参数:
        //   s:
        //     传入的日期
        //   format:
        //     日期转换格式,该format默认为ShortDatePattern的
        //
        // 返回结果:
        //     返回DateTime,如果传入的日期不正确或转换的格式不正确,返回DateTime.MinValue
        public static DateTime ConvertDateTime(string s, string format)
        {
            DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
            dtFormat.ShortDatePattern = format;
            return ConvertDateTime(s, dtFormat);        
        }

        //
        // 摘要:
        //     日期转换
        //
        // 参数:
        //   s:
        //     传入的日期
        //   provider：
        //     自定义所要转换的日期格式
        //
        // 返回结果:
        //     返回DateTime,如果传入的日期不正确或转换的格式不正确,返回DateTime.MinValue
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
