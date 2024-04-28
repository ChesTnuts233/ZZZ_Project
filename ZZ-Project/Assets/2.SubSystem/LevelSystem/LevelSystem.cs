//****************** 代码文件申明 ************************
//* 文件：LevelSystem                                       
//* 作者：Koo
//* 创建时间：2024/04/19 22:07:37 星期五
//* 功能：nothing
//*****************************************************

using System.Collections.Generic;
using GameEditor.Data;
using KooFrame;
using UnityEngine;

namespace SubSystem.LevelSystem
{
    /// <summary>
    /// 关卡中心
    /// </summary>
    public class LevelSystem
    {
        private static string _curLevelID;

        /// <summary>
        /// 当前关卡ID
        /// </summary>
        public static string CurLevelID
        {
            get
            {
                if (_curLevelID.IsNullOrEmpty())
                {
                    _curLevelID = ManagerDatas.CurLevelID;
                }

                return _curLevelID;
            }
            set
            {
                if (_curLevelID.IsNullOrEmpty())
                {
                    _curLevelID = ManagerDatas.CurLevelID;
                }

                _curLevelID = value;
            }
        }


        /// <summary>
        /// 关卡管理的Datas的存放路径
        /// </summary>
        public static string LevelManagerDataPath = "Assets/2.SubSystem/LevelSystem/SO";


        /// <summary>
        /// 关卡管理的数据
        /// </summary>
        private static LevelManagerDatas _managerDatas;

        public static LevelManagerDatas ManagerDatas
        {
            get
            {
                if (_managerDatas == null)
                {
                    _managerDatas = ResSystem.LoadAsset<LevelManagerDatas>(LevelManagerDataPath);
                }

                return _managerDatas;
            }
        }


        /// <summary>
        /// 所有的关卡数据
        /// </summary>
        [SerializeField] private List<LevelData> datas = new();


        //加载所有的关卡数据
    }
}