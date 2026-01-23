using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TwoHSword_FinishStrike", menuName = "CardSystem/TwoHSword/FinishStrike")]
public class CardStats_TwoHSword_FinishStrike : CardStats
{
    public float opt2_skill_damage;
    public float opt3_skill_damage;

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
}
