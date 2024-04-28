//****************** 代码文件申明 ************************
//* 文件：Level                                       
//* 作者：Koo
//* 创建时间：2024/04/19 00:35:55 星期五
//* 功能：二维地图
//*****************************************************

using System;
using KooFrame.Module;
using UnityEngine;

namespace SubSystem.Map
{
    public class Map : GridMapXZ<GridData>
    {
        public Map(int width, int height, float cellSize, Vector3 originPosition,
            Func<GridMapXZ<GridData>, int, int, GridData> createGridObject) : base(width, height, cellSize,
            originPosition, createGridObject)
        {
            
        }
    }
}