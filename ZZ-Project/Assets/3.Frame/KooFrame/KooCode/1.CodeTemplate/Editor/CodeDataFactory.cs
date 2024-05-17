using KooFrame;
using System;
using UnityEditor;
using UnityEngine;
/// <summary>
/// 代码模板工厂
/// </summary>
public class CodeDataFactory : BaseCodeFactory<CodeData>
{

	public Action OnCreateOrAddData;
	public Action OnDeleteData;

	public override void AddData(CodeData data)
	{
		Datas.CodeDatas.Add(data);
		OnCreateOrAddData?.Invoke();
		UpdateAndSave();
	}


	public override CodeData CreateData()
	{
		return CreateData("DefaultTemplate");
	}

	/// <summary>
	/// 创建脚本模板
	/// </summary>
	/// <returns>模板数据</returns>
	public CodeData CreateData(string name)
	{
		CodeData createDate = new CodeData(name);
		Datas.CodeDatas.Add(createDate);

		OnCreateOrAddData?.Invoke();

		UpdateAndSave();
		return createDate;
	}

	/// <summary>
	/// 创建脚本模板
	/// </summary>
	/// <param name="content"></param>
	/// <param name="sourcefile"></param>
	/// <param name="name"></param>
	public CodeData CreateData(string content, TextAsset sourcefile, string name = "DefaultData")
	{
		CodeData createDate = new CodeData(name, content, sourcefile);
		Datas.CodeDatas.Add(createDate);

		OnCreateOrAddData?.Invoke();

		UpdateAndSave();
		return createDate;
	}



	public override void DeleteData(CodeData data)
	{
		Datas.CodeDatas.Remove(data);

		OnDeleteData?.Invoke();

		UpdateAndSave();
	}

	private void UpdateAndSave()
	{
		////更新MenuItem
		//UpdateTemplateMenuItem();

		EditorUtility.SetDirty(Datas);
		//AssetDatabase.SaveAssets();
	}


	private void UpdateTemplateMenuItem()
	{
		
	}

}
