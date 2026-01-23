using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TwoHSword_VolcanoEruption", menuName = "CardSystem/TwoHSword/VolcanoEruption")]
public class CardStats_TwoHSword_VolcanoEruption : CardStats
{
    public float base_skill_damage_2;

    public float opt2_damage_coef;

    public override void ApplyOption(int num)
    {
        switch (num)
        {
            case 1:
                this.skillDistance ++;
                break;

            case 2:
                identityGain = 0;
                break;

            case 3:
                isSuperArmor = true;
                break;

            default:
                Debug.LogWarning("ApplyOption: 알 수 없는 옵션 번호입니다: " + num);
                break;
        }
    }
}
