using UnityEngine;
using UnityEngine.EventSystems;

public class TripodCancelButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        GameManager.Instance.hexTileSelectHandler.SelectStop(false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.Instance.hexTileSelectHandler.SelectStop(true);
    }
}
