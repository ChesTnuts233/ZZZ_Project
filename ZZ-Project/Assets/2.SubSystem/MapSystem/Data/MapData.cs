//****************** 代码文件申明 ************************
//* 文件：MapData                                       
//* 作者：Koo
//* 创建时间：2024/04/19 00:32:54 星期五
//* 功能：nothing
//*****************************************************

using KooFrame;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SubSystem.Map
{
    public class MapData : ConfigBase_SO
    {
        /// <summary>
        /// 所在的Level
        /// </summary>
        public string LevelID;


        [LabelText("网格宽度")] public int gridWidth;
        [LabelText("网格高度")] public int gridHeight;
        [LabelText("网格原点")] public Vector2Int gridOrigin;

        [SerializeField] public Map Map;


        //[Header("网格属性，请用TilemapGridProperties来设置"), Tooltip("网格属性，请用TilemapGridProperties来设置")]
        //public List<GridPropertyDetails> GridPropertyDetails = new List<GridPropertyDetails>();
    }
}