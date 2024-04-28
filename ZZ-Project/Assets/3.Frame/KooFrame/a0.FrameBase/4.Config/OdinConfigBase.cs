//****************** 代码文件申明 ************************
//* 文件：ConfigBase_SO                      
//* 作者：32867
//* 创建时间：2023年08月22日 星期二 17:12
//* 功能：基于Odin的配置基类
//*****************************************************

using Sirenix.OdinInspector;
using UnityEngine;

namespace KooFrame
{
    /// <summary>
    /// 用于玩家属性 道具属性的配置
    /// </summary>
    public class OdinConfigBase : SerializedScriptableObject { }

    /// <summary>
    /// 非Odin的属性配置
    /// </summary>
    public class ConfigBase_SO : ScriptableObject { }
}