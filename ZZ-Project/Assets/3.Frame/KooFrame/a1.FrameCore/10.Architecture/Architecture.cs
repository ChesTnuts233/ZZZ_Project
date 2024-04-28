using System;
using System.Linq;
using KooFrame.Manager;

namespace KooFrame
{
    /// <summary>
    /// 架构层
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Architecture<T> : IArchitecture, ICanDeInit where T : Architecture<T>, new()
    {
        protected static T ArchitectureInstance;

        /// <summary>
        /// 是否已经初始化完毕
        /// </summary>
        private bool _isInited = false;


        /// <summary>
        /// Init之后可能会出现各类问题 在外部注册这个Action 来解决这些问题
        /// </summary>
        public static Action<T> OnRegisterPatch = architecture => { };


        public static IArchitecture Interface
        {
            get
            {
                if (ArchitectureInstance == null)
                {
                    MakeSureArchitecture();
                }

                return ArchitectureInstance;
            }
        }


        static void MakeSureArchitecture()
        {
            if (ArchitectureInstance == null)
            {
                ArchitectureInstance = new T();
                ArchitectureInstance.Init();

                //初始化完成后触发的Action
                OnRegisterPatch?.Invoke(ArchitectureInstance);

                //利用Linq遍历列表初始化所有待初始化的Model
                foreach (var model in ArchitectureInstance._container.GetInstancesByType<IModel>()
                             .Where(model => !model.Initialized))
                {
                    model.Init();
                    model.Initialized = true;
                }


                //利用Linq遍历列表初始化所有待初始化的System
                foreach (var system in ArchitectureInstance._container.GetInstancesByType<ISystem>()
                             .Where(system => !system.Initialized))
                {
                    system.Init();
                    system.Initialized = true;
                }


                ArchitectureInstance._isInited = true;
            }
        }


        /// <summary>
        /// 子类注册模块用
        /// </summary>
        protected abstract void Init();

        protected virtual void OnDeInit() { }

        public void DeInit()
        {
            OnDeInit();
            foreach (var system in _container.GetInstancesByType<ISystem>().Where(s => s.Initialized)) system.DeInit();
            foreach (var model in _container.GetInstancesByType<IModel>().Where(m => m.Initialized)) model.DeInit();
            _container.Clear();
            ArchitectureInstance = null;
        }


        /// <summary>
        /// 架构IOC
        /// </summary>
        private IOCContainer _container = new IOCContainer();

        /// <summary>
        /// 注册数据层
        /// </summary>
        /// <param name="model">数据模型</param>
        /// <typeparam name="TModel">模型类型</typeparam>
        public void RegisterModel<TModel>(TModel model) where TModel : IModel
        {
            model.SetArchitecture(this);
            //IOC容器注册Model
            _container.Register<TModel>(model);

            if (this._isInited) //如果架构已经初始化了
            {
                model.Init();             //初始化Model
                model.Initialized = true; //标记初始化完成
            }
        }

        /// <summary>
        /// 注册系统层
        /// </summary>
        /// <param name="system"></param>
        /// <typeparam name="TSystem"></typeparam>
        public void RegisterSystem<TSystem>(TSystem system) where TSystem : ISystem
        {
            //让system通过接口设置自己的架构实例
            system.SetArchitecture(this);
            //IOC容器注册System
            _container.Register<TSystem>(system);

            if (this._isInited) //如果架构已经初始化了
            {
                system.Init();             //初始化System
                system.Initialized = true; //标记初始化完成
            }
        }


        /// <summary>
        /// 注册工具层
        /// </summary>
        /// <param name="utility"></param>
        /// <typeparam name="TUtility"></typeparam>
        public void RegisterUtility<TUtility>(TUtility utility) where TUtility : IUtility
        {
            _container.Register<TUtility>(utility);
        }

        public TSystem GetSystem<TSystem>() where TSystem : class, ISystem
        {
            return _container.Get<TSystem>();
        }

        public TModel GetModel<TModel>() where TModel : class, IModel
        {
            return _container.Get<TModel>();
        }

        public TUtility GetUtility<TUtility>() where TUtility : class, IUtility
        {
            return _container.Get<TUtility>();
        }

        #region Command

        public TResult SendCommand<TResult>(ICommand<TResult> command)
        {
            return ExecuteCommand(command);
        }

        public void SendCommand<TCommand>(TCommand command) where TCommand : ICommand
        {
            ExecuteCommand(command);
        }

        public bool SendStructCommand(IStructCommand command)
        {
            return ExecuteStructCommand(command);
        }

        protected virtual TResult ExecuteCommand<TResult>(ICommand<TResult> command)
        {
            command.SetArchitecture(this);
            return command.Execute();
        }

        protected virtual void ExecuteCommand(ICommand command)
        {
            command.SetArchitecture(this);
            command.Execute();
        }

        protected virtual bool ExecuteStructCommand(IStructCommand command)
        {
            return command.Execute();
        }

        #endregion

        public TResult SendQuery<TResult>(IQuery<TResult> query)
        {
            query.SetArchitecture(this);
            return query.Do();
        }


        private TypeEventSystem typeEventSystem = new TypeEventSystem();

        public void TriggerEvent<TEvent>() where TEvent : new()
        {
            typeEventSystem.Trigger<TEvent>();
        }

        public void TriggerEvent<TEvent>(TEvent e)
        {
            typeEventSystem.Trigger<TEvent>(e);
        }

        public IUnRegister RegisterEvent<TEvent>(Action<TEvent> onEvent)
        {
            return typeEventSystem.Register<TEvent>(onEvent);
        }


        public void UnRegisterEvent<TEvent>(Action<TEvent> onEvent)
        {
            typeEventSystem.UnRegister<TEvent>(onEvent);
        }
    }
}