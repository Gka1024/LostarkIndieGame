using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossInteraction : MonoBehaviour
{
    public GameManager manager;
    public BossAnimation bossAnimation;

    [SerializeField] private float moveDuration;

    public HexTile currentTile; // 보스가 위치한 타일
    public Color BossTileColor;
    public List<HexTile> neighborTile = new List<HexTile>(); // 보스 주변 타일

    void Start()
    {
        ChangeBossColor();
        ChangeWeaponColor();
        SetStartPosition();
    }

    private void SetStartPosition()
    {
        currentTile = manager.hexTileManager.GetObjectHextile(gameObject);
        neighborTile = new List<HexTile>(currentTile.neighbors);
        ColorBossTile();
    }

    public void Moveto(HexTile targetTile) // 코루틴으로 보스 이동 구현하기
    {
        StartCoroutine(MoveCoroutine(targetTile));

        currentTile = targetTile;
        neighborTile = new List<HexTile>(currentTile.neighbors);
        HexTileManager.Instance.ResetTileColor();

        ColorBossTile();
    }

    public IEnumerator MoveCoroutine(HexTile targetTile)
    {
        bossAnimation.isMoving = true;
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = targetTile.GetThisSpawnPos(0);
        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, elapsed / moveDuration); // 부드러운 가속/감속

            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        transform.position = targetPosition; // 정확한 위치 보정
        bossAnimation.isMoving = false;
    }


    public void ColorBossTile()
    {
        currentTile.PaintColor(BossTileColor);
        foreach (HexTile tile in neighborTile)
        {
            tile.PaintColor(BossTileColor);
        }
    }

    public Color newColor = Color.grey; // 원하는 색상 설정
    private Renderer bossRenderer;

    private void ChangeBossColor()
    {
        bossRenderer = GetComponentInChildren<Renderer>(); // 보스의 Renderer 가져오기
        if (bossRenderer != null)
        {
            bossRenderer.material.color = newColor; // 색상 변경
        }
    }

    public GameObject bossWeapon;
    private Renderer weaponRenderer;

    private void ChangeWeaponColor()
    {
        weaponRenderer = bossWeapon.GetComponent<Renderer>();
        if (weaponRenderer != null)
        {
            weaponRenderer.material.color = newColor; // 색상 변경
        }
    }

    public HexTile GetCurrentTile() => currentTile;
}
