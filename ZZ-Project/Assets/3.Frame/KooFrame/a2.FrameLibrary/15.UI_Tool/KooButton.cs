//****************** 代码文件申明 ************************
//* 文件：KooButton                                       
//* 作者：Koo
//* 创建时间：2024/02/18 21:11:38 星期日
//* 功能：nothing
//*****************************************************

using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.UI;

namespace KooFrame
{
    public class KooButton : Button
    {
        [ShowInInspector]
        public List<TMP_Text> Texts;

        protected override void Awake()
        {
            base.Awake();
            Texts = new List<TMP_Text>();
            Texts.AddRange(GetComponentsInChildren<TMP_Text>());
        }
    }
}