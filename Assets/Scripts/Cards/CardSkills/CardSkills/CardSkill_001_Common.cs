using System.Collections;
using UnityEngine;

public class CardSkill_Common : CardSkill
{
    protected override SkillObject CreateOption(int num)
    {
        return num switch
        {
            1 => new CardSkill_BasicAttack(),
            2 => new CardSkill_QuickMove(),
            3 => new CardSkill_TurnEnd(),
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

public class CardSkill_BasicAttack : SkillObject
{
    public override void ApplyOption(CardSkill card)
    {
        throw new System.NotImplementedException();
    }
}

public class CardSkill_QuickMove : SkillObject
{
    public override void ApplyOption(CardSkill card)
    {
        card.runtimeCardStats.ApplyOption(2);
    }

    public override IEnumerator Execute(CardSkill card)
    {
        base.Execute(card);

        HexTile tile = SkillManager.Instance.GetSelectedTile();

        PlayerMove playerSC = GameManager.Instance.GetPlayer().GetComponent<PlayerMove>();

        if (!card.manager.hexTileManager.IsBossTile(tile))
        {
            Debug.Log("Move Accepted");
            playerSC.MoveToTile(new PlayerMoveInfo(targetTile, isDash: true, isFace: true));
        }

        yield return 0;
    }
}

public class CardSkill_TurnEnd : SkillObject
{
    public override void ApplyOption(CardSkill card)
    {
        throw new System.NotImplementedException();
    }
}




