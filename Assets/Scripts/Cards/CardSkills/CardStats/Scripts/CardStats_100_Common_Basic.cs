using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Common_Basic", menuName = "CardSystem/Common/Basic")]
public class CardStats_Common_Basic : CardStats
{
    public int basicAttackRange = 1;
    public int QuickMoveRange = 3;

    public override void ApplyOption(int num)
    {

        Debug.Log("ApplyOption");

        switch (num)
        {
            case 1: break;
            case 2: SetQuickMove(); break;
            case 3: SetBasicAttack(); break;

            default: Debug.LogWarning("등록되지 않은 스킬입니다."); break;
        }

    }

    private void SetQuickMove()
    {
        Debug.Log("QuickMove");
        tileSelectType = TileSelectType.Distance;
        needToSelectTile = true;
        skillDistance = QuickMoveRange;
        skillDistanceRange = 0;
    }

    private void SetBasicAttack()
    {
        tileSelectType = TileSelectType.Distance;
        skill_damage = 1;
        needToSelectTile = true;
        skillDistance = basicAttackRange;
        skillDistanceRange = 0;
        isCounterAble = true;
    }


}
