using KooFrame;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class Room : MonoBehaviour
{
    [LabelText("房间名称")] public string RoomName;


    [SerializeField, LabelText("房间持有的Tilemap")] private Tilemap tilemap;

    public Tilemap TileMap
    {
        get
        {
            if (tilemap == null)
            {
                tilemap = this.GetComponent<Tilemap>();
            }

            return tilemap;
        }
    }

    /// <summary>
    /// 瓦片地图相关是否完成初始化
    /// </summary>
    [ShowInInspector, ReadOnly] private bool isTileInited = false;

    [ShowInInspector]
    public int Width
    {
        get
        {
            if (!isTileInited) TileInit();

            return tilemap.size.x;
        }
    }

    [ShowInInspector]
    public int Height
    {
        get
        {
            if (!isTileInited) TileInit();

            return tilemap.size.y;
        }
    }

    [ShowInInspector]
    public Vector3Int OriginPos
    {
        get
        {
            if (!isTileInited) TileInit();

            return tilemap.origin + transform.position.ToVector3Int();
        }
    }

    [ShowInInspector] public Vector3Int TopPos => OriginPos + new Vector3Int(Width - 1, Height - 1);

    public void Awake()
    {
    }

    [ContextMenu("初始化房间")]
    public void Init()
    {
        TileInit();
    }

    [Button]
    private void TileInit()
    {
        tilemap.CompressBounds();

        isTileInited = true;
    }
}