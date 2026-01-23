using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Hammer_PerfectSwing", menuName = "CardSystem/Hammer/PerfectSwing")]
public class CardStats_Hammer_PerfectSwing : CardStats
{
    public int opt1_turns;
    public float opt1_damage_coef;

    public bool opt2_isTaunt = false;
    public int opt2_taunt_turns;

    public float opt3_skill_damage_1;
    public float opt3_skill_damage_2;

    public override void ApplyOption(int num)
    {
        switch (num)
        {
            case 1:
                beforeActTurn += opt1_turns;
                skill_damage += opt1_damage_coef;
                break;

            case 2:
                afterActTurn = beforeActTurn;
                beforeActTurn = 0;
                HasChainSkill = true;
                skill_damage = 0;
                break;

            case 3:
                skill_damage = opt3_skill_damage_1;
                HasChainSkill = true;
                break;

            default:
                Debug.LogWarning("ApplyOption: 알 수 없는 옵션 번호입니다: " + num);
                break;
        }
    }
}
/*
public override void ApplyOption(int num)
    {
        switch (num)
        {
            case 1:
                break;

            case 2:
                break;

            case 3:
                break;

            default:
                Debug.LogWarning("ApplyOption: 알 수 없는 옵션 번호입니다: " + num);
                break;
        }
    }
    */
