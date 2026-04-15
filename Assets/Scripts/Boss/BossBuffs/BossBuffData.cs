using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Boss/BuffData")]
public class BossBuffData : ScriptableObject
{
    public int buffID;
    public string buffName;

    public float effectValue;

    public BuffSide buffSide;

    [TextArea(3, 6)]
    public string description;
    public Sprite Icon;

    [SerializeReference]
    public BossBuff specificBuff;

}