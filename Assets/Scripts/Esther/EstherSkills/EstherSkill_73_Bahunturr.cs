using System.Collections.Generic;
using UnityEngine;

public class EstherSkill_Bahunturr : EstherSkill
{
    public EstherSkill_Data_Bahuntur skillData;

    public override void Init(HexTile spawnTile, GameObject obj)
    {
        base.Init(spawnTile, obj);
        EstherSkillTurnMax = skillData.EstherSkillTurnMax;
    }

    public override void OnTurnPassed()
    {
        base.OnTurnPassed();
        if (currentTurn >= skillData.EstherSkillTurnMax)
        {
            isFinished = true;
        }
    }

    public override void Execute(HexTile targetTile, List<HexTile> selectedTiles)
    {
        // 2턴 후 버프 주기
        RegisterTurnAction(1, () =>
        {
            if (estherAnimationController != null) estherAnimationController.PlayAttackAnimation();
            estherManager.GivePlayerBuff(skillData.buff_duration);
            VFXManager.Instance.PlayEffect(VFXID.Esther_Bahuntur, targetTile, 5, -1.96f);
        });

    }
}
