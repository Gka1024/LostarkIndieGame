using System.Collections.Generic;
using UnityEngine;

public class PatternR_Front_Back_Front : BossPattern
{
    // 앞뒤앞 패턴

    public PatternR_Front_Back_Front()
    {
        turnGenerators.Add(MakePattern1);
        turnGenerators.Add(MakePattern2);
        turnGenerators.Add(MakePattern3);
    }

    public override void OnStartPattern(BossAI ai)
    {
        isTileFixed = true;   // 첫 턴은 플레이어 기준 고정
        base.OnStartPattern(ai);
    }

    HexTile fixedTile;

    private BossPatternTurnInfo MakePattern1(BossAI ai)
    {
        HexTile playerTile = GetPlayerTile(true);
        HexTile BossTile = GetBossTile();

        isTileFixed = true;
        fixedTile = playerTile;

        List<HexTile> attackRange = TileDirectionHelper.Instance.GetSectorTiles(BossTile, playerTile, 4, 60);

        return BossPatternBuilder.Create(attackRange).SetDamage(40f).SetKnockback(1).Build();
    }

    private BossPatternTurnInfo MakePattern2(BossAI ai)
    {
        HexTile attackTile = HexTileManager.Instance.GetTileByCube(fixedTile.CubeCoord * -1);
        HexTile BossTile = GetBossTile();

        List<HexTile> attackRange = TileDirectionHelper.Instance.GetSectorTiles(BossTile, attackTile, 4, 60);

        return BossPatternBuilder.Create(attackRange).SetDamage(50).SetKnockback(1).Build();
    }

    private BossPatternTurnInfo MakePattern3(BossAI ai)
    {
        HexTile playerTile = GetPlayerTile(true);
        HexTile BossTile = GetBossTile();

        isTileFixed = true;
        fixedTile = playerTile;

        List<HexTile> attackRange = TileDirectionHelper.Instance.GetSectorTiles(BossTile, playerTile, 6, 80);

        return BossPatternBuilder.Create(attackRange).SetDamage(70).SetKnockback(1).Build();
    }

    public override void OnPatternEnd(BossAI ai)
    {
        isTileFixed = false;
        base.OnPatternEnd(ai);
    }

    public override void PerformActionAnimation(BossAnimation animation)
    {
        // 필요 시 애니메이션 처리
    }
}
