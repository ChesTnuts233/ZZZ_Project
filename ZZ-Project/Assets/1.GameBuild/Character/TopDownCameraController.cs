//****************** 代码文件申明 ************************
//* 文件：TopDownCameraController                                       
//* 作者：Koo
//* 创建时间：2024/05/17 00:20:42 星期五
//* 描述：TopDown视角的相机位点控制方案
//*****************************************************
using Cinemachine;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameBuild
{
	public class TopDownCameraController : MonoBehaviour
	{
		/// <summary>
		/// 鼠标世界坐标与角色之间的距离
		/// </summary>
		[ShowInInspector] private float mouseDistance;

		[SerializeField, LabelText("X轴的权重"), Range(0, 1)] private float xWeight = 1;

		[SerializeField, LabelText("Z轴的权重"), Range(0, 1)] private float zWeight = 1;

		[SerializeField, LabelText("相机距离处理曲线")] private AnimationCurve cameraCurve;

		[SerializeField] private float mouseDistanceX;

		[SerializeField] private float mouseDistanceZ;

		[SerializeField] private float cameraDisX;

		[SerializeField] private float cameraDisZ;

		[SerializeField, LabelText("位点高度")] private float height = 2f;

		

		[SerializeField,LabelText("偏移")]
		private Vector2 offSet;

		private Camera mainCamera;

		private KanojoInputData inputData;



		/// <summary>
		/// 角色
		/// </summary>
		[SerializeField, LabelText("角色")]
		private Kanojo Character;

		public Vector2 CharacterWorldPos => new Vector2(Character.GO.transform.position.x, Character.GO.transform.position.z);

		private void Awake()
		{
			mainCamera = Camera.main;

		}

		private void Start()
		{
			inputData = Character.Input.GetInputData<KanojoInputData>();
		}


		private void LateUpdate()
		{
			HandleCameraWhenMouseController();
		}

		/// <summary>
		/// 当鼠标控制方向的时候处理相机位置
		/// </summary>
		private void HandleCameraWhenMouseController()
		{
			Vector3 s2wPoint = mainCamera.ScreenToWorldPoint(inputData.MousePos);

			Vector2 mouseToWorld = new Vector2(s2wPoint.x, s2wPoint.z);

			//方向
			Vector2 dir = (mouseToWorld - CharacterWorldPos);

			Vector2 rectDir = new Vector2(Mathf.Clamp(dir.x, -1f, 1f), Mathf.Clamp(dir.y, -1f, 1f));


			//计算鼠标指针坐标与角色的距离
			mouseDistance =
			Vector2.Distance(
					mouseToWorld, CharacterWorldPos);


			//X轴的距离
			mouseDistanceX = Mathf.Abs(CharacterWorldPos.x - mouseToWorld.x);

			//Y轴的距离
			mouseDistanceZ = Mathf.Abs(CharacterWorldPos.y - mouseToWorld.y);

			//钳制鼠标距离 最大值为曲线的最后接点时间
			mouseDistanceX = Mathf.Clamp(mouseDistanceX, 0f, cameraCurve.keys[^1].time / xWeight);

			//钳制鼠标距离 
			mouseDistanceZ = Mathf.Clamp(mouseDistanceZ, 0f, cameraCurve.keys[^1].time / zWeight);

			//曲线映射
			cameraDisX = cameraCurve.Evaluate(mouseDistanceX * xWeight);

			cameraDisZ = cameraCurve.Evaluate(mouseDistanceZ * zWeight);

			Vector2 targetPosInXY = rectDir * new Vector2(cameraDisX, cameraDisZ) + offSet;
			//控制相机位置点的坐标
			this.transform.localPosition = new Vector3(targetPosInXY.x, height, targetPosInXY.y);
		}







	}
}

