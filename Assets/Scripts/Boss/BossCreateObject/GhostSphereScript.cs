using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GhostSphereScript : MonoBehaviour
{
    public float amplitude = 0.5f;
    public float frequency = 1f;

    private Vector3 startPos;

    private HexTile currentHexTile;
    private HexTile playerTile;


    void Start()
    {
        startPos = transform.position;
        currentHexTile = HexTileManager.Instance.IsThereHexTile(startPos);
        SetAlpha(0.4f);
    }

    void SetAlpha(float alpha)
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            Color color = renderer.material.color;
            color.a = alpha;
            renderer.material.color = color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * frequency * Mathf.PI * 2) * amplitude;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }

    private void SetPlayerTile()
    {
        PlayerMove playerMove = GameManager.Instance.GetPlayer().GetComponent<PlayerMove>();
        playerTile = playerMove.GetCurrentTile();
    }

    public bool CheckIsPlayerNearby()
    {
        SetPlayerTile();

        if (HexTileManager.Instance.GetTileDistance(currentHexTile, playerTile) <= 1) return true;
        return false;
    }

    public void SetPosition(HexTile tile)
    {
        this.transform.position = new Vector3(tile.transform.position.x, 1.5f, tile.transform.position.z);
    }

    public HexTile GetHexTile() => currentHexTile;

    public void OnBreak()
    {
        GameManager.Instance.GetBoss().GetComponent<BossAI>().bossPatternHelper.OnSummonedObjectDestroyed(this.gameObject);
    }

    public List<HexTile> GetOddTiles()
    {
        SetPlayerTile();

        List<HexTile> returnTiles = new();

        for (int i = 0; i < 3; i++)
        {
            returnTiles.AddRange(HexTileManager.Instance.tileDirectionHelper.GetDistanceTiles(currentHexTile, playerTile, 2 * i + 1, (2 * i + 1) * 6));
        }

        return returnTiles;
    }

    public List<HexTile> GetEvenTiles()
    {
        SetPlayerTile();
        List<HexTile> returnTiles = new();

        for (int i = 0; i < 3; i++)
        {
            returnTiles.AddRange(HexTileManager.Instance.tileDirectionHelper.GetDistanceTiles(currentHexTile, playerTile, 2 * i, (2 * i) * 6));
        }

        return returnTiles;

    }
}
