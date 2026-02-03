using Unity.Mathematics;
using UnityEngine;

public class Lonaun : MonoBehaviour
{
    [SerializeField] private float offset = 0.5f;
    [SerializeField] private float speed = 2f;
    private Vector3 startPos;

    void Start()
    {
        startPos = this.transform.position;
    }

    void Update()
    {
        float y = math.sin(Time.time * speed) * offset;
        this.transform.position = startPos + new Vector3(0, y, 0);

    }

}
