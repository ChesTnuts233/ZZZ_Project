//****************** 代码文件申明 ************************
//* 文件：NodeTree                                       
//* 作者：Koo
//* 创建时间：2024/01/11 22:18:51 星期四
//* 功能：节点树
//*****************************************************

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace KooFrame
{
    public class NodeTree : ScriptableObject
    {
        /// <summary>
        /// 根节点
        /// </summary>
        public Node rootNode;

        /// <summary>
        /// 运行时节点
        /// </summary>
        public Node runningNode;


        /// <summary>
        /// 节点树的状态
        /// </summary>
        public Node.State treeState = Node.State.Waiting;

        /// <summary>
        /// 这个树所拥有的所有的节点
        /// </summary>
        public List<Node> nodes = new List<Node>();

        /// <summary>
        /// 是否完成初始化
        /// </summary>
        public bool isInited;


        /// <summary>
        /// 树初始化
        /// </summary>
        public virtual void Init()
        {
            //重置树和所有节点的状态
            treeState = Node.State.Waiting;
            runningNode = rootNode;
            foreach (var node in nodes)
            {
                node.state = Node.State.Waiting;
                node.started = false;
            }

            isInited = true;
        }


#if UNITY_EDITOR

        public virtual Node CreateNode(System.Type type)
        {
            Undo.RecordObject(this, "Node Tree(CreateNode)");

            Node node = ScriptableObject.CreateInstance(type) as Node;
            node.name = type.Name;
            node.nodeTree = this;
            node.guid = GUID.Generate().ToString();
            nodes.Add(node);


            if (!Application.isPlaying)
            {
                AssetDatabase.AddObjectToAsset(node, this);
            }

            // //判断是否是根节点 当前没节点 不是废话节点 不是自言自语节点
            // if (this.nodes.Count == 1 && node is not NoUseDialogue && node is not SoliloquyNode)
            // {
            //     this.rootNode = node;
            // }


            //保存资源
            Undo.RegisterCreatedObjectUndo(node, "Node Tree (CreateNode)");
            AssetDatabase.SaveAssets();
            return node;
        }

        /// <summary>
        /// 移出节点
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public virtual Node DeleteNode(Node node)
        {
            Undo.RecordObject(this, "Node Tree (DeleteNode)");
            //总节点列表移出节点
            nodes.Remove(node);
            // //移出资源列表
            // AssetDatabase.RemoveObjectFromAsset(node);  //在Undo中管理了
            //保存当前项目中的所有修改过的资产（Assets）到磁盘上的项目文件
            Undo.DestroyObjectImmediate(node);

            AssetDatabase.SaveAssets();
            return node;
        }

        public void RemoveChild(Node parent, Node child)
        {
            Undo.RecordObject(parent, "Node Tree (RemoveChild)");
            parent.Childs.Remove(child);
            EditorUtility.SetDirty(parent);
        }

        public void AddChild(Node parent, Node child)
        {
            Undo.RecordObject(parent, "Node Tree (AddChild)");
            parent.Childs.Add(child);

            //更新根节点和运行中的节点
            if (nodes[0] != null)
            {
                rootNode = nodes[0];
            }


            EditorUtility.SetDirty(parent);
        }


#endif

        public virtual void Update()
        {
            //当树状态为运行的时候 并且 运行时的节点状态也为running
            if (treeState == Node.State.Running && runningNode.state == Node.State.Running)
            {
                runningNode = runningNode.OnUpdate();
            }
        }

        /// <summary>
        /// 对话树开始时
        /// </summary>
        public virtual void OnTreeStart()
        {
            treeState = Node.State.Running;
            
        }

        /// <summary>
        /// 对话树结束时
        /// </summary>
        public virtual void OnTreeEnd()
        {
            treeState = Node.State.Waiting;
        }

        public List<Node> GetChildren(Node parent)
        {
            return parent.Childs;
        }
    }
}