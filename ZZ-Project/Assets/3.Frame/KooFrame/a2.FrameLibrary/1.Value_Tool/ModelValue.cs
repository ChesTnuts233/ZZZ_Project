using System;
using UnityEngine;


namespace KooFrame
{
	/// <summary>
	/// 检测数据绑定
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[Serializable]
	public struct ModelValue<T>
	{
		[SerializeField] private T _value;

		public T Value
		{
			get => _value;
			set
			{
				if (!value.Equals(_value)) OnValueChange?.Invoke(value);
				_value = value;
			}
		}

		public T ValueWithoutAction
		{
			set => _value = value;
		}

		public event Action<T> OnValueChange;

		public static implicit operator T(ModelValue<T> property)
		{
			return property.Value;
		}

		public void SetValueWithoutAction(T value)
		{
			_value = value;
		}

		public void CleanActionMethod()
		{
			OnValueChange = null;
		}


	}
}