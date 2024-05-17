using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor.Tilemaps;

namespace UnityEditor
{
	/// <summary>
	/// 带坐标显示的笔刷
	/// </summary>
	[CustomGridBrush(true, false, false, "Coordinate Brush")]
	[CreateAssetMenu(fileName = "New Coordinate Brush", menuName = "Brushes/Coordinate Brush")]
	public class CoordinateBrush : GridBrush
	{
		[Range(MinZ, MaxZ)] public int z = 0;

		public bool IsShowPos;

		public bool IsNoHeight;

		public const int MinZ = -50;

		public const int MaxZ = 50;

		public override void Paint(GridLayout grid, GameObject brushTarget, Vector3Int position)
		{
			var zPosition = new Vector3Int(position.x, position.y, z);
			base.Paint(grid, brushTarget, zPosition);
		}

		public override void Erase(GridLayout grid, GameObject brushTarget, Vector3Int position)
		{
			if (IsNoHeight)
			{
				// 遍历所有可能的Z坐标范围（假设在-100到100的范围内）
				for (int zPos = MinZ; zPos <= MaxZ; zPos++)
				{
					// 创建当前Z坐标的位置
					var flatPosition = new Vector3Int(position.x, position.y, zPos);
					// 调用基类的Erase方法，擦除这个Z坐标上的内容
					base.Erase(grid, brushTarget, flatPosition);
				}
			}
			else
			{
				var zPosition = new Vector3Int(position.x, position.y, z);
				base.Erase(grid, brushTarget, zPosition);
			}
		}

		public override void FloodFill(GridLayout grid, GameObject brushTarget, Vector3Int position)
		{
			var zPosition = new Vector3Int(position.x, position.y, z);
			base.FloodFill(grid, brushTarget, zPosition);
		}

		public override void BoxFill(GridLayout gridLayout, GameObject brushTarget, BoundsInt position)
		{
			var zPosition = new Vector3Int(position.x, position.y, z);
			position.position = zPosition;
			base.BoxFill(gridLayout, brushTarget, position);
		}
	}

	[CustomEditor(typeof(CoordinateBrush))]
	public class CoordinateBrushEditor : GridBrushEditor
	{
		private CoordinateBrush coordinateBrush
		{
			get { return target as CoordinateBrush; }
		}

		public override void PaintPreview(GridLayout grid, GameObject brushTarget, Vector3Int position)
		{
			var zPosition = new Vector3Int(position.x, position.y, coordinateBrush.z);
			base.PaintPreview(grid, brushTarget, zPosition);
		}

		public override void OnPaintSceneGUI(GridLayout grid, GameObject brushTarget, BoundsInt position,
			GridBrushBase.Tool tool, bool executing)
		{
			base.OnPaintSceneGUI(grid, brushTarget, position, tool, executing);

			var brushTargetPos = brushTarget.transform.position;
			if (coordinateBrush.z != 0)
			{
				var zPosition = new Vector3Int(position.min.x + (int)brushTargetPos.x,
					position.min.y + (int)brushTargetPos.y, coordinateBrush.z);
				BoundsInt newPosition = new BoundsInt(zPosition, position.size);


				//获取四个点
				Vector3[] cellLocals = new Vector3[]
				{
					grid.CellToLocal(new Vector3Int(newPosition.min.x, newPosition.min.y, newPosition.min.z)),
					grid.CellToLocal(new Vector3Int(newPosition.max.x, newPosition.min.y, newPosition.min.z)),
					grid.CellToLocal(new Vector3Int(newPosition.max.x, newPosition.max.y, newPosition.min.z)),
					grid.CellToLocal(new Vector3Int(newPosition.min.x, newPosition.max.y, newPosition.min.z))
				};

				Handles.color = Color.blue;

				int i = 0;
				for (int j = cellLocals.Length - 1; i < cellLocals.Length; j = i++)
				{
					Handles.DrawLine(cellLocals[j], cellLocals[i]);
				}
			}


			var labelText = "Pos: " + new Vector3Int(position.x + (int)brushTargetPos.x,
				position.y + (int)brushTargetPos.y,
				coordinateBrush.z);
			if (position.size.x > 1 || position.size.y > 1)
			{
				labelText += " Size: " + new Vector2Int(position.size.x, position.size.y);
			}

			GUIStyle myStyle = new GUIStyle();
			myStyle.normal.textColor = Color.white;


			Handles.Label(
				grid.CellToWorld(new Vector3Int(position.x + (int)brushTargetPos.x, position.y + (int)brushTargetPos.y,
					coordinateBrush.z)), labelText,
				myStyle);
		}
	}
}
#endif