//****************** 代码文件申明 ************************
//* 文件：IPlayerBase                                       
//* 作者：Koo
//* 创建时间：2024/04/22 01:25:48 星期一
//* 功能：nothing
//*****************************************************

using KooFrame;
using UnityEngine;

namespace GameBuild
{
	public interface IPlayerBase : IArchitecture
	{
		public GameObject GO { get; }

		public MovementBase Movement { get; }

		//public AttackActionBase attackAction { get; }
		//public DeadActionBase deadAction { get; }
		//public ShootActionBase shootAction { get; }

		public RequestHandleBase RequestHandle { get; }
		public RequestReceiverBase Receiver { get; }
		public InvokerBase Invoker { get; }
		public PlayerInputBase Input { get; }



	}
}