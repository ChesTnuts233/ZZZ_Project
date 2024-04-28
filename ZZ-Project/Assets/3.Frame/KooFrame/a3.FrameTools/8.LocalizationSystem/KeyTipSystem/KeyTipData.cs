//****************** 代码文件申明 ************************
//* 文件：KeyTipData                                       
//* 作者：Koo
//* 创建时间：2024/03/08 15:48:35 星期五
//* 功能：nothing
//*****************************************************

using System;
using UnityEngine;
using UnityEngine.U2D;

namespace KooFrame
{
    [Serializable]
    public class KeyTipData
    {
        /// <summary>
        /// 按键对应的设备按键
        /// </summary>
        [SerializeField] public DeviceKeysType keyType;

        /// <summary>
        /// 按键提示的Texture
        /// </summary>
        [SerializeField] public Texture TipTexture;

        /// <summary>
        /// 按键提示的图集
        /// </summary>
        [SerializeField] public SpriteAtlas TipSpriteAtlas;

        /// <summary>
        /// 按键提示的所有精灵图片
        /// </summary>
        [SerializeField] public Sprite[] TipSprites;

        public string TMPName;
    }
}