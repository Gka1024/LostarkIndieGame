using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSkill_GunLance_BurstCannon : CardSkill
{
    protected override SkillObject CreateOption(int num)
    {
        return num switch
        {
            1 => new GunLance_BurstCannon1(),
            2 => new GunLance_BurstCannon2(),
            3 => new GunLance_BurstCannon3(),
            _ => null
        };
    }

    protected override void SkillAnimation(HexTile tile)
    {
        manager.GetPlayer().GetComponent<PlayerMove>().RotateToTile(tile);
        playerAnimation.PlayAnimation(1);
    }

}

public class GunLance_BurstCannon1 : SkillObject
{
    public override void ApplyOption(CardSkill card)
    {
        Debug.Log("ApplyOption - GunLance_BurstCannon1");
        card.runtimeCardStats.ApplyOption(1);
    }

    public override IEnumerator Execute(CardSkill card)
    {
        yield return base.Execute(card);

        Debug.Log("Execute - GunLance_BurstCannon1");
        // 기본 Execute 로직을 먼저 실행합니다. (보스 피격 여부 등 확인)

        // 플레이어를 타겟 타일의 반대 방향으로 1칸 이동시킵니다.
        PlayerMove playerSC = GameManager.Instance.GetPlayer().GetComponent<PlayerMove>();
        HexTile playerTile = playerSC.GetCurrentTile();
        HexTile targetTile = SkillManager.Instance.selectedTile; // 스킬이 사용된 타일

        Debug.Log(targetTile.CubeCoord);

        // 플레이어의 뒤쪽 타일 계산

        HexTile backTile = HexTileManager.Instance.tileBackHelper.GetBackCube(playerTile, targetTile);

        Debug.Log($"BackTile : {backTile}");

        // 뒤쪽 타일이 존재하고, 보스 타일이 아닐 경우 이동
        if (backTile != null && !card.manager.hexTileManager.IsBossTile(backTile))
        {
            Debug.Log("Move Accepted");
            playerSC.RotateToTile(targetTile);
            playerSC.MoveToTile(new PlayerMoveInfo(backTile, isFace: false));
        }

        yield return null;
    }
}

public class GunLance_BurstCannon2 : SkillObject
{
    public override void ApplyOption(CardSkill card)
    {
        card.runtimeCardStats.ApplyOption(2);
    }
}

public class GunLance_BurstCannon3 : SkillObject
{
    public override void ApplyOption(CardSkill card)
    {
        card.runtimeCardStats.ApplyOption(3);
    }
}


/*
// ==== 트라이포드 1번
    public override void Option1()
    {
        baseCardStats.isChainSkill = true;
    }

    public override void Skill1(bool isBossHit, HexTile tile)
    {
        PlayerMove playerSC = manager.GetPlayer().GetComponent<PlayerMove>();
        HexTile playerTile = playerSC.GetCurrentTile();
        Vector3 newTilePos = 2 * playerTile.transform.position - tile.transform.position;
        HexTile backTile = manager.hexTileManager.IsThereHexTile(newTilePos);

        if (backTile != null || manager.hexTileManager.IsBossTile(backTile))
        {
            playerSC.RotateToTile(tile);
            playerSC.MoveToTile(backTile, false);
        }
    }

    // ==== 트라이포드 2번
    public override void Option2()
    {
        baseCardStats.skillAngleRange += 1;
        baseCardStats.stagger += 30;
    }

    public override void Skill2(bool isBossHit, HexTile tile)
    {
        return;
    }

    // ==== 트라이포드 3번
    public override void Option3()
    {
        baseCardStats.beforeActTurn += 1;
    }

    public override void Skill3(bool isBossHit, HexTile tile)
    {
        return;
    }

    public override void SkillAnimation(HexTile tile)
    {
        return;
    }
*/
