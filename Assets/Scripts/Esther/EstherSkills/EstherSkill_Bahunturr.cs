using System.Collections.Generic;
using UnityEngine;

public class EstherSkill_Bahunturr : EstherSkill
{
    public override void Execute()
    {
        EstherSkillTurnMax = 2;

        // 2턴 후 버프 주기
        RegisterTurnAction(2, () =>
        {
            if (estherAnimationController != null) estherAnimationController.PlayAttackAnimation();
            estherManager.GivePlayerBuff("아크투르스의 가호", 7);
        });
       
    }
}
