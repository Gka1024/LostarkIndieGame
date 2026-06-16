using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PatternA_SpearAttack : BossPattern
{ // 공중에서 창 내려찍기
    public PatternA_SpearAttack()
    {
        turnGenerators.Add(MakeBossAir);
        turnGenerators.Add(MakeIdleTurn);
        turnGenerators.Add(MakePattern1);
        turnGenerators.Add(MakePattern2);
        turnGenerators.Add(MakePattern1);
        turnGenerators.Add(MakePattern2);
        turnGenerators.Add(MakePattern1);
        turnGenerators.Add(MakePattern2);
        turnGenerators.Add(MakeIdleTurn);
        turnGenerators.Add((ai) => MakeBossDown(ai, HexTileManager.Instance.GetTileByCube(new Vector3Int(0, 0, 0))));
    }

    private HexTile centerTile;

    public override void OnStartPattern(BossAI ai)
    {
        isTileFixed = false;
        base.OnStartPattern(ai);
    }

    private List<HexTile> AttackRange = new();

    private BossPatternTurnInfo MakePattern1(BossAI ai)
    {
        AttackRange = new();
        HexTile randomTile1 = HexTileManager.Instance.GetRandomTile(HexTileManager.Instance.GetAllTiles());
        HexTile randomTile2 = HexTileManager.Instance.GetRandomTile(HexTileManager.Instance.GetAllTiles());
        HexTile playerTile = ai.bossController.GetPlayerTile();

        AttackRange.Add(randomTile1);
        AttackRange.AddRange(randomTile1.neighbors);

        AttackRange.Add(randomTile2);
        AttackRange.AddRange(randomTile2.neighbors);

        AttackRange.Add(playerTile);
        AttackRange.AddRange(playerTile.neighbors);

        return BossPatternBuilder.Create(AttackRange).SetDamage(0).Build();
    }

    private BossPatternTurnInfo MakePattern2(BossAI ai)
    {
        return BossPatternBuilder.Create(AttackRange).SetDamage(50).Build();
    }

    public override void PerformActionAnimation(BossAnimation animation)
    {

        return;
    }
}