using System;
using System.Collections.Generic;
using System.Linq;
using KooFrame.BaseSystem;
using KooFrame.Manager;
using Sirenix.OdinInspector;
using UnityEngine;

namespace KooFrame
{
    public interface IArchitectureMono : IArchitecture { }


    /// <summary>
    /// Mono的个体架构
    /// 让架构内聚在一个Go内部 适用于较为复杂的游戏对象进行分层
    /// </summary>
    /// <typeparam name="T">游戏对象的控制中心</typeparam>
    public abstract class CharacterCtrlBase<T> : GameMonoBehaviour, IArchitectureMono
        where T : CharacterCtrlBase<T>, new()
    {
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

        #region Unity生命周期

        protected virtual void Awake()
        {
            ArchitectureInstance = this as T;
            InitArchitectureMono();
        }

        protected virtual void OnEnable()
        {
            InitInAwake();
        }


        /// <summary>
        /// 当销毁物品时候
        /// </summary>
        private void OnDestroy()
        {
            //销毁物品的时候触发
            DeInit();
        }

        #endregion

        /// <summary>
        /// 初始化架构
        /// </summary>
        public void InitArchitectureMono()
        {
            InitInAwake();

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


        /// <summary>
        /// 子类注册模块用
        /// </summary>
        protected abstract void InitInAwake();

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
        /// IOC容器
        /// </summary>
        [ShowInInspector] private IOCContainer _container = new IOCContainer();


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
        
//         #region 普通通用参数
//
//         [Tooltip("角色id"), FoldoutGroup("基本信息"), LabelText("角色id"), CharacterDescription] public int CharacterIndex;
//
// #if UNITY_EDITOR
//         [LabelText("禁用加载初始参数"), FoldoutGroup("基本信息")] public bool DisableLoadParameter = false;
// #endif
//
//         /// <summary>
//         /// 是否在移动平台上
//         /// </summary>
//         [Tooltip("当前参数"), FoldoutGroup("基本信息"), LabelText("是否在移动平台上")] public bool IsInPlatform;
//
//         /// <summary>
//         /// 现在在的平台
//         /// </summary>
//         [Tooltip("当前参数"), FoldoutGroup("基本信息"), LabelText("现在所在平台")] public WalkPlat CurWalkPlat;
//
//         [Tooltip("当前参数"), FoldoutGroup("基本信息"), LabelText("当前所在场景的TilemapManager")]
//         public TileMapGridManager TilemapManager;
//
//         #endregion
//
//         #region Unity生命周期
//
//         protected virtual void Start() { }
//
//         protected virtual void Update()
//         {
//             //HandleBuff();
//         }
//
//         #endregion
//
//         #region 基础方法
//
//         /// <summary>
//         /// 加载基础属性
//         /// </summary>
//         protected virtual void LoadInitParams()
//         {
// #if UNITY_EDITOR
//             //如果不加载初始参数直接返回
//             if (DisableLoadParameter) return;
// #endif
//         }
//
//         #endregion
//
//         #region Buff相关方法
//
//         public virtual void ClearBuffs(bool invokeAction, IBuffOwner owner)
//         {
//             GamePlay.Interface.GetSystem<BuffSystem>().ClearBuffs(owner);
//         }
//
//         #endregion
//
//         public virtual void AttackAct() { }
//
//         /// <summary>
//         /// 角色受伤
//         /// </summary>
//         /// <param name="attacker">攻击者</param>
//         /// <param name="damage">伤害</param>
//         /// <param name="dir">方向</param>
//         /// <param name="hitForce">击退力度</param>
//         /// <param name="sharpness">锋利度</param>
//         /// <returns></returns>
//         public virtual bool Injure(ICharacter attacker, float damage, Vector2 dir, float hitForce,
//             float sharpness)
//         {
//             return false;
//         }
    }
}