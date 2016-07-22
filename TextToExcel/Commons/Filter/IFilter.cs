
namespace TextToExcel.Commons.Filter
{
    /// <summary>
    /// 定义过滤器
    /// 作者:李文禾
    /// </summary>
    interface IFilter
    {
        /// <summary>
        /// 过滤器方法
        /// </summary>
        /// <param name="s">要进行过滤的数据</param>
        /// <param name="o">过滤完后返回的数据</param>
        /// <param name="filterChain"></param>
        /// <returns>返回类型为bool,返回true,表示该内容不需要过滤,返回false,表示该内容要过滤</returns>
        bool DoFilter(string s, out string o, FilterChain filterChain);
    }
}
