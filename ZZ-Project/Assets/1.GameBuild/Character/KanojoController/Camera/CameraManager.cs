//****************** 代码文件申明 ************************
//* 文件：CameraManager                                       
//* 作者：Koo
//* 创建时间：2024/05/06 17:42:38 星期一
//* 描述：相机控制
//*****************************************************
using Cinemachine;
using Sirenix.OdinInspector;
using UnityEngine;


namespace GameBuild
{
	public class CameraManager : MonoBehaviour
	{
		[SerializeField]
		public Transform FollowTarget;

		[SerializeField]
		private CinemachineVirtualCamera ThirdCM;

		public Camera mainCamera;

		/// <summary>
		/// 垂直轴
		/// </summary>
		private float cinemachineTargetYaw;

		/// <summary>
		/// 横向轴
		/// </summary>
		private float cinemachineTargetPitch;


		[LabelText("相机垂直角度底部限制")]
		public float BottomClamp = -30.0f;

		[Tooltip("相机垂直角度顶部限制")]
		public float TopClamp = 70.0f;

		[Tooltip("额外角度。在锁定相机位置时，用于微调相机位置时非常有用")]
		public float CameraAngleOverride = 0.0f;


		[SerializeField, LabelText("距离目标距离")]
		public float DistanceFromTarget;


		[SerializeField, LabelText("是否跟随目标")]
		public bool IsFollowTarget;


		[ShowInInspector, LabelText("相机目标位置")]
		private Vector3 targetPos;

		[SerializeField]
		private Kanojo kanojo;


		private KanojoInputData inputData => kanojo.Input.GetInputData<KanojoInputData>();


		#region 生命周期

		private void Awake()
		{
			mainCamera = this.GetComponent<Camera>();
		}

		private void Start()
		{
			ThirdCM.Follow = FollowTarget;
			cinemachineTargetYaw = FollowTarget.rotation.eulerAngles.y;
		}



		private void LateUpdate()
		{
			CameraHandler();

		}


		#endregion



		private void CameraHandler()
		{

			//targetPos = FollowTarget.position;

			//float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

			cinemachineTargetYaw += inputData.Look.x;
			cinemachineTargetPitch += inputData.Look.y;


			cinemachineTargetYaw = ClampAngle(cinemachineTargetYaw, float.MinValue, float.MaxValue);
			cinemachineTargetPitch = ClampAngle(cinemachineTargetPitch, BottomClamp, TopClamp);

			FollowTarget.rotation = Quaternion.Euler(cinemachineTargetPitch + CameraAngleOverride,
				cinemachineTargetYaw, 0.0f);
		}

		private float ClampAngle(float lfAngle, float lfMin, float lfMax)
		{
			if (lfAngle < -360f) lfAngle += 360f;
			if (lfAngle > 360f) lfAngle -= 360f;
			return Mathf.Clamp(lfAngle, lfMin, lfMax);
		}
	}
}

