//****************** 代码文件申明 ************************
//* 文件：ManagerOfManager                                       
//* 作者：32867
//* 创建时间：2023/08/21 13:31:18 星期一
//* 功能：IOC容器
//*****************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;

namespace KooFrame.Manager
{
	public class IOCContainer
	{
		/// <summary>
		/// 容器实例
		/// </summary>
		[DictionaryDrawerSettings(KeyLabel = "模块", ValueLabel = "内容")]
		public Dictionary<Type, object> Instances = new Dictionary<Type, object>();

		/// <summary>
		/// 注册实例
		/// </summary>
		/// <param name="instacne">注册进来的单例</param>
		/// <typeparam name="T">单例的类型</typeparam>
		public void Register<T>(T instacne)
		{
			var key = typeof(T);

			Instances[key] = instacne;
		}

		/// <summary>
		/// 获取实例
		/// </summary>
		/// <typeparam name="T">获取的类型</typeparam>
		/// <returns>返回实例</returns>
		public T Get<T>() where T : class
		{
			var key = typeof(T);

			if (Instances.TryGetValue(key, out var retObj))
			{
				return retObj as T;
			}


			return null;
		}

		public void Set<T>(T instance) where T : class
		{
			Instances[typeof(T)] = instance;
		}

		public void Clear()
		{
			Instances.Clear();
		}

		public IEnumerable<T> GetInstancesByType<T>()
		{
			var type = typeof(T);
			return Instances.Values.Where(instance => type.IsInstanceOfType(instance)).Cast<T>();
		}
	}
}