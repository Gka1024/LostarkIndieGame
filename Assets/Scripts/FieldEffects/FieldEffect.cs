using System.Collections.Generic;
using UnityEngine;

public class FieldEffect
{
    public FieldEffectManager effectManager;

    public bool removeFlag;

    public List<HexTile> tiles;
    public int duration;

    public void Init()
    {
        effectManager = FieldEffectManager.Instance;
    }

    public virtual void OnTurnStart() { }

    public void OnTurnEnd()
    {
        duration--;
    }

    public bool IsFinished()
    {
        return duration <= 0;
    }

}
