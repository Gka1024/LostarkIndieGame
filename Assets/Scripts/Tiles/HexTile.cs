using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class HexTile : MonoBehaviour
{
    public GameManager manager;
    public HexTileManager tileManager;
    public GameObject player;
    public HexTile[] neighbors;
    public MeshRenderer meshRenderer;
    public ObjectManager objectManager;

    public Vector3Int CubeCoord;

    public TileState currentTileState;
    public TileSpecific currentTileSpecific;

    public bool isBossAttackRange = false;

    private Color originalColor;
    public Color playerMoveRangeColor;
    public Color bossAttackRangeColor;

    // ===== 보스 외곽 잡기용
    public OuterGrabMonster monster;
    public bool isMonsterOn;

    private void Awake()
    {
        manager = FindFirstObjectByType<GameManager>();
        tileManager = manager.hexTileManager;
        objectManager = manager.objectManager;
        player = manager.GetPlayer();
        RegisterToObjectManager();
        isMonsterOn = false;

        Init();

    }

    private void Init()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        originalColor = meshRenderer.material.color;
        playerMoveRangeColor = new Color(0.564f, 0.933f, 0.565f);
        bossAttackRangeColor = new Color(0.8f, 0.2f, 0.2f);
        CubeCoord = WorldToCube(transform.position);
    }

    private void RegisterToObjectManager()
    {
        objectManager.Register(this);
    }

    public static Vector3Int WorldToCube(Vector3 worldPos)
    {
        float q = worldPos.x / 2 - worldPos.z / 2 / Mathf.Sqrt(3);
        float r = worldPos.z / Mathf.Sqrt(3);

        return AxialToCube(RoundAxial(q, r));
    }

    public static Vector2 RoundAxial(float q, float r)
    {
        float x = q;
        float z = r;
        float y = -x - z;

        int rx = Mathf.RoundToInt(x - 1e-4f);
        int ry = Mathf.RoundToInt(y - 1e-4f);
        int rz = Mathf.RoundToInt(z - 1e-4f);

        float dx = Mathf.Abs(rx - x);
        float dy = Mathf.Abs(ry - y);
        float dz = Mathf.Abs(rz - z);

        if (dx > dy && dx > dz)
            rx = -ry - rz;
        else if (dy > dz)
            ry = -rx - rz;
        else
            rz = -rx - ry;

        return new Vector2(rx, rz); // q = x, r = z
    }

    public static Vector3Int AxialToCube(Vector2 axial)
    {
        int x = (int)axial.x;
        int z = (int)axial.y;
        int y = -x - z;

        return new Vector3Int(x, y, z);
    }

    public void ChangeColorIsMoveable()
    {
        if (manager.IsPlayerClicked())
        {
            if (GetIsPlayerMoveable())
            {
                PaintColor(playerMoveRangeColor);
            }
        }
        else
        {
            ResetColor();
        }
    }

    public void FindNeighbors(HexTile[] allTiles)
    {
        List<HexTile> foundNeighbors = new List<HexTile>();
        foreach (HexTile tile in allTiles)
        {
            if (tile != this && IsNeighbor(tile))
            {
                foundNeighbors.Add(tile);
            }
        }
        neighbors = foundNeighbors.ToArray();

        if (neighbors.Length == 0)
        {
            PaintColor(Color.red);
        }

    }

    public void OnMouseEnter()
    {
        tileManager.RegisterTile(this.gameObject);
        if (manager.IsPlayerClicked())
        {
            ShowPlayerMoveableRange(true);
        }
        //Debug.Log(this.CubeCoord);
    }

    public void OnMouseExit()
    {
        if (manager.IsPlayerClicked())
        {
            ShowPlayerMoveableRange(false);
            return;
        }
    }

    private void ShowPlayerMoveableRange(bool isEnter)
    {
        if (isEnter)
        {
            // 마우스가 들어왔을 때는 이동 가능 여부에 따라 직관적으로 표시
            Color hoverColor = GetIsPlayerMoveable() ? Color.green : Color.red;
            PaintColor(hoverColor);
        }
        else
        {
            // 마우스가 나갔을 때: 타일의 '원래' 상태에 따른 색상 복구
            ApplyDefaultTileColor();
        }
    }

    /// <summary>
    /// 타일의 현재 상태(보스 패턴, 보스 위치, 이동 범위 등)에 따라 올바른 색상을 입힙니다.
    /// </summary>
    private void ApplyDefaultTileColor()
    {
        // 1순위: 플레이어가 이동 가능한 범위인가?
        if (GetIsPlayerMoveable())
        {
            PaintColor(playerMoveRangeColor);
            return;
        }

        // 2순위: 보스 공격 패턴 범위 내에 있는가? (보스 타일색보다 우선)
        // bossAI나 bossController에서 현재 공격 예고 타일인지 확인하는 로직이 필요합니다.
        if (tileManager.IsAttackPreviewTile(this))
        {
            PaintColor(bossAttackRangeColor); // 또는 패턴용 오렌지/레드 계열
            return;
        }

        // 3순위: 보스가 점유 중인 타일인가?
        if (tileManager.IsBossTile(this))
        {
            PaintColor(tileManager.GetBossTileColor());
            return;
        }


        // 4순위: 아무것도 해당되지 않으면 기본 상태로 복구
        ResetColor();
    }

    public void RegisterBossObject(OuterGrabMonster monster)
    {
        isMonsterOn = true;
        this.monster = monster;
    }

    public void RemoveBossObject()
    {
        isMonsterOn = false;
        this.monster = null;
    }

    public Vector3 GetThisSpawnPos(float Ypos = 1.5f)
    {
        return new Vector3(transform.position.x, Ypos, transform.position.z);
    }

    public void PaintColor(Color color)
    {
        meshRenderer.material.color = color;
    }

    public void ResetColor()
    {
        if (isBossAttackRange)
        {
            PaintColor(bossAttackRangeColor);
        }

        else if (tileManager.IsBossTile(this))
        {
            PaintColor(tileManager.GetBossTileColor());
            return;
        }

        else
        {
            PaintColor(originalColor);
        }
    }

    private bool IsNeighbor(HexTile tile)
    {
        float distance = Vector3.Distance(transform.position, tile.transform.position);
        return distance > 1f && distance < 3f;
    }

    public bool GetIsTileMoveable()
    {
        return currentTileState == TileState.Default;
    }

    public bool GetIsPlayerMoveable()
    {
        return tileManager.IsTileMoveable(player.GetComponent<PlayerMove>().GetCurrentTile(), this, player.GetComponent<PlayerMove>().moveAbleDistance);
    }

    public void SetTileState(TileState state, TileSpecific specific = TileSpecific.Default)
    {
        currentTileState = state;
        currentTileSpecific = specific;
    }

    public void DestroyTile()
    {
        SetTileState(TileState.Destroyed);

        MeshRenderer renderer = GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            renderer.enabled = false;
        }

        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = false;
        }

    }
}

public enum TileState
{
    Default,
    IsBossTile,
    ItemPlaced,
    Destroyed,
    IsWall,
    IsPillar,
    IsObstacle
}

public enum TileSpecific
{
    Default,
    PillarLeftUp,
    PillarLeftMiddle,
    PillarLeftDown,
    PillarRightUp,
    PillarRightMiddle,
    PillarRightDown,
    WallFront,
    WallLeftUp,
    WallLeftDown,
    WallRightUp,
    WallRightDown,
    ObstacleLeftUp,
    ObstacleLeftDown,
    ObstacleRightUp,
    ObstacleRightDown,
    PillarForPattern,
}