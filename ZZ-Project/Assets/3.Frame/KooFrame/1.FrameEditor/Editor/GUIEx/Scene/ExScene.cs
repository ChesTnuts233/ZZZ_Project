// using GameBuild;
// using UnityEditor;
// using UnityEngine;
//
// namespace KooFrame
// {
// 	public class ExScene
// 	{
// #if UNITY_EDITOR //此为宏定义标签 UNITY_EDITOR表示这段代码只在Editor模式下执行 发布后剔除
// #pragma warning disable
// 		public static bool CanChooseInScene = true;
//
// 		[InitializeOnLoadMethod]
// 		static void InitializeOnLoadMethod()
// 		{
// 			//只在相机视角为2D的时候添加此委托
//
// 			SceneView.onSceneGUIDelegate += delegate (SceneView sceneView)
// 			{
// 				if (sceneView.camera.orthographic == false)
// 					return;
//
// 				Event e = Event.current;
// 				if (e != null && e.button == 1 && e.type == EventType.MouseUp)
// 				{
// 					//获取鼠标位置
// 					Vector2 mousePosition = e.mousePosition;
// 					//设置菜单项
// 					var options = new GUIContent[]
// 					{
// 						new GUIContent("Bsk移动到此位置")
// 					};
//
// 					//设置菜单显示区域
// 					var selected = -1;
// 					var userData = Selection.activeGameObject;
// 					var width = 100;
// 					var height = 100;
// 					var position = new Rect(mousePosition.x, mousePosition.y - height, width, height);
// 					//显示菜单
// 					EditorUtility.DisplayCustomMenu(position, options, selected,
// 						delegate (object data, string[] options, int selected)
// 						{
// 							//获取相机
// 							Camera sceneCamera = sceneView.camera;
// 							//获取鼠标在scene窗口上的位置左上角(0,0)右下角(0.66f,0.66f),不知道为什么
// 							Vector2 guiPosition = sceneCamera.ScreenToViewportPoint(mousePosition);
//
// 							//获取当前分辨率屏幕视窗大小
// 							float m = 1f;
//
// 							if (Screen.currentResolution.width == 1920)
// 							{
// 								m = 1f;
// 							}
// 							else if (Screen.currentResolution.width == 2560)
// 							{
// 								m = 0.66f;
// 							}
//
// 							//屏幕长宽比
// 							float kwh = (float)sceneCamera.activeTexture.width / sceneCamera.activeTexture.height;
//
// 							//计算偏移量，即当前屏幕，左上角到右下角的世界坐标距离
// 							Vector2 offset = new Vector2(sceneCamera.orthographicSize * kwh,
// 								-sceneCamera.orthographicSize) * 2f;
//
//
// 							//算出偏移率
// 							float xPercent = guiPosition.x / m;
// 							float yPercent = guiPosition.y / m;
//
// 							//传送位置为，相机位置 + 偏移量 * 偏移率(x,y), 通过鼠标在scene窗口上的偏移率，乘上世界坐标的偏移量，得到世界坐标的偏移坐标。
// 							Vector2 pos = (Vector2)sceneCamera.transform.position +
// 										  new Vector2(offset.x * (-0.5f + xPercent), offset.y * (-0.5f + yPercent));
//
// 							//查找玩家
// 							var bsk = GameObject.FindObjectOfType<Berserker>();
// 							//如果玩家不为空
// 							if (bsk != null)
// 							{
// 								//那就把玩家传送过去
// 								bsk.transform.position = new Vector3(pos.x, pos.y, 0);
// 							}
//
// 							Debug.Log("移动成功" + pos);
// 						}, userData);
// 					e.Use();
// 				}
//
// 				if (e != null && CanChooseInScene == false)
// 				{
// 					//FocusType.Passive表示禁止接受控制焦点 获取它的controllID后即可禁止点击事件穿透下去
// 					int controlID = GUIUtility.GetControlID(FocusType.Passive);
// 					if (e.type == EventType.Layout)
// 					{
// 						HandleUtility.AddDefaultControl(controlID);
// 					}
//
// 					Handles.BeginGUI();
// 					float sceneWindowHeight = SceneView.currentDrawingSceneView.position.height;
// 					GUI.color = Color.red;
// 					GUI.Label(new Rect(10f, sceneWindowHeight - 75f, 200f, 15f), "现在不能在Scene中选择物品哦");
// 					GUI.color = Color.white;
// 					if (GUI.Button(new Rect(10f, sceneWindowHeight - 50f, 140f, 20f), "关闭禁止选中物品功能"))
// 					{
// 						CanChooseInScene = true;
// 						Debug.Log("KooHelp:" + "现在可以在场景中选择物品了");
// 					}
//
// 					Handles.EndGUI();
// 				}
// 			};
// 		}
// 	}
//
// }
//
// //        public static Vector3 GetMousePosToScene(Vector2 mousePos, SceneView sceneView)
// //        {
// //            //当前屏幕坐标,左上角(0,0)右下角(camera.pixelWidth,camera.pixelHeight)
// //            //retina 屏幕需要拉伸值
// //            float mult = 1;
// //#if UNITY_5_4_OR_NEWER
// //            mult = EditorGUIUtility.pixelsPerPoint;
// //#endif
// //            Debug.Log(mult);
// //            //转换成摄像机可接受的屏幕坐标,左下角是(0,0,0);右上角是(camera.pixelWidth,camera.pixelHeight,0)
// //            mousePos.y = sceneView.camera.pixelHeight - mousePos.y * mult;
// //            mousePos.x *= mult;
// //            //近平面往里一些,才能看到摄像机里的位置
// //            Vector3 fakePoint = mousePos;
// //            fakePoint.z = 20;
// //            Vector3 point = sceneView.camera.ScreenToWorldPoint(fakePoint);
// //            return point;
// //        }
// #endif
//
// //在Scene视图中容易选择到子节点 绑定一个[SelectionBase] 即可定位所有子节点到此对象上
// //[SelectionBase]
// //public class RootScripts : MonoBehaviour
// //{
//
// //}
