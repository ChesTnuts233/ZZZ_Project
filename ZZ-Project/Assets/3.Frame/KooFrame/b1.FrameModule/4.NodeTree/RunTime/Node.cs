//****************** 代码文件申明 ************************
//* 文件：Node                                       
//* 作者：Koo
//* 创建时间：2024/01/11 22:09:09 星期四
//* 功能：节点
//*****************************************************

using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace KooFrame
{
	public abstract class Node : ScriptableObject
	{
		public enum State
		{
			Running,
			Waiting
		}

		/// <summary>
		/// 所在的树
		/// </summary>
		public NodeTree nodeTree;

		public Node NextFirstNode
		{
			get
			{
				if (Childs.Count >= 1 && Childs[0] != null)
				{
					return Childs[0];
				}

				return null;
			}
		}

		/// <summary>
		/// 此节点的别名
		/// </summary>
		public string NodeNickName = "默认节点";


		public State state = State.Waiting;

		public bool started = false;

		[ShowInInspector] public string guid;

		[HideInInspector] public Vector2 position;


		public List<Node> Childs = new();


		public Node OnUpdate()
		{
			if (!started)
			{
				OnStart();
				started = true;
			}

			Node currentNode = LogicUpdate();

			if (state != State.Running)
			{
				OnStop();
				started = false;
			}

			//将获取到的节点 返回出去
			return currentNode;
		}

		public abstract Node LogicUpdate();

		public abstract void OnStart();

		public abstract void OnStop();


		public abstract Node EnterNextNode();



		public void Stop()
		{
			state = State.Waiting;
		}


		//#if UNITY_EDITOR
		//        [Button("强制删除子SO自身 防止删除错误")]
		//        public void DeleteMe()
		//        {
		//            AssetDatabase.RemoveObjectFromAsset(this);
		//            AssetDatabase.SaveAssets();
		//            AssetDatabase.Refresh();
		//        }
		//#endif

		public abstract void CopyDataFrom(Node node);
	}
}