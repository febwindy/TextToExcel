using System.Collections.Generic;

namespace TextToExcel.Commons.Filter
{
    //
    // 摘要:
    //     关键字过滤器
    class KeywordFilter : IFilter
    {
        //
        // 摘要:
        //     关键字容器
        private List<string> Keywords = new List<string>();

        //
        // 摘要:
        //     初始化的时候,添加关键字
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
