
using System.Collections.Generic;
namespace TextToExcel.Commons.Filter
{
    //
    // 摘要:
    //     过滤器链
    class FilterChain
    {
        //
        // 摘要:
        //     过滤器所在索引
        private int index = 0;

        //
        // 摘要:
        //     过滤器容器
        private List<IFilter> filters;

        //
        // 摘要:
        //     初始化时初始化过滤器容器
        public FilterChain()
        {
            this.filters = new List<IFilter>();
        }

        //
        // 摘要:
        //     添加过滤器
        //
        // 参数:
        //   filter       
        //     所添加的过滤器
        //
        // 返回结果:
        //     返回过滤器链本身
        public FilterChain AddFilter(IFilter filter)
        {
            filters.Add(filter);
            return this;
        }

        //
        // 摘要:
        //     做过滤器操作
        //
        // 参数:
        //   s:       
        //     过滤的参数
        //   o:
        //     过滤返回的参数
        //
        // 返回结果:
        //     返回类型为bool,返回true,表示该内容不需要过滤,返回false,表示该内容要过滤
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
