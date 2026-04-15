using System.Linq;
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

        Debug.Log($"BuffRegistry : {allBuffDatas.Count() + allDebuffDatas.Count()} 개의 버프 등록 완료.");
    }
}

public enum BuffSide
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
    DEBUFF_ATTACKDOWN = 202,
    DEBUFF_TAUNT = 203,
    DEBUFF_STUN = 204,
    DEBUFF_CORROSION = 205,
    DEBUFF_FRAGILE = 206,
    DEBUFF_FLAME = 207,
    

}

/*

public enum BuffSpecific { Unset, Armor }
public enum DebuffSpecific { Unset, AttackDown, MoreDestruct, DefenceDown, LessShield, Flaming, Stunning, Taunt, }

*/