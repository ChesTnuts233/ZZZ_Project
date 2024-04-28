//****************** 代码文件申明 ************************
//* 文件：MovementBase                                       
//* 作者：Koo
//* 创建时间：2024/04/22 01:26:54 星期一
//* 功能：移动模块基类
//*****************************************************

using UnityEngine;

namespace GameBuild
{
    public class MovementBase
    {
        public bool MovementEnable { get; set; }


        /// <summary>
        /// 最大移动速度
        /// </summary>
        protected float maxSpeed;

        /// <summary>
        /// 加速度
        /// </summary>
        protected float accelerate;

        protected Transform playerTransform;
        protected Transform inputTransform;

        protected Rigidbody rb;
        protected MonoBehaviour mono;
        protected Animator anim;
        
        protected PlayerInputBase input;

        /// <summary>
        /// 是否已经初始化
        /// </summary>
        private bool isHasInited;


        public MovementBase(MonoBehaviour mono, Rigidbody rb, Transform inputSpace, Transform player, Animator anim)
        {
            MovementEnable = true;
            this.rb = rb;
            this.mono = mono;
            this.anim = anim;
            playerTransform = player;
            inputTransform = inputSpace;
            Init();
        }

        public void SetInput(PlayerInputBase input)
        {
            this.input = input;
        }

        public virtual void ExecuteRequest(int request) { }

        public virtual void CancelExecuteRequest(int request) { }

        protected virtual void OnInit() { }

        /// <summary>
        /// 物理Update更新帧开始处理
        /// </summary>
        protected virtual void OnFixedUpdateStartHandle() { }

        /// <summary>
        /// 物理Update更新帧处理逻辑
        /// </summary>
        protected virtual void OnFixedUpdateHandle() { }

        /// <summary>
        /// 物理Update更新帧结束处理
        /// </summary>
        protected virtual void OnFixedUpdateOverHandle()
        {
            OnClearState();
        }

        protected virtual void OnUpdateStartHandle() { }
        protected virtual void OnUpdateHandle() { }
        protected virtual void OnUpdateOverHandle() { }

        //清除状态
        protected virtual void OnClearState() { }


        private void Init()
        {
            if (isHasInited) return;
            isHasInited = true;
            OnInit();
        }

        public void Update()
        {
            OnUpdateStartHandle();
            OnUpdateHandle();
            OnUpdateOverHandle();
        }

        public void FixedUpdate()
        {
            if (!MovementEnable) return;
            OnFixedUpdateStartHandle();
            OnFixedUpdateHandle();
            OnFixedUpdateOverHandle();
        }
    }
}