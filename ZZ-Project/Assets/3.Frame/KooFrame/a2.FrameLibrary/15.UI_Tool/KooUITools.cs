using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KooFrame
{
    public static partial class KooTool
    {
        /// <summary>
        /// 检测点击是否在UI上
        /// </summary>
        public static bool IsPointerOverUIObject()
        {
            // 检查是否有事件系统存在
            if (UnityEngine.EventSystems.EventSystem.current == null)
                return false;

            // 生成射线
            PointerEventData eventDataCurrentPosition =
                new PointerEventData(UnityEngine.EventSystems.EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            // 通过事件系统判断是否点击在 UI 上
            var results = new List<RaycastResult>();
            UnityEngine.EventSystems.EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }

        /// <summary>
        /// 屏幕空间坐标 转 本地UIPanel的Rect坐标
        /// 转换后的 localPos 用于给Panel的子物体赋值
        /// 让子物体在父物体panel上的坐标相对正确
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="screenPoint"></param>
        /// <returns></returns>
        public static Vector2 ScreenPointToUILocalPoint(RectTransform rect, Vector2 screenPoint, Camera UICamera)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, screenPoint, UICamera, out var localPos);
            // 转换后的 localPos 使用下面方法赋值
            // target 为需要使用的 UI RectTransform
            // rect 是 target.parent.GetComponent<RectTransform>()
            // 最后赋值 target.anchoredPosition = localPos;
            return localPos;
        }


        /// <summary>
        /// 统一修改一个按钮的不透明度
        /// PS: 按钮禁用时候的不透明度统一为changeValue的0.5倍
        /// 按钮的文字需要是TMP
        /// </summary>
        /// <param name="button">封装后的按钮</param>
        /// <param name="changeValue"></param>
        public static void ChangeAlpha(this KooButton button, float changeValue)
        {
            ColorBlock colors = new ColorBlock();
            colors.normalColor = new Color(button.colors.normalColor.r, button.colors.normalColor.g,
                button.colors.normalColor.b, changeValue);
            colors.pressedColor = new Color(button.colors.pressedColor.r, button.colors.pressedColor.g,
                button.colors.pressedColor.b, changeValue);
            colors.highlightedColor = new Color(button.colors.highlightedColor.r, button.colors.highlightedColor.g,
                button.colors.highlightedColor.b, changeValue);
            colors.selectedColor = new Color(button.colors.selectedColor.r, button.colors.selectedColor.g,
                button.colors.selectedColor.b, changeValue);
            colors.disabledColor = new Color(button.colors.disabledColor.r, button.colors.disabledColor.g,
                button.colors.disabledColor.b, changeValue * 0.5f);
            colors.colorMultiplier = 1f;
            button.colors = colors;
            foreach (var buttonText in button.Texts)
            {
                buttonText.color = new Color(buttonText.color.r, buttonText.color.g, buttonText.color.b, changeValue);
            }
        }
    }
}