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

    // [수정] 실행 중인 코루틴을 제어하기 위한 캐싱 변수 추가
    private Coroutine moveCoroutine;
    private Coroutine rotateCoroutine;

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

        currentTile = targetTile;
        targetTile.ResetColor();

        Vector3 targetPosition = new Vector3(
            targetTile.transform.position.x,
            transform.position.y,
            targetTile.transform.position.z
        );

        // [수정] 새로운 이동 명령이 오면 기존 이동 코루틴을 끄고 시작합니다.
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
        moveCoroutine = StartCoroutine(MoveCoroutine(targetPosition, info.isTurnEnd, info.isDash));
    }

    public void PlayerKnockBack(HexTile tile, bool KnockbackToDeath = false)
    {
        if (tile == null) return;
        Vector3 targetPosition = new Vector3(tile.transform.position.x, transform.position.y, tile.transform.position.z);
        currentTile = tile;
        tile.ResetColor();

        // [수정] 넉백은 최우선 순위 이동이므로 기존 이동을 끊어버립니다.
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
        moveCoroutine = StartCoroutine(MoveCoroutine(targetPosition, false));
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
            float t = Mathf.SmoothStep(0, 1, elapsed / moveDuration);

            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        transform.position = targetPosition;
        playerAnimation.isMoving = false;

        // [수정] 루프가 완전히 종료되면 변수 초기화
        moveCoroutine = null;

        if (isTurnEnd) GameManager.Instance.EndPlayerTurn();
    }

    public void RotateToTile(HexTile tile)
    {
        if (tile == null) return;

        Vector3 direction = (tile.transform.position - transform.position).normalized;
        
        // 엣지 케이스 방어: 만약 제자리 회전이거나 타일 위치가 완벽히 겹쳐 direction이 0이 나오면 회전 연산을 패스합니다.
        if (direction.sqrMagnitude < 0.001f) return; 

        Quaternion targetRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

        // [수정] 새로운 회전 명령이 들어오면 돌고 있던 회전 코루틴을 멈추어 부들부들 떨림을 방지합니다.
        if (rotateCoroutine != null) StopCoroutine(rotateCoroutine);
        rotateCoroutine = StartCoroutine(RotateCoroutine(targetRotation));
    }

    private IEnumerator RotateCoroutine(Quaternion targetRotation)
    {
        // 타겟 회전값에 거의 도달할 때까지 보간 회전
        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.5f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            yield return null;
        }

        transform.rotation = targetRotation;
        
        // [수정] 루프 종료 시 변수 초기화
        rotateCoroutine = null;
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