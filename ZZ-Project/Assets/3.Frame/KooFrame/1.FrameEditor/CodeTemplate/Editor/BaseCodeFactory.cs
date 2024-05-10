using UnityEditor;

public abstract class BaseCodeFactory<T> where T : class
{
	private KooCodeDatas codeDatas;

	public KooCodeDatas Datas => codeDatas;

	public BaseCodeFactory()
	{
		codeDatas = AssetDatabase.LoadAssetAtPath<KooCodeDatas>("Assets/3.Frame/KooFrame/1.FrameEditor/CodeTemplate/Data/CodeDatas.asset");
	}


	public abstract T CreateData();

	public abstract void DeleteData(T data);

	public abstract void AddData(T data);

}