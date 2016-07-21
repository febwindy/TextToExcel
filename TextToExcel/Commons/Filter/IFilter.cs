
namespace TextToExcel.Commons.Filter
{
    //
    // 摘要:
    //     定义过滤器
    interface IFilter
    {
        //
        // 摘要:
        //     过滤器方法
        //
        // 参数:
        //   s:
        //     要进行过滤的数据
        //   o:
        //     过滤完后返回的数据
        //
        // 返回结果:
        //     返回类型为bool,返回true,表示该内容不需要过滤,返回false,表示该内容要过滤
        bool DoFilter(string s, out string o, FilterChain filterChain);
    }
}
