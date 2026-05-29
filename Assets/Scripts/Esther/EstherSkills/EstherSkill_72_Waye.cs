using System.Collections.Generic;
using UnityEngine;

public class EstherSkill_Waye : EstherSkill
{
    public EstherSkill_Data_Waye skillData;

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
        RegisterTurnAction(2, () =>
       {
           if (estherAnimationController != null) estherAnimationController.PlayAttackAnimation();
           estherManager.ProcessEstherSkillDamageData(new BossDamageData(skillData.skillDamage1, skillData.stagger1));
           VFXSystem.Instance.PlayEffect(VFXID.Esther_Waye, targetTile);
       });

        RegisterTurnAction(3, () =>
        {
            if (estherAnimationController != null) estherAnimationController.PlayAttackAnimation();
            estherManager.ProcessEstherSkillDamageData(new BossDamageData(skillData.skillDamage2, skillData.stagger2));
            VFXSystem.Instance.PlayEffect(VFXID.Esther_Waye, targetTile);
        });

        RegisterTurnAction(4, () =>
        {
            if (estherAnimationController != null) estherAnimationController.PlayAttackAnimation();
            estherManager.ProcessEstherSkillDamageData(new BossDamageData(skillData.skillDamage3, skillData.stagger3));
            VFXSystem.Instance.PlayEffect(VFXID.Esther_Waye, targetTile);
        });
    }
}
