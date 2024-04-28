//****************** 代码文件申明 ************************
//* 文件：KooInputTool                                       
//* 作者：Koo
//* 创建时间：2024/04/27 19:02:49 星期六
//* 功能：输入的工具类
//*****************************************************

using System;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif


namespace KooFrame
{
    public static partial class KooTool
    {
        #if ENABLE_INPUT_SYSTEM
        /// <summary>
        /// 选择性的注册输入事件
        /// </summary>
        /// <param name="inputAction"></param>
        /// <param name="action"></param>
        public static void RegisterInputEvent(this InputAction inputAction, Action<InputAction.CallbackContext> action,
            bool started = false, bool performed = false, bool canceled = false)
        {
            if (started) inputAction.started += action;
            if (performed) inputAction.performed += action;
            if (canceled) inputAction.canceled += action;
        }

        /// <summary>
        /// 注销所有输入事件
        /// </summary>
        public static void UnRegisterInputEvent(this InputAction inputAction,
            Action<InputAction.CallbackContext> action)
        {
            inputAction.started -= action;
            inputAction.performed -= action;
            inputAction.canceled -= action;
        }
        #endif
    }
}