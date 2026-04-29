using UnityEngine;

public class EstherUI : MonoBehaviour
{
    public GameObject estherCancelButton;
    public RectTransform estherGaugeMask;
    [SerializeField] private float maskFullWidth;

    public float curEstherRatio;

    private float MAX_ESTHER_VALUE;

    [SerializeField] private GameObject estherCard1;
    [SerializeField] private GameObject estherCard2;
    [SerializeField] private GameObject estherCard3;

    public void Init()
    {
        maskFullWidth = estherGaugeMask.sizeDelta.x;
        MAX_ESTHER_VALUE = EstherManager.Instance.GetMaxEstherValue();
    }

    public void UpdateEstherBar(float estherValue)
    {
        curEstherRatio = Mathf.Clamp01(estherValue / MAX_ESTHER_VALUE);
        estherGaugeMask.sizeDelta = new Vector2(curEstherRatio * maskFullWidth, estherGaugeMask.sizeDelta.y);
        EstherCardBackgroundShow(IsEstherFull());
    }

    private void EstherCardBackgroundShow(bool show)
    {
        estherCard1.GetComponent<EstherCard>()?.ShowBackground(show);
        estherCard2.GetComponent<EstherCard>()?.ShowBackground(show);
        estherCard3.GetComponent<EstherCard>()?.ShowBackground(show);
    }

    private bool IsEstherFull()
    {
        return EstherManager.Instance.IsEstherFull();
    }
}