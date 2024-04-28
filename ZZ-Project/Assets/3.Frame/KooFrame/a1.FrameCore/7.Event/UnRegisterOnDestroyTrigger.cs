using System.Collections.Generic;
using UnityEngine;

namespace KooFrame
{
    /// <summary>
    /// 注销的触发器
    /// </summary>
    public class UnRegisterTrigger : MonoBehaviour
    {
        private HashSet<IUnRegister> unRegistersHash = new HashSet<IUnRegister>();

        public void AddUnRegister(IUnRegister unRegister) => unRegistersHash.Add(unRegister);

        /// <summary>
        /// 移除注销
        /// </summary>
        /// <param name="unRegister"></param>
        public void RemoveUnRegister(IUnRegister unRegister) => unRegistersHash.Remove(unRegister);

        public void UnRegister()
        {
            foreach (var unRegister in unRegistersHash)
            {
                unRegister.UnRegister();
            }

            unRegistersHash.Clear();
        }
    }

    public class UnRegisterOnDestroyTrigger : UnRegisterTrigger
    {
        private void OnDestroy()
        {
            UnRegister();
        }
    }

    public class UnRegisterOnDisableTrigger : UnRegisterTrigger
    {
        private void OnDisable()
        {
            UnRegister();
        }
    }
}