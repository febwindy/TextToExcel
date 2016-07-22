using System.Collections.Generic;

namespace TextToExcel.Commons.Filter
{
    /// <summary>
    /// 关键字过滤器
    /// 作者:李文禾
    /// </summary>
    class KeywordFilter : IFilter
    {
        /// <summary>
        /// 关键字容器
        /// </summary>
        private List<string> Keywords = new List<string>();
 
        /// <summary>
        /// 初始化的时候,添加关键字
        /// </summary>
        public KeywordFilter()
        {
            Keywords.Add("不通过");
        }

        #region IFilter.Member
        public bool DoFilter(string s, out string o, FilterChain filterChain)
        {
            // 判断数据是否存在关键字
            foreach (string keyword in Keywords)
            {
                if (s.IndexOf(keyword) != -1)
                {
                    o = s;
                    return false;
                }
            }

            // 重新进入过滤链
            return filterChain.DoFilter(s, out o);
        }
        #endregion
    }
}
