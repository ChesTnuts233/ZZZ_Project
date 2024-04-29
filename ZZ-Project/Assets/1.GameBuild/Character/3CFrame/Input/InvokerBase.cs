//****************** 代码文件申明 ************************
//* 文件：InvokerBase                                       
//* 作者：Koo
//* 创建时间：2024/04/22 21:40:49 星期一
//* 功能：nothing
//*****************************************************

using System.Collections.Generic;

namespace GameBuild
{
	public abstract class InvokerBase
	{
		protected List<int> commandList;
		protected RequestReceiverBase receiver;

		public InvokerBase(RequestReceiverBase receiver)
		{
			this.receiver = receiver;
			this.commandList = new List<int>();
		}

		public abstract void Call(int requestID);

		public void Update()
		{
			OnUpdate();
		}

		public void FixedUpdate()
		{
			OnFixedUpdate();
		}

		protected virtual void OnUpdate() { }
		protected virtual void OnFixedUpdate() { }
	}
}