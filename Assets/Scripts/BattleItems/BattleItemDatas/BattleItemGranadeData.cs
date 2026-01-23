using System.Collections;
using UnityEngine;

public class BattleItemGranadeData : MonoBehaviour
{
    public GranadeType type;

    public float damage;

    public int destruction;
    public float stagger;

    public DebuffType debuffType = DebuffType.Unset;
    public float effectValue;
    public int duration;

}

