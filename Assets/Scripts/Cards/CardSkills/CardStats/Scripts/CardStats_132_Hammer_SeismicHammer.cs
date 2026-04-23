using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Hammer_SeismicHammer", menuName = "CardSystem/Hammer/SeismicHammer")]
public class CardStats_Hammer_SeismicHammer : CardStats
{
    public float opt1_damage_coef;

    public float opt3_damage;

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
                this.tileSelectType = TileSelectType.Around;
                this.aroundRange = 1;
                break;

            default:
                Debug.LogWarning("ApplyOption: 알 수 없는 옵션 번호입니다: " + num);
                break;
        }
    }
}
