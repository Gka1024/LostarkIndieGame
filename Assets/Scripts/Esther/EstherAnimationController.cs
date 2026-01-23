using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class EstherAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    //[SerializeField] private AnimatorOverrideController animatorOverrideController;

    public EstherType esther;
    private const float ROTATION_SPEED = 40;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SpawnToGround(HexTile tile)
    {
        StartCoroutine(MoveToGround(tile.GetThisSpawnPos(), 0.2f));
    }

    public void PlayAppearAnimation() => animator.SetTrigger("Appear");
    public void PlayDisappearAnimation() => animator.SetTrigger("Disappear");

    public void PlayAttackAnimation(int index = 1)
    {
        string triggerName = $"Attack{index}";
        animator.SetTrigger(triggerName);
    }


    public void PlayAttack(float delay, Action onHit)
    {
        StartCoroutine(PlayAttackCoroutine(delay, onHit));
    }

    public void Disappear()
    {
        PlayDisappearAnimation();
        Destroy(this.gameObject);
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
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * ROTATION_SPEED);
            yield return null;
        }

        transform.rotation = targetRotation;
    }

    private IEnumerator PlayAttackCoroutine(float delay, Action onHit)
    {
        PlayAttackAnimation();  
        yield return new WaitForSeconds(delay);
    }

    private void OnLand() => animator.SetTrigger("OnLand");

    public IEnumerator MoveToGround(Vector3 targetPosition, float duration)
    {
        Vector3 start = transform.position;
        OnLand();
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            transform.position = Vector3.Lerp(start, targetPosition, time / duration);
            yield return null;
        }
        transform.position = targetPosition;
    }
}
