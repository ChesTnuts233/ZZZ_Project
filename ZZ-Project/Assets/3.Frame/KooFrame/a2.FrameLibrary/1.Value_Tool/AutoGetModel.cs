using System;
using UnityEngine;

namespace KooFrame
{
    [Serializable]
    public struct AutoGetModel<T> where T : Component
    {
        [SerializeField]
        private T value;


        public T Value(GameObject go)
        {
            return value == null ? go.GetComponent<T>() : value;
        }
        
        
        
    }

    
}