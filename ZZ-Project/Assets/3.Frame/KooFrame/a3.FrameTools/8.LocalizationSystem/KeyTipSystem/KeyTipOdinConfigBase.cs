//****************** 代码文件申明 ************************
//* 文件：KeyTipOdinConfigBase                                       
//* 作者：Koo
//* 创建时间：2024/03/08 15:43:18 星期五
//* 功能：nothing
//*****************************************************

using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace KooFrame
{
    public abstract class KeyTipOdinConfigSuperBase : OdinConfigBase_SO { }

    public abstract class KeyTipOdinConfigBase<DeviceKeyType> : KeyTipOdinConfigSuperBase where DeviceKeyType : Enum
    {
        /// <summary>
        /// 全局总配置
        /// 根据玩家操作的名称枚举 对应
        /// </summary>
        [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.CollapsedFoldout),
         LabelText("按键提示内容配置")]
        public Dictionary<PlayerControllerDeviceType, Dictionary<PlayerActionType, KeyTipData>> GlobalConfig = new();

        /// <summary>
        /// 配置操作的名称
        /// 通过此配置来生成玩家操作的名称枚举
        /// </summary>
        public List<string> tipConfigList = new();


        /// <summary>
        /// 得到按键提示数据
        /// </summary>
        /// <param name="deviceType">玩家所在平台</param>
        /// <param name="playerActionType">玩家对应的操作</param>
        /// <returns>对应的提示数据集</returns>
        public KeyTipData GetKeyTipData(PlayerControllerDeviceType deviceType, PlayerActionType playerActionType)
        {
            KeyTipData data = null;
            if (GlobalConfig.TryGetValue(deviceType, out var dic))
            {
                dic.TryGetValue(playerActionType, out data);
            }

            return data;
        }
    }
}