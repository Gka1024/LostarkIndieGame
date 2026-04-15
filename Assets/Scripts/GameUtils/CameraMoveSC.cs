using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float panSpeed = 20f;
    [SerializeField] private bool useMouseMove;
    [SerializeField] private float panBorderThickness = 20f;

    [Header("Zoom")]
    [SerializeField] private float zoomSpeed = 15f;
    [SerializeField] private float minHeight = 10f;
    [SerializeField] private float maxHeight = 40f;

    [Header("Smooth")]
    [SerializeField] private float moveSmooth = 10f;

    [Header("Target")]
    [SerializeField] private Transform player;

    private Vector3 targetPosition;
    private Vector3 dragStartWorld;

    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
        targetPosition = transform.position;
        AdjustFocusOffset();
    }

    private void LateUpdate()
    {
        HandleDragMove();
        HandleEdgeMove();
        HandleZoom();
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

            // ⭐ 기준점 갱신 (핵심)
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
        if (Input.GetMouseButton(1) || !useMouseMove) return;

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

        targetPosition.y -= scroll * zoomSpeed;
        targetPosition.y = Mathf.Clamp(targetPosition.y, minHeight, maxHeight);
    }

    // ======================
    // Focus Player (Space)
    // ======================

    [SerializeField] private Vector3 focusOffset = new Vector3(0, 0, 0);

    private void AdjustFocusOffset()
    {
        focusOffset = this.transform.position - player.transform.position;
    }

    private void HandleFocus()
    {
        if (player == null) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            targetPosition = player.position + focusOffset;
        }
    }

    // ======================
    // Apply Movement
    // ======================
    private void ApplyMovement()
    {
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
