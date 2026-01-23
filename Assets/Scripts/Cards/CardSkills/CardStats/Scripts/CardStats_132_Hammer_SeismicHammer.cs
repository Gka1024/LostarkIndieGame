using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Hammer_SeismicHammer", menuName = "CardSystem/Hammer/SeismicHammer")]
public class CardStats_Hammer_SeismicHammer : CardStats
{
    public float opt1_damage_coef;

    public float opt3_damage_coef;

    public override void ApplyOption(int num)
    {
        switch (num)
        {
            case 1:
                this.skill_damage += opt1_damage_coef;
                break;

            case 2:
                this.beforeActTurn = 0;
                break;

            case 3:
                this.HasChainSkill = true;
                this.ChainSkill = CardList.Instance.GetChainSkills(this.CardID, 3);

                this.isRaySkill = false;
                this.isAroundSkill = true;
                this.aroundRange = 1;
                break;

            default:
                Debug.LogWarning("ApplyOption: 알 수 없는 옵션 번호입니다: " + num);
                break;
        }
    }
}
