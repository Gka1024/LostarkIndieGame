using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class BossAnimation : MonoBehaviour
{
    public Animator animator;
    public GameObject baseModel;

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


    public IEnumerator FloatBoss(float height, float duration)
    {
        Vector3 targetPos = new Vector3(0, height, 0);
        float elapsed = 0;
        Vector3 startPos = baseModel.transform.localPosition;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            baseModel.transform.localPosition = Vector3.Lerp(startPos, targetPos, elapsed / duration);
            yield return null;
        }

        baseModel.SetActive(false);
    }

    public IEnumerator LandBoss(float duration)
    {
        baseModel.SetActive(true);
        Vector3 startPos = baseModel.transform.localPosition;
        Vector3 targetPos = new Vector3(0, 1.5f, 0);
        float elapsed = 0;

        // 1. 내려오는 이동 연출
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            // 좀 더 묵직하게 떨어지게 하려면 EaseIn 계열의 보간을 사용하는 것이 좋습니다.
            float t = elapsed / duration;
            t *= t; // Acceleration (가속도 추가)

            baseModel.transform.localPosition = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        baseModel.transform.localPosition = targetPos;
    }

    [SerializeField] private Material originalMaterial;
    [SerializeField] private Material ghostMaterial;

    public void SetGhostMode(bool isGhost)
    {
        SetGhostAppearance(isGhost);
    }

    public void SetGhostAppearance(bool isGhost, float alpha = 0.7f)
    {
        var renderer = GetComponentInChildren<Renderer>();
        if (isGhost)
        {
            SetURPTransparent(alpha);
        }
        else
        {
            renderer.material = originalMaterial;
        }
    }

    public void SetURPTransparent(float alpha)
    {
        var renderer = GetComponentInChildren<Renderer>();
        if (renderer == null) return;

        // material을 사용하면 인스턴스화된 복사본을 수정하므로 원본에 영향을 주지 않습니다.
        Material mat = renderer.material;

        // 1. #008080 컬러 변환
        Color tealColor;
        if (ColorUtility.TryParseHtmlString("#008080", out tealColor))
        {
            tealColor.a = alpha; // 원하는 투명도 설정
        }
        else
        {
            tealColor = new Color(0f, 0.5f, 0.5f, alpha); // 변환 실패 시 기본 Teal 값
        }

        // 2. URP Transparent 설정 (Surface Type 전환)
        mat.SetFloat("_Surface", 1); // 1: Transparent

        // 3. 블렌딩 및 렌더링 옵션 (Alpha 블렌딩 최적화)
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat.SetInt("_ZWrite", 0); // 투명 물체이므로 깊이 쓰기 비활성화

        // 4. 키워드 활성화
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.EnableKeyword("_ALPHABLEND_ON");
        mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

        // 5. 최종 컬러 적용 (URP Lit의 메인 컬러 프로퍼티)
        mat.SetColor("_BaseColor", tealColor);

        // 선택 사항: 유령 느낌을 위해 발광(Emission)도 같은 계열로 추가하면 좋습니다.
        mat.EnableKeyword("_EMISSION");
        mat.SetColor("_EmissionColor", tealColor * 0.5f);
    }

}
