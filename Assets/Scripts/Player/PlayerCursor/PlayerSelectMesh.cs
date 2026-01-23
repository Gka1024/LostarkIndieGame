using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class PyramidMesh : MonoBehaviour
{
    public float baseSize = 0.3f; // 바닥 크기
    public float height = 0.3f; // 높이
    public float rotationSpeed = 30f; // 회전 속도

    public GameObject player;

    private Quaternion initialRotation;

    void Start()
    {
        MakeMesh();
        SetShadowSettings();
    }

    void MakeMesh()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        Mesh mesh = new Mesh();

        // 정점(Vertex) 좌표 설정
        Vector3[] vertices = {
            new Vector3(-baseSize / 2, height, -baseSize / 2), // Bottom Left (0)
            new Vector3(baseSize / 2, height, -baseSize / 2),  // Bottom Right (1)
            new Vector3(baseSize / 2, height, baseSize / 2),   // Top Right (2)
            new Vector3(-baseSize / 2, height, baseSize / 2),  // Top Left (3)
            new Vector3(0, 0, 0)                               // Peak (Bottom) (4)
        };

        // 삼각형(Triangle) 설정 (시계 방향)
        int[] triangles = {
            0, 2, 1,  0, 3, 2, // 바닥 (정사각형)
            0, 1, 4,  // 앞면
            1, 2, 4,  // 오른쪽
            2, 3, 4,  // 뒷면
            3, 0, 4   // 왼쪽
        };

        // UV 좌표 설정 (텍스처 적용 가능)
        Vector2[] uv = {
            new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1),
            new Vector2(0.5f, 0.5f) // 정점 4 (꼭짓점)
        };

        // 메시 설정
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mesh.RecalculateNormals(); // 빛 반사 적용

        meshFilter.mesh = mesh;
    }

    void SetShadowSettings()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.shadowCastingMode = ShadowCastingMode.Off; // 그림자 캐스팅 비활성화
        meshRenderer.receiveShadows = false; // 그림자 수신 비활성화
    }

    void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}