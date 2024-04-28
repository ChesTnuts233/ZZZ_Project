using System;
using System.Collections.Generic;
using GameEditor.Data;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class SceneData
{
    /// <summary>
    /// 场景的路径
    /// 这个路径是Addressable标记的最初记录路径
    /// </summary>
    [SerializeField, FilePath] public string ScenePath;

    [SerializeField] public string SceneName;

    [SerializeField] public int SceneBuildIndix;

    [SerializeField, LabelText("拥有的区域")] public List<AreaData> AreaDatas;
    
    
    
    
    public SceneData(Scene scene)
    {
        ScenePath = scene.path;
        SceneName = scene.name;
        SceneBuildIndix = scene.buildIndex;
    }


    public override string ToString()
    {
        return SceneName + ",Path:" + SceneName + "BuildIn:" + SceneBuildIndix;
    }
}