namespace KooFrame
{
    public interface ICanInit
    {
        /// <summary>
        /// 是否已经初始化
        /// </summary>
        bool Initialized { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>
        void Init();

        /// <summary>
        /// 取消初始化
        /// </summary>
        void DeInit();
    }
}