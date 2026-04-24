using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    public PlayerBuffState state;
    public PlayerAnimation anim;
    public PlayerMove move;
    public PlayerStats stats;

    public GameObject PlayerCursor;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (state == null) state = GetComponent<PlayerBuffState>();
        if (anim == null) anim = GetComponent<PlayerAnimation>();
        if (move == null) move = GetComponent<PlayerMove>();
        if (stats == null) stats = GetComponent<PlayerStats>();
    }

    public bool IsMoveable()
    {
        bool value = true;
        if (stats.GetPlayerStun()) return false;
        if (stats.GetPlayerDown()) return false;
        if (QueueManager.Instance.HasAction()) return false;

        return value;

    }

}
