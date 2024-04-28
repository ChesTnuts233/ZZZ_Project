//****************** 代码文件申明 ************************
//* 文件：GridData                                       
//* 作者：Koo
//* 创建时间：2024/04/19 00:35:15 星期五
//* 功能：网格的数据
//*****************************************************

using System;
using KooFrame.Module;

namespace SubSystem.Map
{
    [Serializable]
    public class GridData
    {
        /// <summary>
        /// 所属的地图
        /// </summary>
        public GridMapXZ<GridData> Map;

        public int X;

        public int Z;

        /// <summary>
        /// 是否不属于地图区域
        /// </summary>
        public bool IsInOutSideMap;

        /// <summary>
        /// 可行走的
        /// </summary>
        public bool IsWalkable;
        
        
        public GridData(GridMapXZ<GridData> map, int x, int z)
        {
            Map = map;
            X = x;
            Z = z;
        }
    }
}