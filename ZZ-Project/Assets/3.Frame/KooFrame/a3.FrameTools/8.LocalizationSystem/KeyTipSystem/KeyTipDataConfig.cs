//****************** 代码文件申明 ************************
//* 文件：KeyTipData                                       
//* 作者：Koo
//* 创建时间：2024/03/08 15:40:35 星期五
//* 功能：提示Data
//*****************************************************

using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEditor;

#if UNITY_EDITOR
  #endif

using UnityEngine;

namespace KooFrame
{
    [CreateAssetMenu(menuName = "KooFrame/KeyTipGlobalConfig")]
    public class KeyTipDataConfig : KeyTipOdinConfigBase<PlayerControllerDeviceType>
    {
        #if UNITY_EDITOR

        [Button("一键生成玩家操作对应按键提示表")]
        private void GeneratorKeyTipConfig()
        {
            string updateContent = "";

            // //遍历所有的
            for (var index = 0; index < tipConfigList.Count; index++)
            {
                var s = tipConfigList[index];
                updateContent += "        "; //加上空格
                updateContent += $"[LabelText(\"{s}\")] {s} = {index},\n";
            }

            KooTool.CodeGenerator_ByTag<PlayerActionType>(updateContent);


            //遍历KeyTipData 生成相关数据
            foreach (Dictionary<PlayerActionType, KeyTipData> keyTipDatas in GlobalConfig.Values)
            {
                foreach (KeyTipData keyTipData in keyTipDatas.Values)
                {
                    //如果有材质
                    if (keyTipData.TipTexture != null)
                    {
                        string path = AssetDatabase.GetAssetPath(keyTipData.TipTexture);
                        Object[] sprites = AssetDatabase.LoadAllAssetsAtPath(path);
                        keyTipData.TipSprites = new Sprite[sprites.Length - 1];
                        for (var i = 1; i < sprites.Length; i++)
                        {
                            keyTipData.TipSprites[i - 1] = sprites[i] as Sprite;
                        }

                        //材质生成TMP 富文本图集资源
                        TMP_SpriteAsset spriteAsset = ScriptableObject.CreateInstance<TMP_SpriteAsset>();

                        //spriteAsset.

                        AssetDatabase.CreateAsset(spriteAsset,
                            "Assets/8.Data/SpriteAssets/" + keyTipData.TipTexture.name + ".asset");
                    }
                }
            }


            AssetDatabase.Refresh();
        }


          #endif
    }
}