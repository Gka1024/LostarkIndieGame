using UnityEngine;

public class BossStaggerBar : MonoBehaviour
{
    public BossStats bossStats;

    public RectTransform staggerMask;
    public float maskFullWidth;

    private float maxStagger;

    void Start()
    {
        maskFullWidth = staggerMask.sizeDelta.x;
        maxStagger = BossStats.MAX_STAGGER;
    }

    public void UpdateBossStagger()
    {
        float ratio = Mathf.Clamp01(bossStats.staggerAmount / maxStagger);
        staggerMask.sizeDelta = new Vector2(maskFullWidth * ratio, staggerMask.sizeDelta.y);
    }
}
