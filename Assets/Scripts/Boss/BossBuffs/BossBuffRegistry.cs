using UnityEngine;

public class BuffRegistry : MonoBehaviour
{
    public BossBuffData[] allBuffDatas;
    public BossBuffData[] allDebuffDatas;

    void Awake()
    {
        foreach (var buff in allBuffDatas)
        {
            BossBuffFactory.RegisterBuff(buff);
        }

        foreach (var buff in allDebuffDatas)
        {
            BossBuffFactory.RegisterDebuff(buff);
        }
    }
}

public enum EffectSide
{
    Buff,
    Debuff
}

public enum BuffID
{
    NONE = 0,

    BUFF_VALTAN_ARMOR = 101,
    BUFF_RAGE = 102,


    DEBUFF_DEFENCEDOWN = 201,
    DEBUFF_TAUNT = 202,

}

/*

public enum BuffSpecific { Unset, Armor }
public enum DebuffSpecific { Unset, AttackDown, MoreDestruct, DefenceDown, LessShield, Flaming, Stunning, Taunt, }

*/