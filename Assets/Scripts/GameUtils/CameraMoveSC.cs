using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float panSpeed = 20f;
    [SerializeField] private bool useMouseMove;
    [SerializeField] private float panBorderThickness = 20f;

    [Header("Map Bounds")]
    [SerializeField] private bool useBounds = false;
    [SerializeField] private Vector2 minBounds; // 맵의 최소 X, Z
    [SerializeField] private Vector2 maxBounds; // 맵의 최대 X, Z

    [Header("Zoom")]
    [SerializeField] private float zoomSpeed = 15f;
    [SerializeField] private float minHeight = 10f;
    [SerializeField] private float maxHeight = 40f;

    [Header("Smooth")]
    [SerializeField] private float moveSmooth = 10f;

    [Header("Target")]
    [SerializeField] private Transform player;

    private Vector3 targetPosition;
    private Camera cam;

    // [수정] 고정된 Vector3 대신 카메라의 피치(기울기) 각도에 따른 수평 오프셋만 유지합니다.
    private Vector3 horizontalFocusOffset;

    private Vector3 initialFullOffset;
    private float initialHeight; // 초기 높이

    private void Awake()
    {
        cam = Camera.main;
        targetPosition = transform.position;

        if (player != null)
        {
            // Y축(높이)을 포함한 카메라와 플레이어의 '초기 상대 위치 벡터'를 통째로 저장합니다.
            initialFullOffset = transform.position - player.position;
            initialHeight = transform.position.y;
        }
    }

    private void LateUpdate()
    {
        // 줌과 드래그가 동시에 일어나서 생기는 떨림 방지
        if (!isDragging)
        {
            HandleZoom();
        }

        HandleDragMove();
        HandleEdgeMove();
        HandleFocus();

        ApplyMovement();
    }

    // ======================
    // Drag Move (Right Click)
    // ======================
    private bool isDragging;
    private Vector3 lastDragWorld;

    private void HandleDragMove()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isDragging = true;
            lastDragWorld = GetMouseWorldPosition();
        }

        if (Input.GetMouseButton(1) && isDragging)
        {
            Vector3 currentWorld = GetMouseWorldPosition();
            Vector3 delta = lastDragWorld - currentWorld;
            delta.y = 0;

            targetPosition += delta;
            lastDragWorld = currentWorld;
        }

        if (Input.GetMouseButtonUp(1))
        {
            isDragging = false;
        }
    }

    // ======================
    // Screen Edge Move
    // ======================
    private void HandleEdgeMove()
    {
        // [수정] 드래그 중이 아닐 때만 엣지 이동 작동 (마우스 밖으로 나가는 버그 방지)
        if (isDragging || !useMouseMove) return;

        Vector3 dir = Vector3.zero;

        if (Input.mousePosition.y >= Screen.height - panBorderThickness)
            dir += transform.forward;
        if (Input.mousePosition.y <= panBorderThickness)
            dir -= transform.forward;
        if (Input.mousePosition.x >= Screen.width - panBorderThickness)
            dir += transform.right;
        if (Input.mousePosition.x <= panBorderThickness)
            dir -= transform.right;

        dir.y = 0;

        if (dir.sqrMagnitude > 0)
        {
            targetPosition += dir.normalized * panSpeed * Time.deltaTime;
        }
    }

    // ======================
    // Zoom
    // ======================
    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) < 0.01f) return;

        Vector3 zoomDirection = transform.forward;

        Vector3 zoomMovement = zoomDirection * scroll * zoomSpeed;

        Vector3 nextTargetPos = targetPosition + zoomMovement;

        float clampedY = Mathf.Clamp(nextTargetPos.y, minHeight, maxHeight);

        if (nextTargetPos.y >= minHeight && nextTargetPos.y <= maxHeight)
        {
            targetPosition = nextTargetPos;
        }
        else
        {
            float currentY = targetPosition.y;
            if (currentY != clampedY)
            {
                float remainingFactor = (clampedY - currentY) / zoomMovement.y;
                targetPosition += zoomMovement * remainingFactor;
            }
        }
    }

    // ======================
    // Focus Player (Space)
    // ======================
    private void HandleFocus()
    {
        if (player == null) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            targetPosition = player.position + initialFullOffset;
        }
    }
    
    // ======================
    // Apply Movement
    // ======================
    private void ApplyMovement()
    {
        // [추가] 맵 영역 제한 처리
        if (useBounds)
        {
            targetPosition.x = Mathf.Clamp(targetPosition.x, minBounds.x, maxBounds.x);
            targetPosition.z = Mathf.Clamp(targetPosition.z, minBounds.y, maxBounds.y);
        }

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            Time.deltaTime * moveSmooth
        );
    }

    // ======================
    // Utils
    // ======================
    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Plane ground = new Plane(Vector3.up, Vector3.zero);
        ground.Raycast(ray, out float distance);
        return ray.GetPoint(distance);
    }
}