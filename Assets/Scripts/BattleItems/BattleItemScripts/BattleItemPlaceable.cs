using System.Collections;
using UnityEngine;

public class BattleItemPlaceable : MonoBehaviour
{
    public HexTile currentHextile;
    public int placeDuration;

    public void RegisterHextile(HexTile tile)
    {
        currentHextile = tile;
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

    public void SetPlaceDuration(int duration)
    {
        placeDuration = duration;
    }

    public void DestroyGameObject()
    {
        Destroy(this);
    }
}
