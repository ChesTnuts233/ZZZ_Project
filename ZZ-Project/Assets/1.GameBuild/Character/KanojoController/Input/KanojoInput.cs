//****************** 代码文件申明 ************************
//* 文件：TankController                                       
//* 作者：Koo
//* 创建时间：2024/04/22 23:34:09 星期一
//* 功能：nothing
//*****************************************************

using KooFrame;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameBuild
{
	public class KanojoInput : PlayerInputBase
	{
		public GameInputAction InputAction;

		[ShowInInspector]
		public KanojoInputData InputData;

		public KanojoInput(InvokerBase invoker) : base(invoker)
		{
			InputData = GetInputData<KanojoInputData>();
			InputAction = new GameInputAction();
			RegisterInputAction();
			InputAction.Enable();
		}


		public void RegisterInputAction()
		{
			InputAction.GamePlay.Move.RegisterInputEvent(MoveHandle, true, true, true);
			//InputAction.GamePlay.Rotate.RegisterInputEvent(RotateHandle);
		}


		private void MoveHandle(InputAction.CallbackContext context)
		{
			InputData.Move = context.ReadValue<Vector2>();
		}

		private void RotateHandle() { }
	}
}