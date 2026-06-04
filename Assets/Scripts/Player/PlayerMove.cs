using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public GameManager manager;
    public HexTileManager hexTileManager;

    public PlayerAnimation playerAnimation;

    [SerializeField] private HexTile currentTile;
    public int moveAbleDistance;
    public float moveDuration;
    public float rotationSpeed;

    private Queue<HexTile> path = new Queue<HexTile>();


    void Awake()
    {
        playerAnimation = gameObject.GetComponent<PlayerAnimation>();
    }

    void Start()
    {
        currentTile = hexTileManager.GetObjectHextile(gameObject);
    }

    public HexTile GetCurrentTile() => currentTile;

    public void Revive()
    {
        if (currentTile.currentTileState == TileState.Destroyed)
        {
            Debug.Log("revive");
            HexTile centerTile = HexTileManager.Instance.GetTileByCube(Vector3Int.zero);

            HexTile targetTile = null;

            int distanceToCenter = HexTileManager.Instance.GetTileDistance(currentTile, centerTile);

            for (int i = 1; i <= distanceToCenter; i++)
            {
                HexTile tileTemp = TileDirectionHelper.Instance.GetFrontTile(currentTile, centerTile, i);

                if (tileTemp != null && tileTemp.currentTileState == TileState.Default)
                {
                    targetTile = tileTemp;
                    break;
                }
            }

            if (targetTile == null)
            {
                targetTile = centerTile;
            }

            Debug.Log(targetTile.CubeCoord);

            MoveToTile(new PlayerMoveInfo(targetTile, ignoreDistance: true));
        }
    }

    public void MoveToTile(PlayerMoveInfo info)
    {
        HexTile targetTile = info.tile;

        if (targetTile == null || !targetTile.GetIsTileMoveable()) return;

        if (!info.ignoreDistance)
        {
            if (!hexTileManager.IsTileMoveable(currentTile, targetTile, moveAbleDistance))
            {
                targetTile.ResetColor();
                return;
            }
        }
        else
        {
            if (hexTileManager.IsBossTile(targetTile))
            {
                HexTile tile = TileDirectionHelper.Instance.GetFrontTile(targetTile, currentTile);
                targetTile = tile;
            }
        }

        if (info.isFace)
        {
            RotateToTile(targetTile);
        }

        // 타일 위치 및 정보 갱신
        currentTile = targetTile;
        targetTile.ResetColor();

        // 코루틴 실행을 위한 최종 목적지 계산
        Vector3 targetPosition = new Vector3(
            targetTile.transform.position.x,
            transform.position.y,
            targetTile.transform.position.z
        );

        StartCoroutine(MoveCoroutine(targetPosition, info.isTurnEnd, info.isDash));
    }


    public void PlayerKnockBack(HexTile tile, bool KnockbackToDeath = false)
    {
        if (tile == null) return;
        Vector3 targetPosition = new Vector3(tile.transform.position.x, transform.position.y, tile.transform.position.z);
        currentTile = tile;
        tile.ResetColor();

        StartCoroutine(MoveCoroutine(targetPosition, false));
    }


    private IEnumerator MoveCoroutine(Vector3 targetPosition, bool isTurnEnd, bool isDash = false)
    {
        if (!isDash) playerAnimation.isMoving = true;
        if (isDash) playerAnimation.UseDash();

        Vector3 startPosition = transform.position;
        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, elapsed / moveDuration); // 부드러운 가속/감속

            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        transform.position = targetPosition; // 정확한 위치 보정
        playerAnimation.isMoving = false;

        if (isTurnEnd) GameManager.Instance.EndPlayerTurn();
    }

    public void RotateToTile(HexTile tile)
    {
        if (tile == null) return;

        Vector3 direction = (tile.transform.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

        StartCoroutine(RotateCoroutine(targetRotation));
    }

    private IEnumerator RotateCoroutine(Quaternion targetRotation)
    {
        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.5f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            yield return null;
        }

        transform.rotation = targetRotation;
    }

    public void SetPath(List<HexTile> newPath)
    {
        if (newPath == null || newPath.Count == 0) return;
        path = new Queue<HexTile>(newPath);
    }

}

public class PlayerMoveInfo
{
    public HexTile tile;
    public bool isDash;
    public bool isFace;
    public bool ignoreDistance;
    public bool isTurnEnd;

    public PlayerMoveInfo(HexTile tile, bool isDash = false, bool isFace = true, bool ignoreDistance = false, bool isTurnEnd = false)
    {
        this.tile = tile;
        this.isDash = isDash;
        this.isFace = isFace;
        this.ignoreDistance = ignoreDistance;
        this.isTurnEnd = isTurnEnd;
    }
}