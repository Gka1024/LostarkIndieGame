using UnityEngine;

public class CursorOffset : MonoBehaviour
{
    public float distanceOffsetY = 1.0f;
    public float distanceOffsetZ = 0.3f;

    public GameObject player;
    
    void Update()
    {
        transform.position = player.transform.position + new Vector3(0, distanceOffsetY, distanceOffsetZ);
        transform.localRotation = Quaternion.Inverse(transform.parent.rotation);
    }
}