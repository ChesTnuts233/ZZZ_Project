using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace GameBuild.SubSystem
{
    public class BezierManager : MonoBehaviour
    {
        /// <summary>
        /// 当前编辑用的曲线
        /// </summary>
        [SerializeField]
        private SimpleBezierCurvePath editorPath;

#pragma warning disable
        /// <summary>
        /// 细分度
        /// </summary>
        [SerializeField, Range(10, 100), LabelText("细分度"), Tooltip("细分度决定了生成参数值的数量多少")]
        private int editorSegments = 20;
#pragma warning enable

        /// <summary>
        /// 坐标集合
        /// </summary>
        public List<Vector2> PosList = new List<Vector2>();

#if UNITY_EDITOR
        /// <summary>
        /// 创建一条贝塞尔曲线
        /// </summary>
        [Button("创建一条曲线")]
        public void CreateBezier(string curveName = "EditorPath")
        {
            GameObject pathGO = new GameObject();

            pathGO.name = curveName;

            pathGO.transform.parent = transform;

            pathGO.hideFlags = HideFlags.NotEditable;

            editorPath = pathGO.AddComponent<SimpleBezierCurvePath>();

            editorPath.berBezierManager = this;

            editorPath.Curve = new BezierCurveSerializable()
            {
                points =
                {
                    [0] = new BezierCurvePoint(Vector2.zero,
                        Vector2.right * 0.5f),
                    [1] = new BezierCurvePoint(Vector2.one,
                        Vector2.down * 0.5f)
                },
                segments = editorSegments
            };


            //生成一次坐标集合
            PosList = editorPath.GeneterPositionByEvaluate();

            Selection.activeGameObject = pathGO;
        }


        [Button("保存曲线数据")]
        public void SaveBezierCurveData()
        {
            if (editorPath == null)
            {
                Debug.Log("还没创建曲线");
                return;
            }
            
            BezierData pathData = ScriptableObject.CreateInstance<BezierData>();

            pathData.poslist = PosList;

            pathData.xList = PosList.Select(pos => pos.x).ToList();
            pathData.yList = PosList.Select(pos => pos.y).ToList();

            string name = editorPath.gameObject.name;

            string path = "Assets/8.Data/BezierData/" + name + ".asset";

            Debug.Log("数据保存到了" + path + "的SO文件里啦！");

            AssetDatabase.CreateAsset(pathData, path);
            AssetDatabase.Refresh();
        }
#endif
    }
}