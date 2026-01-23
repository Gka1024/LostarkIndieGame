using UnityEngine;

public class EstherSkill_Thrain : EstherSkill
{
    public float skillDamage1;
    public float skillDamage2;

    public float stagger1;
    public float stagger2;

    public override void Execute()
    {
        EstherSkillTurnMax = 5;

        // 2턴 후 데미지 + 무력화
        RegisterTurnAction(2, () =>
        {
            if (estherAnimationController != null) estherAnimationController.PlayAttackAnimation();
            estherManager.ProcessEstherSkillDamage(skillDamage1);
        });

        // 5턴 후 데미지 + 무력화
        RegisterTurnAction(5, () =>
        {
            if (estherAnimationController != null) estherAnimationController.PlayAttackAnimation();
            estherManager.ProcessEstherSkillDamage(skillDamage2);
        });
    }
}
    