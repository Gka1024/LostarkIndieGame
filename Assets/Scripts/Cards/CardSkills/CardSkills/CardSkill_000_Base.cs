using System.Collections;
using UnityEngine;

public class CardSkill_Base : CardSkill
{
    protected override SkillObject CreateOption(int num)
    {
        return num switch
        {
            1 => new Default_Base_Option1(),
            // 2 => new GunLance_Bash_Option2(),
            // 3 => new GunLance_Bash_Option3(),
            _ => null
        };
    }

    protected override void SkillAnimation(HexTile tile)
    {
        manager.GetPlayer().GetComponent<PlayerMove>().RotateToTile(tile);
        playerAnimation.ChangeWeapon(PlayerWeapon.Gunlance);
        playerAnimation.PlayAnimation(1);
    }

}

public class Default_Base_Option1 : SkillObject
{
    public override void ApplyOption(CardSkill card)
    {
        var stats = card.GetStats<CardStats_GunLance_Bash>().Clone<CardStats_GunLance_Bash>();
        stats.ApplyOption(1);
    }

    public override IEnumerator Execute(CardSkill card)
    {
        base.Execute(card);
        
        yield return null;
    }
}







