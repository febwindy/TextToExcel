
using System.Collections.Generic;
namespace TextToExcel.Commons.Filter
{
    /// <summary>
    /// 过滤器链
    /// 作者:李文禾
    /// </summary>
    class FilterChain
    {   
        /// <summary>
        /// 过滤器所在索引
        /// </summary>
        private int index = 0;
   
        /// <summary>
        /// 过滤器容器
        /// </summary>
        private List<IFilter> filters;
   
        /// <summary>
        /// 初始化时初始化过滤器容器
        /// </summary>
        public FilterChain()
        {
            this.filters = new List<IFilter>();
        }
   
        /// <summary>
        /// 添加过滤器
        /// </summary>
        /// <param name="filter">所添加的过滤器</param>
        /// <returns>返回过滤器链本身</returns>
        public FilterChain AddFilter(IFilter filter)
        {
            filters.Add(filter);
            return this;
        }    

        /// <summary>
        /// 做过滤器操作
        /// </summary>
        /// <param name="s">过滤的参数</param>
        /// <param name="o">过滤返回的参数</param>
        /// <returns>返回类型为bool,返回true,表示该内容不需要过滤,返回false,表示该内容要过滤</returns>
        public bool DoFilter(string s, out string o)
        {
            if (index == filters.Count)
            {
                o = s;
                index = 0;
                return true;
            }

            IFilter filter = filters[index];
            index++;

            return filter.DoFilter(s, out o, this);
        }
    }
}
