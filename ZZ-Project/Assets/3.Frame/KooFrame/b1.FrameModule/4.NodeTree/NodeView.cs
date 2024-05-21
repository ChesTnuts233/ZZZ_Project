//****************** 代码文件申明 ************************
//* 文件：NodeView                                       
//* 作者：Koo
//* 创建时间：2024/01/13 00:15:54 星期六
//* 功能：节点编辑器内可视化的编辑节点
//*****************************************************

using System;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace KooFrame
{
    public class NodeView : UnityEditor.Experimental.GraphView.Node
    {
        public Action<NodeView> OnNodeSelected;


        public TextField NodeNameInput;

        public Node node;

        public Port RightOutPort;
        public Port RightInPort;

        public Port LeftOutPort;
        public Port LeftInPort;

        public Port UpInPort;
        public Port UpOutPort;

        public Port DownInPort;
        public Port DownOutPort;


        public Type Type;

        #region 接口元素

        private VisualElement upPortDiv;
        private VisualElement downPortDiv;
        private VisualElement rightPortDiv;
        private VisualElement leftPortDiv;

        #endregion

        public TextField fieldName;
        
        public NodeView(Node node) : base("Assets/3.Frame/KooFrame/b1.FrameModule/4.NodeTree/NodeView.uxml")
        {
            this.node = node;
            //this.title = NodeTreeEditorTool.ChangeToChinese(node.name);


            //绑定昵称修改器
            NodeNameInput = this.Q<TextField>("NodeName");

            upPortDiv = this.Q<VisualElement>("UpPortDiv");
            downPortDiv = this.Q<VisualElement>("DownPortDiv");
            rightPortDiv = this.Q<VisualElement>("RightPortDiv");
            leftPortDiv = this.Q<VisualElement>("LeftPortDiv");

            fieldName = this.Q<TextField>("FieldName");

            if (this.node.NodeNickName != "DefaultName")
            {
                NodeNameInput.SetValueWithoutNotify(this.node.NodeNickName);
            }

            NodeNameInput.RegisterValueChangedCallback((value) =>
            {
                this.node.NodeNickName = value.newValue;
                EditorUtility.SetDirty(this.node);
                AssetDatabase.SaveAssets();
            });


            this.viewDataKey = node.guid; //guid作为Node类中的viewDataKey关联 进行后续的视图层管理
            style.left = node.position.x;
            style.top = node.position.y;

            CreatePorts();
            SetNodeViewClass();
        }


        /// <summary>
        /// 设置节点样式
        /// </summary>
        private void SetNodeViewClass()
        {
            //this.AddToClassList(node);
        }


        /// <summary>
        /// 创建一个输入接口
        /// </summary>
        private void CreatePorts()
        {
            /*
             * 节点入口设置
             * 接口链接方向 横向Orientation.Vertical 竖向Orientation.Horizontal
             * 接口可链接数量 port.Capacity.Single
             * 接口类型 typeof(bool)
             */

            // 默认所有节点为多入口类型
            //如果是单向节点 节点端口类型为Single
            UpInPort = InstantiatePort(Orientation.Vertical, Direction.Input,
                node is ISingleNode ? Port.Capacity.Single : Port.Capacity.Multi, typeof(bool));

            UpOutPort = InstantiatePort(Orientation.Vertical, Direction.Output,
                node is ISingleNode ? Port.Capacity.Single : Port.Capacity.Multi, typeof(bool));

            DownInPort = InstantiatePort(Orientation.Vertical, Direction.Input,
                node is ISingleNode ? Port.Capacity.Single : Port.Capacity.Multi, typeof(bool));

            DownOutPort = InstantiatePort(Orientation.Vertical, Direction.Output,
                node is ISingleNode ? Port.Capacity.Single : Port.Capacity.Multi, typeof(bool));

            RightInPort = InstantiatePort(Orientation.Horizontal, Direction.Input,
                node is ISingleNode ? Port.Capacity.Single : Port.Capacity.Multi, typeof(bool));

            RightOutPort = InstantiatePort(Orientation.Horizontal, Direction.Output,
                node is ISingleNode ? Port.Capacity.Single : Port.Capacity.Multi, typeof(bool));

            LeftInPort = InstantiatePort(Orientation.Horizontal, Direction.Input,
                node is ISingleNode ? Port.Capacity.Single : Port.Capacity.Multi, typeof(bool));

            LeftOutPort = InstantiatePort(Orientation.Horizontal, Direction.Output,
                node is ISingleNode ? Port.Capacity.Single : Port.Capacity.Multi, typeof(bool));


            //input.style.flexDirection = FlexDirection.Column;
            UpInPort.portName = "";
            UpOutPort.portName = "";
            DownInPort.portName = "";
            DownOutPort.portName = "";
            RightInPort.portName = "";
            RightOutPort.portName = "";
            LeftInPort.portName = "";
            LeftOutPort.portName = "";

            upPortDiv.Add(UpInPort);
            upPortDiv.Add(UpOutPort);
            UpInPort.style.marginRight = new StyleLength(-42);

            downPortDiv.Add(DownInPort);
            downPortDiv.Add(DownOutPort);
            DownInPort.style.marginRight = new StyleLength(-42);

            rightPortDiv.Add(RightInPort);
            RightInPort.style.paddingLeft = new StyleLength(22);
            RightInPort.style.marginBottom = new StyleLength(-23);
            rightPortDiv.Add(RightOutPort);

            leftPortDiv.Add(LeftInPort);
            LeftInPort.style.paddingLeft = new StyleLength(22);
            LeftInPort.style.marginBottom = new StyleLength(-23);
            leftPortDiv.Add(LeftOutPort);
        }


        // /// <summary>
        // /// 创建输出端口
        // /// </summary>
        // private void CreateOutputPorts()
        // {
        //     //如果是单向节点 节点端口类型为Single
        //     output = InstantiatePort(Orientation.Vertical, Direction.Output,
        //         node is ISingleNode ? Port.Capacity.Single : Port.Capacity.Multi, typeof(bool));
        //
        //     if (output != null)
        //     {
        //         output.portName = "▼";
        //         //output.style.flexDirection = FlexDirection.ColumnReverse;
        //         outputContainer.Add(output);
        //     }
        // }

        //设置节点在节点树视图中的位置
        public override void SetPosition(Rect newPos)
        {
            Undo.RecordObject(node, "Node Tree(Set Position)");
            // 视图中节点位置设置为最新位置newPos
            base.SetPosition(newPos);
            // 将最新位置记录到运行时节点树中持久化存储
            node.position.x = newPos.xMin;
            node.position.y = newPos.yMin;

            //设置脏数据 方便撤回
            EditorUtility.SetDirty(node);
        }

        /// <summary>
        /// 重写选中方法
        /// </summary>
        public override void OnSelected()
        {
            base.OnSelected();
            //当选中的时候  treeViewer中的OnNodeSelected被传递触发
            OnNodeSelected?.Invoke(this);
            //("当前节点位置" + GetPosition().position).Log();
        }


        /// <summary>
        /// 设置节点的状态
        /// </summary>
        public void SetNodeState()
        {
            RemoveFromClassList("running");
            if (Application.isPlaying)
            {
                switch (node.state)
                {
                    case Node.State.Running:
                        if (node.started)
                        {
                            AddToClassList("running");
                        }

                        break;
                }
            }
        }

        public void SetNodeToNormalClass()
        {
            AddToClassList("normalClass");
        }

        public void SetNodeTestClass()
        {
            AddToClassList("testClass");
        }
    }
}