using System.Linq;
using UnityEngine;

public class PlayerBuffRegistry : MonoBehaviour
{
    public PlayerBuffData[] allBuffDatas;
    public PlayerBuffData[] allDebuffDatas;

    void Awake()
    {
        foreach (var buff in allBuffDatas)
        {
            PlayerBuffFactory.RegisterBuff(buff);
        }

        foreach (var buff in allDebuffDatas)
        {
            PlayerBuffFactory.RegisterDebuff(buff);
        }

        Debug.Log($"BuffRegistry : {allBuffDatas.Count() + allDebuffDatas.Count()} 개의 버프 등록 완료.");
    }
}


public enum BuffID_Player
{
    NONE = 0,
    ESTHER_BAHUNTUR = 1,

    PLAYER_ATTACK_UP = 101,
    PLAYER_SHIELD = 102,
    PLAYER_MANA_REGEN = 103,
    ITEM_HIDING_ROBE = 104,
    PLAYER_SUPER_ARMOR = 105,

    STUN = 201,
    DOWN = 202,
    SILENCE = 203,
    
    

}

/*

public enum BuffSpecific { Unset, Armor }
public enum DebuffSpecific { Unset, AttackDown, MoreDestruct, DefenceDown, LessShield, Flaming, Stunning, Taunt, }

*/