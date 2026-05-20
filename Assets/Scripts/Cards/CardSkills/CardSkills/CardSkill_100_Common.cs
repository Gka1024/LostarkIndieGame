using System.Collections;
using UnityEngine;

public class CardSkill_Common : CardSkill
{
    protected override SkillObject CreateOption(int num)
    {
        return num switch
        {
            1 => new CardSkill_TurnEnd(),
            2 => new CardSkill_QuickMove(),
            3 => new CardSkill_BasicAttack(),
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

public class CardSkill_TurnEnd : SkillObject
{
    public override void ApplyOption(CardSkill card)
    {
        card.runtimeCardStats.ApplyOption(1);
    }
}

public class CardSkill_QuickMove : SkillObject
{
    public override void ApplyOption(CardSkill card)
    {
        card.runtimeCardStats.ApplyOption(2);
    }

    public override IEnumerator Execute(CardSkill card, SkillQueueData data, bool isBossHit)
    {
        if (CardList.Instance.CheckSpecialMove())
        {
            PlayerMove playerMove = GameManager.Instance.GetPlayer().GetComponent<PlayerMove>();

            if (!card.manager.hexTileManager.IsBossTile(data.mainTile))
            {
                Debug.Log("Move Accepted");
                playerMove.MoveToTile(new PlayerMoveInfo(data.mainTile, isDash: true, isFace: true, ignoreDistance: true));
            }

            isPlayAnimation = false;

            CardList.Instance.SetSpecialMoveCooldown();

            base.Execute(card, data, isBossHit);

            yield return 0;
        }
        else
        {
            Debug.Log("쿨타임입니다.");
        }
    }
}

public class CardSkill_BasicAttack : SkillObject
{
    public override void ApplyOption(CardSkill card)
    {
        card.runtimeCardStats.ApplyOption(3);
    }
}








