//****************** 代码文件申明 ************************
//* 文件：TankController                                       
//* 作者：Koo
//* 创建时间：2024/04/22 23:34:09 星期一
//* 功能：nothing
//*****************************************************

using KooFrame;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameBuild
{
	public class CharacterInput : PlayerInputBase
	{
		public GameInputAction InputAction;

		[ShowInInspector]
		public KanojoInputData InputData;

		public CharacterInput(InvokerBase invoker) : base(invoker)
		{
			InputData = GetInputData<KanojoInputData>();
			InputAction = new GameInputAction();
			RegisterInputAction();
			InputAction.Enable();
		}

		public override void DeInit()
		{
			base.DeInit();
			UnRegisterInputAction();
		}


		public void RegisterInputAction()
		{
			InputAction.GamePlay.Move.RegisterInputEvent(MoveHandle, true, true, true);
			InputAction.GamePlay.Look.RegisterInputEvent(LookHandle, true, true, true);
			InputAction.GamePlay.MousePos.RegisterInputEvent(MousePosHandle, true, true, true);
		}


		public void UnRegisterInputAction()
		{
			InputAction.GamePlay.Move.UnRegisterInputEvent(MoveHandle);
			InputAction.GamePlay.Look.UnRegisterInputEvent(LookHandle);
			InputAction.GamePlay.MousePos.UnRegisterInputEvent(MousePosHandle);
		}

		private void MousePosHandle(InputAction.CallbackContext context)
		{
			InputData.MousePos = context.ReadValue<Vector2>();
		}

		private void LookHandle(InputAction.CallbackContext context)
		{
			InputData.Look = context.ReadValue<Vector2>();
		}

		private void MoveHandle(InputAction.CallbackContext context)
		{
			InputData.Move = context.ReadValue<Vector2>();
		}







	}
}