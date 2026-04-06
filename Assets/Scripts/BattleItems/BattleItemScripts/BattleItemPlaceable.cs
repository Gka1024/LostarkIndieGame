using System.Collections;
using UnityEngine;

public class BattleItemPlaceable : MonoBehaviour
{
    public HexTile currentTile;
    public int placeDuration;

    public void RegisterHextile(HexTile tile)
    {
        currentTile = tile;
    }

    public virtual void OnturnEnds()
    {
        if (placeDuration > 0)
        {
            placeDuration--;
            if (placeDuration == 0)
            {
                DestroyGameObject();
            }
        }
    }

    public virtual void OnItemPlaced(){}

    public void SetPlaceDuration(int duration)
    {
        placeDuration = duration;
    }

    public void DestroyGameObject()
    {
        Destroy(this);
    }
}
