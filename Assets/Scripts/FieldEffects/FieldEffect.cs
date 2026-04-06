using System.Collections.Generic;
using UnityEngine;

public class FieldEffect : MonoBehaviour
{
    public FieldEffectManager effectManager;

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
