using UnityEngine;

namespace KooFrame
{
    /// <summary>
    /// 注销触发器使用简化
    /// </summary>
    public static class UnRegisterExtension
    {
        /// <summary>
        /// 利用静态拓展使注销触发器的使用简化
        /// </summary>
        public static IUnRegister UnRegisterWhenGameObjectDestroyed(this IUnRegister unRegister, GameObject gameObject)
        {
            var trigger = gameObject.GetComponent<UnRegisterOnDestroyTrigger>();

            if (!trigger)
            {
                //触发器为空就直接添加一个触发器
                trigger = gameObject.AddComponent<UnRegisterOnDestroyTrigger>();
            }

            //添加注销事件
            trigger.AddUnRegister(unRegister);

            return unRegister;
        }
        
        public static IUnRegister UnRegisterWhenDisabled(this IUnRegister unRegister,
            UnityEngine.GameObject gameObject)
        {
            var trigger = gameObject.GetComponent<UnRegisterOnDisableTrigger>();

            if (!trigger)
            {
                trigger = gameObject.AddComponent<UnRegisterOnDisableTrigger>();
            }

            trigger.AddUnRegister(unRegister);

            return unRegister;
        }
        
        public static IUnRegister UnRegisterWhenGameObjectDestroyed<T>(this IUnRegister self, T component)
            where T : Component
        {
            return self.UnRegisterWhenGameObjectDestroyed(component.gameObject);
        }
        
        public static IUnRegister UnRegisterWhenDisabled<T>(this IUnRegister self, T component)
            where T : UnityEngine.Component
        {
            return self.UnRegisterWhenDisabled(component.gameObject);
        }
    }
}