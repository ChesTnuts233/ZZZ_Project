//****************** 代码文件申明 ************************
//* 文件：SetPropertyDrawer                          
//* 作者：Koo
//* 创建时间：2024/03/18 00:15:54 星期六
//* 功能：可设置属性的Drawer 学习自https://github.com/LMNRY/SetProperty/tree/master开源项目
//*****************************************************

using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace KooFrame
{
	[CustomPropertyDrawer(typeof(SetPropertyAttribute))]
	public class SetPropertyDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			//默认的属性面板绘制方法

			//在属性开始被编辑时记录当前属性的值。然后，通过调用 EditorGUI.EndChangeCheck() 方法，可以检查属性的值是否已经发生了变化。
			EditorGUI.BeginChangeCheck();
			EditorGUI.PropertyField(position, property, label);

			// 仅在必要时更新属性值
			SetPropertyAttribute setProperty = attribute as SetPropertyAttribute;
			if (EditorGUI.EndChangeCheck())
			{
				// 当 SerializedProperty 被修改时，字段的值并不会立即更新，需要标记属性为脏，
				// 以便在下一次 OnGUI 事件中更新
				setProperty.IsDirty = true;
			}
			else if (setProperty.IsDirty)
			{
				//属性被标记为脏时候更新其数值
				object parent = GetParentObjectOfProperty(property.propertyPath, property.serializedObject.targetObject);
				Type type = parent.GetType();
				PropertyInfo pi = type.GetProperty(setProperty.Name);
				if (pi == null)
				{
					Debug.LogError("Invalid property name(不存在的属性名称): " + setProperty.Name + "\nCheck your [SetProperty] attribute(请检查[SetProperty]属性)");
				}
				else
				{
					// 使用 FieldInfo 来设置属性值
					pi.SetValue(parent, fieldInfo.GetValue(parent), null);
				}
				setProperty.IsDirty = false;
			}
		}

		/// <summary>
		/// 通过递归的方式获取父对象的属性值
		/// </summary>
		/// <param name="path"></param>
		/// <param name="obj"></param>
		/// <returns></returns>
		private object GetParentObjectOfProperty(string path, object obj)
		{
			string[] fields = path.Split('.'); //将路径分割为数组

			if (fields.Length == 1)
			{
				return obj;
			}

			FieldInfo fi = obj.GetType().GetField(fields[0], BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
			obj = fi.GetValue(obj);


			return GetParentObjectOfProperty(string.Join(".", fields, 1, fields.Length - 1), obj);
		}

	}
}
