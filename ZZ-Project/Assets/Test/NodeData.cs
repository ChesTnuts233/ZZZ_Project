//
// //****************** 代码文件申明 ************************
// //* 文件：NodeDataInOwner                                       
// //* 作者：Koo
// //* 创建时间：2024/04/04 00:08:02 星期四
// //* 功能：nothing
// //*****************************************************
//
// using System;
// using System.Collections.Generic;
// using System.Reflection;
// using KooFrame;
// using UnityEngine;
// using UnityEngine.Events;
//
// namespace SubSystem
// {
//     [Serializable]
//     public class NodeDataInOwner
//     {
//         //public Dictionary<string, MethodInfo> methodsDic = new();
//
//         [SerializeField, HideInInspector] public List<string> methodsTypeNames = new();
//
//         [SerializeField] public UnityEvent Event = new UnityEvent();
//
//         /// <summary>
//         /// 运行时 会根据这个名字 对Event加委托监听
//         /// </summary>
//         [SerializeField] public string CurRegisterMethodName;
//
//
//         [SerializeField] public int OwnerDataID;
//
//         [SerializeField] public TreeOwner Owner;
//
//
//         [SerializeField] public GameObject EventTrigger;
//
//         /// <summary>
//         /// 绑定的Node
//         /// </summary>
//         [SerializeField] public Node BindNode;
//
//         public NodeDataInOwner(Node node, TreeOwner owner)
//         {
//             BindNode = node;
//             Owner = owner;
//             OwnerDataID = Animator.StringToHash(owner.name +
//                                                 (owner.NodeDatas.Count)
//                                                 .ToString());
//         }
//
//
//         public void EventAddListener(MethodInfo methodInfo, Type invokeObj)
//         {
//             UnityAction methodDelegate = () =>
//                 methodInfo.Invoke(
//                     EventTrigger.GetComponent(invokeObj), null);
//             CurRegisterMethodName = invokeObj.FullName + "/" + methodInfo.Name;
//             Event.AddListener(methodDelegate);
//         }
//     }
// }