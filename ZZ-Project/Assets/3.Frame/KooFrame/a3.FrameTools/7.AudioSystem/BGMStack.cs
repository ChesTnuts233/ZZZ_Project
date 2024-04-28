//****************** 代码文件申明 ************************
//* 文件：BGMStack                                       
//* 作者：wheat
//* 创建时间：2024/01/20 10:29:43 星期六
//* 描述：存储BGM相关的信息
//*****************************************************
using UnityEngine;
using KooFrame.BaseSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace KooFrame
{
    [System.Serializable]
    public class BGMStack
    {
        [field: SerializeField, LabelText("BGMid")] public int BGMIndex { get; private set; }
        [field: SerializeField, LabelText("BGM名称")] public string BGMName { get; private set; }
        [field: SerializeField, LabelText("音乐Clip")] public List<BGMClipStack> Clips { get; private set; }

        public BGMStack(int bgmIndex, string bgmName, List<BGMClipStack> clips)
        {
            BGMIndex = bgmIndex;
            BGMName = bgmName;
            Clips = clips;
        }
    }
}

