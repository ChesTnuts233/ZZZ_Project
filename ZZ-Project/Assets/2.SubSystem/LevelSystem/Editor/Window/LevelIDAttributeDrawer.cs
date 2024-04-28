//****************** 代码文件申明 ************************
//* 文件：LevelIDAttributeDrawer                                       
//* 作者：wheat
//* 创建时间：2024/02/26 07:27:00 星期一
//* 描述：可以显示LevelID的选项
//*****************************************************

using UnityEngine;
using KooFrame.BaseSystem;
using System;
using System.Collections.Generic;
using GameEditor;
using GameEditor.Data;
using KooFrame;
using UnityEditor;

namespace GameBuild
{
    [CustomPropertyDrawer(typeof(LevelIDAttribute))]
    public class LevelIDAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            //如果是string值类型
            if (property.propertyType == SerializedPropertyType.String)
            {
                //那就显示
                EditorGUI.LabelField(new Rect(position.x, position.y, position.width / 3f, position.height), label);

                //显示按钮选项
                string btnLabel = string.IsNullOrEmpty(property.stringValue) ? "点击选择一个LevelID" : property.stringValue;
                if (GUI.Button(
                        new Rect(position.x + position.width / 3f, position.y, position.width / 3f * 2f,
                            position.height), btnLabel))
                {
                    //是否是在LevelData中进行修改LevelID 
                    bool isChangeLevelData = property.serializedObject.targetObject.GetType() == typeof(LevelData);
                    //显示编辑窗口
                    LevelIDEditorWindow.ShowWindow((levelID) =>
                    {
                        //赋值
                        property.stringValue = levelID;
                        //应用修改
                        property.serializedObject.ApplyModifiedProperties();
                        
                    }, isChangeLevelData);
                }
            }
            else
            {
                //不是string类型
                EditorGUI.LabelField(new Rect(position.x, position.y, position.width, position.height),
                    label + $"   {nameof(LevelIDAttribute)}这个特性只能加在String类型上");
            }

            EditorGUI.EndProperty();
        }
    }
}