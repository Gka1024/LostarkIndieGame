using System.Collections.Generic;
using UnityEngine;

public class EstherSkill_Thrain : EstherSkill
{
    public EstherSkill_Data_Thirain skillData;
    private HexTile spawnTile;

    public override void Init(HexTile spawnTile, GameObject obj)
    {
        base.Init(spawnTile, obj);
        EstherSkillTurnMax = skillData.EstherSkillTurnMax;
        this.spawnTile = spawnTile;
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
        // 2턴 후 데미지 + 무력화
        RegisterTurnAction(2, () =>
        {
            if (estherAnimationController != null) estherAnimationController.PlayAttackAnimation();
            estherManager.ProcessEstherSkillDamageData(new BossDamageData(skillData.skillDamage1, skillData.stagger1, skillData.destroy1));
            VFXSystem.Instance.PlayProjectile(VFXID.Esther_Thirain_Projectile, spawnTile, targetTile, 20f, 1.5f);
        });

        // 5턴 후 데미지 + 무력화
        RegisterTurnAction(3, () =>
        {
            if (estherAnimationController != null) estherAnimationController.PlayAttackAnimation();
            estherManager.ProcessEstherSkillDamageData(new BossDamageData(skillData.skillDamage2, skillData.stagger2, skillData.destroy2));
            VFXSystem.Instance.PlayEffect(VFXID.Esther_Thirain, selectedTiles, 0.1f);
        });
    }
}
