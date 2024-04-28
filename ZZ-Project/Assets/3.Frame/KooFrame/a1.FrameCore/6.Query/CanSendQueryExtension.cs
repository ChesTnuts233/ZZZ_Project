namespace KooFrame
{
    /// <summary>
    /// 方便发送查询的静态拓展
    /// </summary>
    public static class CanSendQueryExtension
    {
        /// <summary>
        /// 发送查询
        /// </summary>
        /// <param name="self">查询者</param>
        /// <param name="query">查询</param>
        /// <typeparam name="TResult">返回的查询结果</typeparam>
        /// <returns></returns>
        public static TResult SendQuery<TResult>(this ICanSendQuery self, IQuery<TResult> query)
        {
            return self.GetArchitecture().SendQuery(query);
        }
    }
}