using System;
using System.Collections.Generic;

namespace KooFrame
{
    public class TypeEventSystem : ITypeEventSystem
    {
        /// <summary>
        /// 注册接口
        /// </summary>
        interface IRegistrations
        {
        }

        class Registrations<T> : IRegistrations
        {
            /// <summary>
            /// 空委托(本身可以一对多注册)
            /// </summary>
            public Action<T> OnEvent = obj => { };
        }

        class Registrations<T, T1> : IRegistrations
        {
            public Action<T, T1> OnEvent = (obj, obj1) => { };
        }


        private Dictionary<Type, IRegistrations> eventRegistrationsMap = new();

        public void Trigger<T>() where T : new()
        {
            var e = new T();
            Trigger<T>(e);
        }

        public void Trigger<T>(T e)
        {
            var type = typeof(T);
            IRegistrations eventRegistrations;
            if (eventRegistrationsMap.TryGetValue(type, out eventRegistrations))
            {
                (eventRegistrations as Registrations<T>)?.OnEvent.Invoke(e);
            }
        }

        public IUnRegister Register<T>(Action<T> onEvent)
        {
            var type = typeof(T);
            IRegistrations eventRegistrations;

            //字典中不存在事件的时候 新添加
            if (!eventRegistrationsMap.TryGetValue(type, out eventRegistrations))
            {
                eventRegistrations = new Registrations<T>();
                eventRegistrationsMap.Add(type, eventRegistrations);
            }

            ((Registrations<T>)eventRegistrations).OnEvent += onEvent;

            return new TypeEventSystemUnRegister<T>()
            {
                OnEvent = onEvent,
                TypeEventSystem = this
            };
        }
        

        public void UnRegister<T>(Action<T> onEvent)
        {
            var type = typeof(T);
            IRegistrations eventRegistrations;

            if (eventRegistrationsMap.TryGetValue(type, out eventRegistrations))
            {
                ((Registrations<T>)eventRegistrations).OnEvent -= onEvent;
            }
        }

        
    }
}