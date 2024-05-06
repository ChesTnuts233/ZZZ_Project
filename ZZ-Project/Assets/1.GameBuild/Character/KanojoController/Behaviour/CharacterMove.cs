//****************** 代码文件申明 ************************
//* 文件：TankMove                                       
//* 作者：Koo
//* 创建时间：2024/04/23 11:39:29 星期二
//* 功能：坦克移动
//*****************************************************

using KooFrame;
using UnityEngine;

namespace GameBuild
{
	public class CharacterMove : MovementBase
	{


		private KanojoInputData inputData => input.GetInputData<KanojoInputData>();

		private KanojoModel model;

		public CharacterMove(MonoBehaviour mono, Rigidbody rb, Transform inputSpace, Transform player, Animator anim, IPlayerBase self) : base(mono, rb, inputSpace, player, anim, self)
		{

		}

		protected override void OnInit()
		{
			base.OnInit();
			model = this.GetModel<KanojoModel>();
		}



		public override void ExecuteRequest(int request)
		{
			switch (request)
			{
				case (int)Requests.Move:
					break;
			}
		}

		protected override void OnFixedUpdateHandle()
		{
			base.OnFixedUpdateHandle();
			Move();
		}

		public void Move()
		{
			rb.AddForce(GetMoveForce());
		}


		public Vector3 GetMoveForce()
		{
			if (inputData.Move != Vector2.zero)
			{
				Vector3 CurMoveSpeed = rb.velocity;

				//计算目标速度 = 输入方向归一化 * 当前身体的移动速度 + 参考系速度
				Vector3 targetVelocity = new Vector3(inputData.Move.x, 0, inputData.Move.y).normalized * model.MaxSpeed;


				//计算每帧应该加的速度 为 (目标速度 - 当前速度) △v / (变化时间) △t
				Vector3 accelerateVector = (targetVelocity - CurMoveSpeed) /
											 (Time.fixedDeltaTime);

				//计算每帧要加的力
				Vector3 force = rb.mass * accelerateVector;

				return force;
			}







			return Vector2.zero;
		}
	}
}