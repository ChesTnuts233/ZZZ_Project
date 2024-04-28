using System;

namespace KooFrame
{
    public interface IArchitecture
    {
        /// <summary>
        /// 注册系统层
        /// </summary>
        /// <param name="system"></param>
        /// <typeparam name="T"></typeparam>
        void RegisterSystem<T>(T system) where T : ISystem;

        /// <summary>
        /// 注册模块层
        /// </summary>
        /// <param name="model"></param>
        /// <typeparam name="T"></typeparam>
        void RegisterModel<T>(T model) where T : IModel;

        /// <summary>
        /// 注册工具层
        /// </summary>
        /// <param name="utility"></param>
        /// <typeparam name="T"></typeparam>
        void RegisterUtility<T>(T utility) where T : IUtility;

        /// <summary>
        /// 获取系统层
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetSystem<T>() where T : class, ISystem;

        /// <summary>
        /// 获取Model层
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetModel<T>() where T : class, IModel;

        /// <summary>
        /// 获取工具层
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetUtility<T>() where T : class, IUtility;

        /// <summary>
        /// 发送命令
        /// </summary>
        /// <param name="command"></param>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        TResult SendCommand<TResult>(ICommand<TResult> command);

        /// <summary>
        /// 发送命令
        /// </summary>
        /// <param name="command"></param>
        /// <typeparam name="T"></typeparam>
        void SendCommand<T>(T command) where T : ICommand;

        bool SendStructCommand(IStructCommand command);

        // void SendStructCommand<T>(T command) where T : struct;

        TResult SendQuery<TResult>(IQuery<TResult> query);


        /// <summary>
        /// 触发事件
        /// </summary>
        void TriggerEvent<T>() where T : new();

        /// <summary>
        /// 触发事件
        /// </summary>
        void TriggerEvent<T>(T e);

        /// <summary>
        /// 注册事件
        /// </summary>
        IUnRegister RegisterEvent<T>(Action<T> onEvent);

        /// <summary>
        /// 注销事件
        /// </summary>
        void UnRegisterEvent<T>(Action<T> onEvent);
    }

    public interface ICanDeInit
    {
        void DeInit();
    }
}