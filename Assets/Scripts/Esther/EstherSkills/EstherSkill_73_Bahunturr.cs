using System.Collections.Generic;
using UnityEngine;

public class EstherSkill_Bahunturr : EstherSkill
{
    public EstherSkill_Data_Bahuntur skillData;

    public override void Init(HexTile spawnTile)
    {
        base.Init(spawnTile);
        EstherSkillTurnMax = skillData.EstherSkillTurnMax;
    }

    public override void Execute(HexTile targetTile, List<HexTile> selectedTiles)
    {
        // 2턴 후 버프 주기
        RegisterTurnAction(2, () =>
        {
            if (estherAnimationController != null) estherAnimationController.PlayAttackAnimation();
            estherManager.GivePlayerBuff("아크투르스의 가호", skillData.buff_duration);
            VFXManager.Instance.PlayEffect(VFXID.Esther_Bahuntur, targetTile, 5);
        });

    }
}
