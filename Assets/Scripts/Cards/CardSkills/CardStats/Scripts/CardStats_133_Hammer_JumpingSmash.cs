using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Hammer_JumpingSmash", menuName = "CardSystem/Hammer/JumpingSmash")]
public class CardStats_Hammer_JumpingSmash : CardStats
{
    public float opt3_skill_damage;

    public override void ApplyOption(int num)
    {
        switch (num)
        {
            case 1:
                this.isCounterAble = true;
                this.beforeActTurn -= 1;
                break;

            case 2:
                break;

            case 3:
                this.skill_damage += opt3_skill_damage;

                this.tileSelectType = TileSelectType.Around;
                this.aroundRange = 2;
                break;

            default:
                Debug.LogWarning("ApplyOption: 알 수 없는 옵션 번호입니다: " + num);
                break;
        }
    }
}
