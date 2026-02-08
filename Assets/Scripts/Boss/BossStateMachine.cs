using UnityEngine;

public class BossStateMachine
{
    [SerializeField] private BossState state = BossState.Idle;


}

public enum BossState
{
    Idle,
}
