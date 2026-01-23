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

    public GameObject OnTileObject;

    public Vector3Int CubeCoord;

    public TileState currentTileState;

    public bool isMoveable = true;
    public bool isBossAttackRange = false;

    private Color originalColor;
    public Color playerMoveRangeColor;
    public Color bossAttackRangeColor;

    private void Awake()
    {
        manager = FindFirstObjectByType<GameManager>();
        tileManager = FindFirstObjectByType<HexTileManager>();
        player = manager.GetPlayer();
        meshRenderer = GetComponent<MeshRenderer>();
        originalColor = meshRenderer.material.color;
        playerMoveRangeColor = new Color(0.564f, 0.933f, 0.565f);
        bossAttackRangeColor = new Color(0.8f, 0.2f, 0.2f);
        CubeCoord = WorldToCube(transform.position);
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
        if (GetIsPlayerMoveable() && isEnter) // 플레이어가 이동 가능한 범위에 마우스가 들어왔을때
        {
            PaintColor(Color.green);
        }
        else if (!GetIsPlayerMoveable() && isEnter) // 플레이어가 이동 불가능한 범위에 마우스가 들어왔을때
        {
            PaintColor(Color.red);
        }
        else if (GetIsPlayerMoveable() && !isEnter) // 플레이어가 이동 가능한 범위에서 마우스가 나갔을때
        {
            if (tileManager.IsBossTile(this))
            {
                PaintColor(tileManager.GetBossTileColor());
            }
            else
            {
                PaintColor(playerMoveRangeColor);
            }
        }
        else if (!GetIsPlayerMoveable() && !isEnter) // 플레이어가 이동 불가능한 범위에서 마우스가 나갔을때
        {
            if (tileManager.IsBossTile(this))
            {
                PaintColor(tileManager.GetBossTileColor());
            }
            else
            {
                ResetColor();
            }
        }
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
        return isMoveable && currentTileState == TileState.Default;
    }

    public bool GetIsPlayerMoveable()
    {
        return tileManager.IsTileMoveable(player.GetComponent<PlayerMove>().GetCurrentTile(), this, player.GetComponent<PlayerMove>().moveAbleDistance);
    }

    public void BreakWalls(bool breakObject = false)
    {
        if (currentTileState == TileState.IsWall)
        {
            currentTileState = TileState.Default;
        }

        if(breakObject)
        {
            Destroy(OnTileObject);
        }

    }

}

public enum TileState
{
    Default,
    IsBossTile,
    ItemPlaced,
    Destroyed,
    IsWall
}