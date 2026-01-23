using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TwoHSword_RedDust", menuName = "CardSystem/TwoHSword/RedDust")]
public class CardStats_TwoHSword_RedDust : CardStats
{
    public int base_buff_turns;
    public float base_buff_attack;
    public float base_skill_damage;

    public float opt1_identity_bonus;

    public float opt2_attack_increase;
    public int opt2_increase_turns;

    public override void ApplyOption(int num)
    {
        switch (num)
        {
            case 1:
                identityGain += opt1_identity_bonus;
                break;

            case 2:
                base_buff_attack += opt2_attack_increase;
                base_buff_turns += opt2_increase_turns;
                break;

            case 3:
                isCounterAble = true;
                break;

            default:
                Debug.LogWarning("ApplyOption: 알 수 없는 옵션 번호입니다: " + num);
                break;
        }
    }
}
