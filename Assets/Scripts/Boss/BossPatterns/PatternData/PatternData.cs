using UnityEngine;

public abstract class BossPatternData : ScriptableObject
{
    [Header("Common")]
    public float damage;
    public int knockbackDistance;
    public bool breakWalls;
}