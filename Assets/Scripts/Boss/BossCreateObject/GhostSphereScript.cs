using System;
using System.Collections.Generic;
using UnityEngine;

public class GhostSphereScript : MonoBehaviour
{
    [Header("Float Settings")]
    [SerializeField] private float amplitude = 0.5f;
    [SerializeField] private float frequency = 1f;

    private Vector3 startPos;

    private HexTile currentHexTile;
    private HexTile playerTile;

    private BossAI ownerAI;

    // 🔥 외부 통지용 이벤트
    public event Action<GhostSphereScript> OnSphereBroken;

    // =========================================================
    // ================== 초기화 ================================
    // =========================================================

    public void Initialize(HexTile tile, BossAI ai)
    {
        ownerAI = ai;

        transform.position = new Vector3(
            tile.transform.position.x,
            1.5f,
            tile.transform.position.z
        );

        startPos = transform.position;
        currentHexTile = tile;

        SetAlpha(0.4f);
    }

    private void Update()
    {
        FloatMotion();
    }

    private void FloatMotion()
    {
        float newY = startPos.y +
                     Mathf.Sin(Time.time * frequency * Mathf.PI * 2) * amplitude;

        transform.position = new Vector3(
            startPos.x,
            newY,
            startPos.z
        );
    }

    private void SetAlpha(float alpha)
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer == null) return;

        Color color = renderer.material.color;
        color.a = alpha;
        renderer.material.color = color;
    }

    // =========================================================
    // ================== 플레이어 관련 ==========================
    // =========================================================

    private void UpdatePlayerTile()
    {
        PlayerMove playerMove =
            GameManager.Instance.GetPlayer().GetComponent<PlayerMove>();

        playerTile = playerMove.GetCurrentTile();
    }

    public bool IsPlayerNearby()
    {
        UpdatePlayerTile();

        return HexTileManager.Instance
            .GetTileDistance(currentHexTile, playerTile) <= 1;
    }

    public HexTile GetHexTile() => currentHexTile;

    // =========================================================
    // ================== 파괴 처리 =============================
    // =========================================================

    public void Break()
    {
        // 🔥 이벤트만 던진다
        OnSphereBroken?.Invoke(this);

        Destroy(gameObject);
    }

    // =========================================================
    // ================== 패턴용 타일 계산 =======================
    // =========================================================

    public List<HexTile> GetOddTiles()
    {
        UpdatePlayerTile();

        List<HexTile> tiles = new();

        for (int i = 0; i < 3; i++)
        {
            int dist = 2 * i + 1;

            tiles.AddRange(
                HexTileManager.Instance.tileDirectionHelper
                    .GetDistanceTiles(currentHexTile, playerTile, dist, dist * 6)
            );
        }

        return tiles;
    }

    public List<HexTile> GetEvenTiles()
    {
        UpdatePlayerTile();

        List<HexTile> tiles = new();

        for (int i = 0; i < 3; i++)
        {
            int dist = 2 * i;

            tiles.AddRange(
                HexTileManager.Instance.tileDirectionHelper
                    .GetDistanceTiles(currentHexTile, playerTile, dist, dist * 6)
            );
        }

        return tiles;
    }
}
