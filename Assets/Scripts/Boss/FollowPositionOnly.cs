using UnityEngine;

public class FollowPositionOnly : MonoBehaviour
{
    public Transform target;

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = target.position;
        transform.rotation = Quaternion.identity;
    }
}
