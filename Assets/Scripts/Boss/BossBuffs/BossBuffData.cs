using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/BuffData")]
public class BossBuffData : ScriptableObject
{
    public int buffID;
    public string buffName;

    public float effectValue;

    public BuffType buffType;
    public DebuffType debuffType;

    [TextArea(3, 6)]
    public string description;
    public Sprite Icon;

    

}