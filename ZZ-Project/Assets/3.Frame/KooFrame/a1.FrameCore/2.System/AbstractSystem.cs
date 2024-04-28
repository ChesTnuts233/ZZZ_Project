namespace KooFrame
{
    public abstract class AbstractSystem : ISystem
    {
        protected IArchitecture Architecture;

        IArchitecture IBelongToArchitecture.GetArchitecture() { return Architecture; }

        void ICanSetArchitecture.SetArchitecture(IArchitecture architecture) { Architecture = architecture; }

        public bool Initialized { get; set; }
        void ICanInit.Init() => OnInit();

        public void DeInit() => OnDeInit();


        protected abstract void OnInit();

        protected virtual void OnDeInit() { }
    }
}