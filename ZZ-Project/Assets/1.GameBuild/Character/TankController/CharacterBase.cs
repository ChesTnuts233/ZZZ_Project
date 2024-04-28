//****************** 代码文件申明 ************************
//* 文件：CharacterBase                                       
//* 作者：Koo
//* 创建时间：2024/04/22 01:19:31 星期一
//* 功能：nothing
//*****************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using KooFrame;
using KooFrame.Manager;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameBuild
{
    public abstract class CharacterBase<T> : MonoBehaviour, IPlayerBase where T : CharacterBase<T>, new()
    {
        //public MovementBase movement { get; private set; }
 
        #region Unity生命周期  

        protected virtual void Awake()
        {
            InitInAwake();
            ArchitectureInstance = this as T;
            FrameInit();
            InputInit();
        }

        protected virtual void OnEnable() { }


        protected void OnDestroy()
        {
            //销毁时注销初始化
            DeInit();
        }

        #endregion

        #region 初始化流程

        private void InitInAwake()
        {
            InitArchitectureMono(); //初始化Mono架构
            OnInit();
            EndOfInit();
        }

        /// <summary>
        /// 初始化架构
        /// </summary>
        private void InitArchitectureMono()
        {
            RegisterIOC();

            //遍历列表初始化所有待初始化的Model
            foreach (var architectureModel in ArchitectureInstance._modelHashSet)
            {
                architectureModel.Init();
            }

            //清空Model初始化列表
            ArchitectureInstance._modelHashSet.Clear();

            //遍历列表初始化所有待初始化的System
            foreach (var architectureSystem in ArchitectureInstance._systemHashSet)
            {
                architectureSystem.Init();
            }

            //清空System初始化列表
            ArchitectureInstance._systemHashSet.Clear();

            OnRegisterPatch?.Invoke(ArchitectureInstance);

            _isInited = true;
        }


        private void DeInit()
        {
            OnDeInit();
            foreach (var system in _container.GetInstancesByType<ISystem>().Where(s => s.Initialized)) system.DeInit();
            foreach (var model in _container.GetInstancesByType<IModel>().Where(m => m.Initialized)) model.DeInit();
            _container.Clear();
            ArchitectureInstance = null;
        }


        private void FrameInit()
        {
            movement = KooTool.CreateInstance<MovementBase>(movementClass, movementArgs);
        }

        private void InputInit()
        {
            //input = KooTool.CreateInstance<PlayerInputBase>(inputClassType, inputArgs);
        }

        #endregion

        #region 架构API

        /// <summary>
        /// 注册System
        /// </summary>
        /// <param name="system"></param>
        /// <typeparam name="TSystem"></typeparam>
        public void RegisterSystem<TSystem>(TSystem system) where TSystem : ISystem
        {
            system.SetArchitecture(this);
            //IOC注册System
            Container.Register<TSystem>(system);

            if (!_isInited)
            {
                _systemHashSet.Add(system);
            }
            else
            {
                system.Init();
            }
        }

        /// <summary>
        /// 注册Model
        /// </summary>
        /// <param name="model"></param>
        /// <typeparam name="TModel"></typeparam>
        public void RegisterModel<TModel>(TModel model) where TModel : IModel
        {
            model.SetArchitecture(this);
            //IOC容器注册Model
            Container.Register<TModel>(model);

            if (!_isInited)
            {
                _modelHashSet.Add(model);
            }
            else
            {
                model.Init();
            }
        }

        /// <summary>
        /// 注册Utility
        /// </summary>
        /// <param name="utility"></param>
        /// <typeparam name="TUtility"></typeparam>
        public void RegisterUtility<TUtility>(TUtility utility) where TUtility : IUtility
        {
            Container.Register<TUtility>(utility);
        }

        public TSystem GetSystem<TSystem>() where TSystem : class, ISystem
        {
            return Container.Get<TSystem>();
        }

        public TModel GetModel<TModel>() where TModel : class, IModel
        {
            return Container.Get<TModel>();
        }

        public TUtility GetUtility<TUtility>() where TUtility : class, IUtility
        {
            return Container.Get<TUtility>();
        }

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

        private bool ExecuteStructCommand(IStructCommand command)
        {
            return command.Execute();
        }

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

        #endregion


        #region 抽象与虚方法

        /// <summary>
        /// 子类注册模块用
        /// </summary>
        protected abstract void RegisterIOC();

        protected virtual void OnInit() { }

        protected virtual void OnDeInit() { }

        protected virtual void EndOfInit()
        {
            movement.SetInput(input);
        }

        #endregion

        #region 角色特殊模块

        public MovementBase movement { get; private set; }


        public RequestHandleBase requestHandle { get; }
        public RequestReceiverBase receiver { get; }
        public InvokerBase invoker { get; }
        public PlayerInputBase input { get; }

        protected abstract Type inputClassType { get; }

        protected abstract object[] inputArgs { get; }
        
        protected abstract Type movementClass { get; }

        protected abstract object[] movementArgs { get; }

        #endregion

        #region 架构部分

        private static T ArchitectureInstance;

        public static IArchitecture Interface => ArchitectureInstance;

        /// <summary>
        /// IOC容器
        /// </summary>
        protected IOCContainer Container
        {
            get => _container;
            set => _container = value;
        }

        /// <summary>
        /// IOC容器
        /// </summary>
        [ShowInInspector] private IOCContainer _container = new IOCContainer();

        /// <summary>
        /// 是否已经初始化
        /// </summary>
        private bool _isInited = false;

        /// <summary>
        /// system的待初始化 -哈希表
        /// </summary>
        private HashSet<ISystem> _systemHashSet = new HashSet<ISystem>();

        /// <summary>
        /// model的待初始化 -哈希表
        /// </summary>
        private HashSet<IModel> _modelHashSet = new HashSet<IModel>();


        /// <summary>
        /// Init之后可能会出现各类问题 在外部注册这个Action 来解决这些问题
        /// </summary>
        public static Action<T> OnRegisterPatch = architecture => { };

        #endregion
    }
}