namespace KooFrame
{
    /// <summary>
    /// Model抽象模板
    /// </summary>
    public abstract class AbstractModel : IModel
    {
        private IArchitecture _architecture = null;

        /// <summary>
        /// 通过接口的显式申明返回架构对象
        /// 这样同时限制了子类对这个方法的调用 比如在Model层 原则上是不允许发生Command 所以再次进行阉割
        /// </summary>
        /// <returns></returns>
        IArchitecture IBelongToArchitecture.GetArchitecture() { return _architecture; }

        void ICanSetArchitecture.SetArchitecture(IArchitecture architecture) { this._architecture = architecture; }

        public bool Initialized { get; set; }
        void ICanInit.Init() => OnInit();

        public void DeInit() => OnDeInit();


        protected abstract void OnInit();

        protected virtual void OnDeInit() { }
    }
}