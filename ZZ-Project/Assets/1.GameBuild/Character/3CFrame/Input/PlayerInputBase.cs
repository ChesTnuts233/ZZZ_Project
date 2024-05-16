//****************** 代码文件申明 ************************
//* 文件：PlayerInputBase                                       
//* 作者：Koo
//* 创建时间：2024/04/22 21:40:08 星期一
//* 功能：nothing
//*****************************************************

namespace GameBuild
{
	public class PlayerInputBase
	{
		protected InvokerBase invoker;
		protected InputData inputData;


		public PlayerInputBase(InvokerBase invoker)
		{
			this.invoker = invoker;
		}


		public void Update() { }

		protected virtual void Init() { }

		public virtual void DeInit() { }


		protected virtual void GetInputAxis() { }

		protected virtual void EndOfUpdate() { }

		protected virtual void FixedCallCommand() { }

		protected virtual void CallCommand() { }

		public T GetInputData<T>() where T : InputData, new()
		{
			inputData ??= new T();
			return (T)inputData;
		}
	}
}