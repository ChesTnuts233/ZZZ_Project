using System.Collections.Generic;
using KooFrame;
using UnityEngine;

namespace GameBuild.SubSystem
{
    public enum BezierType
    {
        One,   //一阶贝塞尔
        Two,   //二阶贝塞尔
        Three, //三阶贝塞尔
    }

    /// <summary>
    /// 曲线的使用情况 
    /// </summary>
    public enum BezierUseType
    {
        TwoDimension,
        ThreeDimension
    }

    /// <summary>
    /// 贝塞尔曲线
    /// </summary>
    public class BezierCurve
    {
        // /// <summary>
        // /// 贝塞尔曲线类型
        // /// </summary>
        // private BezierType _bezierType = BezierType.Three;

        /// <summary>
        /// 使用类型
        /// </summary>
        private BezierUseType _userType;

        /// <summary>
        /// 使用二维的时候可以使用编辑器来定义曲线
        /// </summary>
        public AnimationCurve widthCurve;

        /// <summary>
        /// 段数
        /// </summary>
        [Range(1, 100)]
        public int segments = 10;

        /// <summary>
        /// 是否循环
        /// </summary>
        public bool loop;

        /// <summary>
        /// 点集合
        /// 默认初始化两个节点
        /// </summary>
        public List<BezierCurvePoint> points = new List<BezierCurvePoint>(2);


        public BezierCurve(BezierCurvePoint startPoint, BezierCurvePoint endPoint, int segments)
        {
            points.Add(startPoint);
            points.Add(endPoint);
            this.segments = segments;
        }


        /// <summary>
        /// 根据归一化位置值获取对应的贝塞尔曲线上的点
        /// </summary>
        /// <param name="value">归一化位置值 [0,1]</param>
        /// <returns></returns>
        public Vector3 EvaluatePosition(float value)
        {
            //返回的结果坐标
            Vector3 retVal = Vector3.zero;
            if (points.Count > 0)
            {
                //max表示了控制点的最大索引 用于保准标准化的value在有效的范围内
                float max = points.Count - 1 < 1 ? 0 : (loop ? points.Count : points.Count - 1);
                //如果是循环的 标准值会取模 保准在0 到 max之间 不循环
                float standardized = (loop && max > 0)
                    ? ((value %= max) + (value < 0 ? max : 0))
                    : Mathf.Clamp(value, 0, max);
                int rounded = Mathf.RoundToInt(standardized);
                //i1 i2 这两个控制点是用来标识是那两个控制点索引之间的位置
                int i1, i2;
                // Mathf.Epsilon  是一个大于0的最小浮点数

                #region 什么是Mathf.Epsilon

                // 此函数遵循以下规则：
                // anyValue + Epsilon = anyValue
                // anyValue - Epsilon = anyValue
                // 0 + Epsilon = Epsilon
                // 0 - Epsilon = -Epsilon
                // 任意数 与 Epsilon 的之间值将导致在任意数发生舍位误差（truncating errors）

                #endregion

                //standardized是否非常接近rounded 如果是 那么直接为rounded这个整数
                if (Mathf.Abs(standardized - rounded) < Mathf.Epsilon)
                {
                    i1 = i2 = (rounded == points.Count) ? 0 : rounded;
                }
                else
                {
                    //i1为standardized的前一个整数值 开始控制点
                    i1 = Mathf.FloorToInt(standardized);
                    //如果i1的数值超过控制点数量 则减去max这个最大索引
                    if (i1 >= points.Count)
                    {
                        standardized -= max;
                        i1 = 0;
                    }

                    //i2为standardized的后一个整数值 结束控制点
                    i2 = Mathf.CeilToInt(standardized);
                    //i2是否超过控制点数量 超过就设置为0
                    i2 = i2 >= points.Count ? 0 : i2;
                }

                if (i1 == i2) //如果两个点相等 直接返回控制点坐标
                    retVal = points[i1].position;
                else
                    retVal = KooTool.Bezier3(
                        points[i1].position, //起点
                        points[i1].position + points[i1].tangent,  //起点控制点
                        points[i2].position + points[i2].tangent,  //终点控制点
                        points[i2].position, //终点
                        standardized - i1);
            }

            return retVal;
        }
    }
}