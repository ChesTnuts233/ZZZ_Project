//****************** 代码文件申明 ************************
//* 文件：CharacterMove                                       
//* 作者：Koo
//* 创建时间：2024/04/23 11:39:29 星期二
//* 功能：角色移动
//*****************************************************

using KooFrame;
using UnityEngine;

namespace GameBuild
{
	public class CharacterMove : MovementBase
	{

		private KanojoInputData inputData => input.GetInputData<KanojoInputData>();

		private KanojoModel model;

		private Camera mainCamera;

		public CharacterMove(MonoBehaviour mono, Rigidbody rb, Transform inputSpace, Transform player, Animator anim, IPlayerBase self) : base(mono, rb, inputSpace, player, anim, self)
		{

		}

		protected override void OnInit()
		{
			base.OnInit();
			model = this.GetModel<KanojoModel>();
			mainCamera = Camera.main;
		}



		public override void ExecuteRequest(int request)
		{
			switch (request)
			{
				case (int)Requests.Move:
					break;
			}
		}

		protected override void OnUpdateHandle()
		{
			base.OnUpdateHandle();
			UpdateAnim();
			UpdateRotate();
		}




		protected override void OnFixedUpdateHandle()
		{
			base.OnFixedUpdateHandle();
			Move();
			ApplyFriction();
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

				//计算每帧要加的力
				Vector3 force = rb.mass * model.Acceleration * targetVelocity;

				return force;
			}

			return Vector2.zero;
		}


		// 应用摩擦力
		public void ApplyFriction()
		{
			if (rb.velocity == Vector3.zero)
			{
				return;
			}
			//根据当前速度计算摩擦力
			Vector3 velocity = rb.velocity;



			//计算摩擦力 = -速度 * 摩擦系数
			Vector3 frictionForce = -velocity * model.FrictionCoefficient;
			//应用摩擦力
			rb.AddForce(frictionForce);
		}


		private void UpdateRotate()
		{
			Vector3 s2wPoint = mainCamera.ScreenToWorldPoint(inputData.MousePos);


			Vector2 mouseToWorld = new Vector2(s2wPoint.x, s2wPoint.z);

			

			//方向
			Vector2 dir = (mouseToWorld - new Vector2(playerTransform.position.x, playerTransform.position.z));


			//计算与相机的向量
			Vector2 curForwardDir = dir;



			// 根据当前方向计算旋转角度 
			float rotationAngle = Mathf.Atan2(curForwardDir.x, curForwardDir.y) * Mathf.Rad2Deg;
			// 根据旋转角度来旋转对象
			playerTransform.rotation =
				Quaternion.Euler(0f, rotationAngle, 0f);
		}

		private void UpdateAnim()
		{
			var speed = Mathf.Lerp(0, 1, rb.velocity.magnitude / model.MaxSpeed);


			anim.SetFloat(AnimHash.MoveSpeedHash, speed);
		}
	}
}