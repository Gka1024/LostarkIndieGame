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

    private bool isMoving = false;
    private Queue<HexTile> path = new Queue<HexTile>();


    void Awake()
    {
        playerAnimation = gameObject.GetComponent<PlayerAnimation>();
    }


    void Start()
    {
        currentTile = hexTileManager.GetObjectHextile(gameObject);
    }

    void Update()
    {
        TryMoveAlongPath();
    }

    private void TryMoveAlongPath()
    {
        if (path.Count > 0 && !isMoving)
        {
            MoveToTile(path.Dequeue(), rotate: true);
        }
    }

    public HexTile GetCurrentTile() => currentTile;

    public void MoveToTile(PlayerMoveInfo info)
    {
        HexTile tile = info.tile;

        if (tile == null) return;

        // 강제 이동이 아닐 경우엔 이동 가능 여부를 체크
        if (!info.ignoreDistance)
        {
            if (!hexTileManager.IsTileMoveable(currentTile, tile, moveAbleDistance))
            {
                tile.ResetColor();
                return;
            }
        }
        else
        {
            // 강제 이동 시 보스 타일 체크 (막고 싶을 경우)
            if (hexTileManager.IsBossTile(tile)) return;
        }

        if (info.isFace)
        {
            RotateToTile(tile);
        }

        Vector3 targetPosition = new Vector3(tile.transform.position.x, transform.position.y, tile.transform.position.z);
        currentTile = tile;
        tile.ResetColor();

        StartCoroutine(MoveCoroutine(targetPosition, info.isTurnEnd));
    }

    public void MoveToTile(HexTile tile, bool rotate = true, bool isForceMove = false, bool isTurnEnd = false)
    {

    }

    public void PlayerKnockBack(HexTile tile)
    {
        if (tile == null) return;
        Vector3 targetPosition = new Vector3(tile.transform.position.x, transform.position.y, tile.transform.position.z);
        currentTile = tile;
        tile.ResetColor();

        StartCoroutine(MoveCoroutine(targetPosition, false));
    }


    private IEnumerator MoveCoroutine(Vector3 targetPosition, bool isTurnEnd)
    {
        playerAnimation.isMoving = true;

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