using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class BossAnimation : MonoBehaviour
{
    public Animator animator;

    public bool isMoving;
    public bool isRunning;

    private static readonly int IsWalkingHash = Animator.StringToHash("isWalking");
    private static readonly int IsRunningHash = Animator.StringToHash("isRunning");
    private static readonly int IsSpinningHash = Animator.StringToHash("isSpinning"); // Animator 파라미터

    
    private bool isSpinning = false;
    private Coroutine spinRoutine;

    void Update()
    {
        UpdateIsMoving();
    }

    private void UpdateIsMoving()
    {
        if (animator != null)
        {
            animator.SetBool(IsWalkingHash, isMoving);
            animator.SetBool(IsRunningHash, isRunning);
        }
    }

    /// <summary>
    /// 일정 시간 동안 회전 애니메이션을 재생합니다.
    /// </summary>
    public void SpinForSeconds(float duration)
    {
        if (isSpinning)
        {
            // 이미 회전 중이면 중복 실행 방지
            if (spinRoutine != null)
                StopCoroutine(spinRoutine);
        }

        spinRoutine = StartCoroutine(SpinRoutine(duration));
    }

    private IEnumerator SpinRoutine(float duration)
    {
        isSpinning = true;
        animator.SetBool(IsSpinningHash, true); // 회전 시작

        yield return new WaitForSeconds(duration); // duration초 동안 유지

        animator.SetBool(IsSpinningHash, false); // 회전 종료
        isSpinning = false;
        spinRoutine = null;
    }

    

    public void StartSpin()
    {
        animator.SetBool(IsSpinningHash, true);
    }

    public void StopSpin()
    {
        animator.SetBool(IsSpinningHash, false);
    }

    public void FlashCounterBlueLight()
    {
        // TODO: 반격 시 파란 빛 효과
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
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 20f);
            yield return null;
        }

        transform.rotation = targetRotation;
    }

}
