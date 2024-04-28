//****************** 代码文件申明 ************************
//* 文件：TankMove                                       
//* 作者：Koo
//* 创建时间：2024/04/23 11:39:29 星期二
//* 功能：坦克移动
//*****************************************************

using UnityEngine;

namespace GameBuild
{
    public class TankMove : MovementBase
    {
        public TankMove(MonoBehaviour mono, Rigidbody rb, Transform inputSpace, Transform player, Animator anim) : base(
            mono, rb, inputSpace, player, anim) { }

        private TankInputData inputData => input.GetInputData<TankInputData>();


        public override void ExecuteRequest(int request)
        {
            switch (request)
            {
                case (int)Requests.Move:
                    break;
            }
        }


        public Vector2 GetCurMoveVector()
        {
            if (inputData.Move != Vector2.zero)
            {
                //计算目标速度 = 输入方向归一化 * 当前身体的移动速度 + 参考系速度
                Vector2 targetVelocity = inputData.Move.normalized * maxSpeed;
            }




            return Vector2.zero;
        }
    }
}