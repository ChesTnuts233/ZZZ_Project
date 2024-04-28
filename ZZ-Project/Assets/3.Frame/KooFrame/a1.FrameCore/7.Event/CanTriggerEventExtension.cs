namespace KooFrame
{
    /// <summary>
    /// 通过静态拓展简化事件发送
    /// </summary>
    public static class CanTriggerEventExtension
    {
        /// <summary>
        /// 在架构Architecture内部触发事件
        /// </summary>
        /// <param name="self"></param>
        /// <typeparam name="T"></typeparam>
        public static void TriggerEvent<T>(this ICanTriggerEvent self) where T : new()
        {
            self.GetArchitecture().TriggerEvent<T>();
        }

        public static void TriggerEvent<T>(this ICanTriggerEvent self, T e)
        {
            self.GetArchitecture().TriggerEvent<T>(e);
        }
    }
}