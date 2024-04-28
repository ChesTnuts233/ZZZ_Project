//****************** 代码文件申明 ************************
//* 文件：TankController                                       
//* 作者：Koo
//* 创建时间：2024/04/22 23:34:09 星期一
//* 功能：nothing
//*****************************************************

using KooFrame;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameBuild.TankInput
{
    public class TankInput : PlayerInputBase
    {
        public GameInputAction InputAction;

        public TankInputData InputData;

        public TankInput(InvokerBase invoker) : base(invoker)
        {
            InputData = GetInputData<TankInputData>();
            RegisterInputAction();
        }


        private void RegisterInputAction()
        {
            //InputAction.GamePlay.Move.RegisterInputEvent(MoveHandle);
            //InputAction.GamePlay.Rotate.RegisterInputEvent();
        }


        private void MoveHandle(InputAction.CallbackContext context)
        {
            InputData.Move = context.ReadValue<Vector2>();
        }

        private void RotateHandle() { }
    }
}