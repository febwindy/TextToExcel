using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TextToExcel.Commons.Filter
{  
    /// <summary>
    /// 姓名和身份证过滤器
    /// 作者:李文禾
    /// </summary>
    class NameAndIdCardFilter : IFilter
    {
        #region IFilter.Member
        public bool DoFilter(string s, out string o, FilterChain filterChain)
        {
            // 过滤完数据所在容器
            List<string> newStrList = new List<string>();

            // 过滤数据并对数据重新进行组装
            string[] sArr = Regex.Split(s, @"\s+");
            for (int i = 0; i < sArr.Length; i ++)
            {
                if (sArr[i].IndexOf(@".jpg") != -1)
                {
                    sArr[i] = sArr[i].Replace(@".jpg", "");
                    string[] tmpSArr = Regex.Split(sArr[i], @"_");
                    if (tmpSArr.Length == 2)
                    {
                        newStrList.Add(tmpSArr[0]);
                        newStrList.Add(tmpSArr[1]);
                    }
                    else
                    {
                        newStrList.Add(tmpSArr[0]);
                        newStrList.Add(null);
                    }
                }
                else
                {
                    newStrList.Add(sArr[i]);
                }
            }

            // 对数据进行赋值
            o = string.Join(" ", newStrList.ToArray());

            // 重新进入过滤链
            return filterChain.DoFilter(o, out o);
        }
        #endregion
    }
}
