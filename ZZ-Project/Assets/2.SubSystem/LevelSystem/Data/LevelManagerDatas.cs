//****************** 代码文件申明 ************************
//* 文件：LevelManagerDatas                                       
//* 作者：Koo
//* 创建时间：2024/02/18 23:40:11 星期日
//* 功能：关卡的编辑数据
//*****************************************************

using System.Collections.Generic;
using KooFrame;
using Sirenix.OdinInspector;
using SubSystem.Map;
using UnityEngine;

namespace GameEditor.Data
{
    [CreateAssetMenu(fileName = "LevelManagerDatas", menuName = "ScriptableObject/LevelManagerDatas")]
    public class LevelManagerDatas : ConfigBase_SO
    {
        /// <summary>
        /// 这个项目的Scene存在文件夹的路径
        /// </summary>
        public string SceneSavePath;

        /// <summary>
        /// 所有的关卡数据
        /// </summary>
        [SerializeField] private List<LevelData> datas = new();

        /// <summary>
        /// 这里存储了所有已有的LevelID
        /// </summary>
        [SerializeField] public List<string> LevelIDs = new();

        /// <summary>
        /// 目前所有的地图数据
        /// </summary>
        [SerializeField] public List<MapData> MapDatas;


        public List<LevelData> Datas => datas;

#if UNITY_EDITOR

        [SerializeField, LabelText("当前所在场景ID")] public string CurLevelID;

#endif

        /// <summary>
        /// 通过关卡ID返回关卡名称
        /// </summary>
        /// <param name="levelID"></param>
        /// <returns></returns>
        public string GetLevelNameByID(string levelID)
        {
            foreach (var levelData in datas)
            {
                if (levelData.LevelID == levelID)
                {
                    return levelData.LevelName;
                }
            }

            return "";
        }

        public void AddData(LevelData levelData)
        {
            datas.Add(levelData);
        }
    }
}