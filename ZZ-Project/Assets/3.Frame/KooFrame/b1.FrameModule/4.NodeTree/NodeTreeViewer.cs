//****************** 代码文件申明 ************************
//* 文件：NodeTreeViewer                                       
//* 作者：Koo
//* 创建时间：2024/01/13 00:15:03 星期六
//* 功能：节点树视图查看器
//*****************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using GameBuild;

namespace KooFrame
{
    [Serializable]
    public class NodeCopyData
    {
        [SerializeField] public List<Node> copyNode = new();
    }

    public class NodeTreeViewer : GraphView
    {
        /// <summary>
        /// 节点选中时候的Action
        /// </summary>
        public Action<NodeView> OnNodeSelected;

        //public NodeView ShowNodeView;
        public new class UxmlFactory : UxmlFactory<NodeTreeViewer, GraphView.UxmlTraits> { }


        protected NodeTree Tree;

        private bool isCopying;

        private Vector2 windowMousePosition = Vector2.zero;


        public NodeTreeViewer()
        {
            Insert(0, new GridBackground());
            //添加 Manipulator(操控器) 视图缩放
            this.AddManipulator(new ContentZoomer());
            //添加视图拖拽
            this.AddManipulator(new ContentDragger());
            //添加选中对象拖拽
            this.AddManipulator(new SelectionDragger());
            //添加框选
            this.AddManipulator(new RectangleSelector());

            //加载样式表
            var styleSheet =
                AssetDatabase.LoadAssetAtPath<StyleSheet>(
                    "Assets/3.Frame/KooFrame/b1.FrameModule/4.NodeTree/NodeTreeViewer.uss");

            styleSheets.Add(styleSheet);
            Undo.undoRedoPerformed += OnUndoRedo;


            // 添加鼠标事件监听器
            this.RegisterCallback<MouseDownEvent>(OnMouseDown);
        }

        //右键添加右键
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            // //添加Node抽象类下所有子类到右键创建栏中
            var types = TypeCache.GetTypesDerivedFrom<Node>();
            // //使用Linq 对types进行筛选
            foreach (var type in types.Where(type => !type.IsAbstract))
            {
                evt.menu.AppendAction(type.Name, (a) => { CreateNode(type); });
            }


            if (selection.Count > 0)
            {
                evt.menu.AppendAction("复制节点", (a) => { CopyNode(selection); });
            }

            if (isCopying)
            {
                evt.menu.AppendAction("黏贴节点", (a) => { PasteNode(); });
            }
        }


        protected void SetNodeViewPosToMousePos(NodeView nodeView)
        {
            var graphMousePosition = this.contentViewContainer.WorldToLocal(windowMousePosition);
            nodeView.SetPosition(new Rect(graphMousePosition, new Vector2()));
        }


        private void OnMouseDown(MouseDownEvent evt)
        {
            // 获取鼠标在GraphView中的位置
            windowMousePosition = evt.mousePosition;
        }

        protected void OnUndoRedo()
        {
            PopulateView(Tree);         //重新刷新
            AssetDatabase.SaveAssets(); //保存资源
        }


        protected NodeView CreateNode(Type type)
        {
            //创建运行时节点树上的对应类型节点
            Node node = Tree.CreateNode(type);
            return CreateNodeView(node);
        }

        /// <summary>
        /// 复制节点
        /// </summary>
        public void CopyNode(List<ISelectable> selectables)
        {
            //得到所有选中的节点
            NodeCopyData copyData = new();


            foreach (var selectable in selectables)
            {
                NodeView nodeView = (NodeView)selectable;
                copyData.copyNode.Add(nodeView.node);
            }

            string json = JsonUtility.ToJson(copyData);
            KooTool.CopyText(json);
            isCopying = true;
        }

        public void PasteNode()
        {
            Undo.RecordObject(Tree, "Node Tree(CopyNode)");
            if (isCopying == false)
            {
                return;
            }

            string json = KooTool.GetCopyText();
            "现在还复制不了 TODO".Log();
        }


        protected NodeView CreateNodeView(Node node)
        {
            //创建节点UI
            NodeView nodeView = new NodeView(node);
            // 节点创建成功后 让nodeView.OnNodeSelected与当前节点树上的OnNodeSelected关联
            nodeView.OnNodeSelected = OnNodeSelected;

            if (node.NodeNickName.Contains("Test"))
            {
                nodeView.SetNodeTestClass();
            }
            else
            {
                nodeView.SetNodeToNormalClass();
            }
            
            
            //反射类名称
            Type nodeClassType = Type.GetType("GameBuild." + node.NodeNickName);

            if (nodeClassType != null)
            {
                // 获取所有字段
                FieldInfo[] fieldsInfo = nodeClassType.GetFields(BindingFlags.Public | BindingFlags.NonPublic |
                                                                 BindingFlags.Instance | BindingFlags.Static);

                StringBuilder fields = new StringBuilder();

                foreach (var field in fieldsInfo)
                {
                    if (field.IsPublic)
                    {
                        fields.AppendLine($"+ {field.Name}");
                    }
                    else
                    {
                        fields.AppendLine($"- {field.Name}");
                    }
                }

                nodeView.fieldName.SetValueWithoutNotify(fields.ToString());
            }


            // 将对应节点UI添加到节点树视图上
            AddElement(nodeView);
            return nodeView;
        }

        private NodeView FindNodeView(Node node)
        {
            return GetNodeByGuid(node.guid) as NodeView;
        }

        /// <summary>
        /// 视图刷新
        /// </summary>
        /// <param name="nodeTree"></param>
        public void PopulateView(NodeTree nodeTree)
        {
            this.Tree = nodeTree;
            // 在节点树视图重新绘制之前需要取消视图变更方法OnGraphViewChanged的订阅
            // 以防止视图变更记录方法中的信息是上一个节点树的变更信息
            graphViewChanged -= OnGraphViewChanged;
            // 清除之前渲染的graphElements图层元素
            DeleteElements(graphElements);
            // 在清除节点树视图所有的元素之后重新订阅视图变更方法
            graphViewChanged += OnGraphViewChanged;

            Tree.nodes.ForEach(node => CreateNodeView(node));

            Tree.nodes.ForEach(node =>
            {
                var childrenList = Tree.GetChildren(node);
                childrenList.ForEach(childNode =>
                {
                    NodeView parentView = FindNodeView(node);
                    NodeView childView = FindNodeView(childNode);
                    Edge edge = parentView.DownOutPort.ConnectTo(childView.UpInPort);


                    AddElement(edge);
                });
            });
        }


        /// <summary>
        /// 当节点树视图发生变化的时候 触发对应的项目资源也发生变化
        /// </summary>
        /// <param name="graphViewChange"></param>
        /// <returns></returns>
        protected virtual GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            //对所有删除进行遍历记录 只要视图内有元素删除进行判断
            if (graphViewChange.elementsToRemove != null)
            {
                graphViewChange.elementsToRemove.ForEach(graphElement =>
                {
                    //找到节点树视图中删除的NodeView 
                    var nodeView = graphElement as NodeView;
                    if (nodeView != null)
                    {
                        //并将该NodeView所关联的运行时节点删除
                        Tree.DeleteNode(nodeView.node);
                    }

                    var edge = graphElement as Edge;
                    if (edge != null) //如果节点之前的连接线被清除了 要清除两个节点之间的父子关系
                    {
                        NodeView parentView = edge.output.node as NodeView;
                        NodeView childView = edge.input.node as NodeView;
                        if (parentView != null && childView != null)
                        {
                            Tree.RemoveChild(parentView.node, childView.node);
                        }
                    }
                });
            }

            // 如果项目资产内 没有父子关系 但视图中确连接在一起
            if (graphViewChange.edgesToCreate != null)
            {
                graphViewChange.edgesToCreate.ForEach(edge =>
                {
                    NodeView parentView = edge.output.node as NodeView;
                    NodeView childView = edge.input.node as NodeView;
                    if (parentView != null && childView != null)
                    {
                        Tree.AddChild(parentView.node, childView.node);
                    }
                });
            }

            return graphViewChange;
        }


        /// <summary>
        /// 适配端口 那些可以进行链接
        /// </summary>
        /// <param name="startPort"></param>
        /// <param name="nodeAdapter"></param>
        /// <returns></returns>
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            /*
             * 节点编辑器中 端口有输入方向 和输出方向
             * endPort.direction != startPort.direction 这里要求连接的两个端口必须有不同的方向
             * 这个条件确保了只有输入端口可以连接到输出端口
             * endPort.node != startPort.node 条件确保两个连接的端口必须属于不同的节点 防止自己连接到自己
             * 这里通过Linq 对所有可以连接的端口集合做出筛选
             */
            return ports.ToList()
                .Where(endPort => endPort.direction != startPort.direction && endPort.node != startPort.node).ToList();
        }

        /// <summary>
        /// 更新节点状态
        /// </summary>
        public void UpdateNodeStates()
        {
            nodes.ForEach(node =>
            {
                NodeView view = node as NodeView;
                view.SetNodeState();
            });
        }
    }
}