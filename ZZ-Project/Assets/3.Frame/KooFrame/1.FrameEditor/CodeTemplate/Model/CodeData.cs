
using KooFrame;
using System;


[Serializable]
public class CodeData
{
	/// <summary>
	/// 数据名称
	/// </summary>
	public ModelValue<string> Name = new() { ValueWithoutAction = "DefaultTemplate" };


	/// <summary>
	/// 数据ID
	/// </summary>
	public string ID;

	public virtual void UpdateData()
	{

	}

}
