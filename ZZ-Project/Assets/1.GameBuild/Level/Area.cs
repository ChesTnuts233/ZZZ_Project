using System.Collections.Generic;
using KooFrame;
using KooFrame.Module;
using Sirenix.OdinInspector;
using SubSystem.Map;
using UnityEngine;

public class Area : MonoBehaviour
{
    [SerializeField, LabelText("此区域的所有房间")] private List<Room> rooms = new List<Room>();

    private GridMapXZ<GridData> map;

    [SerializeField, LabelText("区域左下角")] private Vector3Int mapOrigin;
    [SerializeField, LabelText("区域右上角")] private Vector3Int mapTopPos;

    /// <summary>
    /// 区域宽度
    /// </summary>
    private int areaWidth;

    /// <summary>
    /// 区域宽度
    /// </summary>
    public int Width
    {
        get => areaWidth;
        set => areaWidth = value;
    }

    /// <summary>
    /// 区域高度
    /// </summary>
    private int areaHeight;

    /// <summary>
    /// 区域高度
    /// </summary>
    public int Height
    {
        get => areaHeight;
        set => areaHeight = value;
    }



    private void Start()
    {
        InitRoom();
        InitMap();
    }

    #region 初始化

    [ContextMenu("所有房间初始化")]
    private void InitRoom()
    {
        GetAllRoom();
        //找到左下角的房间和右下角的房间 
        if (rooms == null || rooms.Count == 0) return;

        int mapOriginX = int.MaxValue;
        int mapOriginY = int.MaxValue;
        int mapTopPosX = int.MinValue;
        int mapTopPosY = int.MinValue;


        foreach (Room room in rooms)
        {
            if (room.TopPos.x > mapTopPosX)
            {
                mapTopPosX = room.TopPos.x;
            }

            if (room.TopPos.y > mapTopPosY)
            {
                mapTopPosY = room.TopPos.y;
            }

            if (room.OriginPos.x < mapOriginX)
            {
                mapOriginX = room.OriginPos.x;
            }

            if (room.OriginPos.y < mapOriginY)
            {
                mapOriginY = room.OriginPos.y;
            }
        }

        //左下角的房间的Origin为区域原点
        mapOrigin = new Vector3Int(mapOriginX, mapOriginY);
        //右上角为最右上角房间的顶点
        mapTopPos = new Vector3Int(mapTopPosX, mapTopPosY);

        areaWidth = mapTopPos.x - mapOrigin.x;
        areaHeight = mapTopPos.y - mapOrigin.y;
    }

    private void InitMap()
    {
        map = new GridMapXZ<GridData>(areaWidth + 1, areaHeight + 1, 1, mapOrigin.XYToXZ(), createGridData);
    }

    private GridData createGridData(GridMapXZ<GridData> map, int x, int z)
    {
        return new GridData(map, x, z);
    }


    [ContextMenu("找到所有房间")]
    private void GetAllRoom()
    {
        rooms.Clear();
        //得到所有房间
        foreach (Transform go in this.transform)
        {
            if (go.TryGetComponent(out Room room))
            {
                room.Init();
                rooms.Add(room);
            }
        }
    }


    /// <summary>
    /// 地图初始化
    /// </summary>
    private void MapInit()
    {
        //map = new GridMapXZ<GridData>()
    }

    #endregion
}