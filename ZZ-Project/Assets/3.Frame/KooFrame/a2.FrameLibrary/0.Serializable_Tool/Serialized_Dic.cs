using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace KooFrame
{
	/// <summary>
	/// 可序列化的字典，支持二进制和JsonUtility
	/// </summary>
	/// <remarks cref="UnityEngine.Rendering.SerializedDictionary{K,V}">
	/// 如果使用URP或HDRP渲染管线，官方提供的序列化字典
	/// </remarks>
	/// <typeparam name="K">字典Key</typeparam>
	/// <typeparam name="V">字典Value</typeparam>
	[Serializable]
	public class Serialized_Dic<K, V> : ISerializationCallbackReceiver
	{
		[SerializeField] private List<K> keyList;
		[SerializeField] private List<V> valueList;


		[NonSerialized] // 不序列化 避免报错
		private Dictionary<K, V> reallyDictionary;

		public Dictionary<K, V> Dic
		{
			get => reallyDictionary;
		}
		public Serialized_Dic()
		{
			reallyDictionary = new Dictionary<K, V>();
		}

		public Serialized_Dic(Dictionary<K, V> dictionary)
		{
			this.reallyDictionary = dictionary;
		}

		// 序列化的时候把字典里面的内容放进list
		[OnSerializing]
		private void OnSerializing(StreamingContext context)
		{
			OnBeforeSerialize();
		}

		// 反序列化时候自动完成字典的初始化
		[OnDeserialized]
		private void OnDeserialized(StreamingContext context)
		{
			OnAfterDeserialize();
		}

		/// <summary>
		/// Unity序列化前调用
		/// </summary>
		public void OnBeforeSerialize()
		{
			keyList = new List<K>(reallyDictionary.Keys);
			valueList = new List<V>(reallyDictionary.Values);
		}

		/// <summary>
		/// Unity反序列化后调用
		/// </summary>
		public void OnAfterDeserialize()
		{
			reallyDictionary = new Dictionary<K, V>();

			for (int i = 0; i < keyList.Count; i++)
				reallyDictionary.Add(keyList[i], valueList[i]);

			keyList.Clear();
			valueList.Clear();
		}
	}
}
