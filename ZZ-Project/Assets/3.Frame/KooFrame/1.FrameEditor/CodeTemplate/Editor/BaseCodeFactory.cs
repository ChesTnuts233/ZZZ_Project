using KooFrame;
using UnityEditor;

public abstract class BaseCodeFactory<T> where T : class
{

	public KooCodeDatas Datas => KooCode.Datas;


	public abstract T CreateData();

	public abstract void DeleteData(T data);

	public abstract void AddData(T data);

}