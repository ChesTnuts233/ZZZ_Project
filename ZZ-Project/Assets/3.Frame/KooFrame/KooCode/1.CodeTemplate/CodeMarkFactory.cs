using System;

namespace KooFrame
{
	public class CodeMarkFactory : BaseCodeFactory<CodeMarkData>
	{

		public Action OnCreateOrAddData;
		public Action OnDeleteData;

		public override void AddData(CodeMarkData data)
		{
			Datas.CodeMarks.Add(data);
			OnCreateOrAddData?.Invoke();
		}

		public override CodeMarkData CreateData()
		{
			return CreateData("DefaultData");
		}


		public CodeMarkData CreateData(string name)
		{
			//创建笔记数据
			CodeMarkData newData = new CodeMarkData();
			newData.Name.Value = name;
			Datas.CodeMarks.Add(newData);

			OnCreateOrAddData?.Invoke();

			return newData;
		}

		public override void DeleteData(CodeMarkData data)
		{
			Datas.CodeMarks.Remove(data);
			OnDeleteData?.Invoke();
		}
	}
}