using GameBuild;
using KooFrame;
using UnityEditor;


[CustomEditor(typeof(CameraManager))]
public class CameraManagerEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		CameraManager CMtarget = (CameraManager)target;



		//if (CMtarget.FollowTarget != null)
		//{
		//	CMtarget.transform.rotation = CMtarget.FollowTarget.transform.rotation;
		//}


	}
}
