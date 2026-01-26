using UnityEngine;

public class BossDamagePopup : MonoBehaviour
{
    public Camera mainCamera;
    public Canvas canvas;
    public RectTransform popupRoot;
    public GameObject damageTextPrefab;
    public Transform worldAnchor; // 보스 머리 위

    public Vector2 damageOffset = new(0, 0);

    public void ShowDamage(float damage)
    {
        Vector3 screenPos = mainCamera.WorldToScreenPoint(worldAnchor.position);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            popupRoot, screenPos, canvas.worldCamera, out Vector2 localPos);

        var obj = Instantiate(damageTextPrefab, popupRoot);
        RectTransform rt = obj.GetComponent<RectTransform>();
        rt.anchoredPosition = localPos + damageOffset;

        obj.GetComponent<DamageText>().Init(damage);
    }
}
