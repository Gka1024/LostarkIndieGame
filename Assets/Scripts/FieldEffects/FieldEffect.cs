using System.Collections.Generic;
using UnityEngine;

public class FieldEffect : MonoBehaviour
{
    public List<HexTile> tiles;
    public int duration;

    public virtual void OnTurnStart(){}

    public void OnTurnEnd()
    {
        duration--;
    }

    public bool IsFinished()
    {
        return duration <= 0;
    }



    
}
