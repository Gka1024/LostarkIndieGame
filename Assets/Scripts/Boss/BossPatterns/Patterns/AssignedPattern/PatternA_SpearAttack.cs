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
        turnGenerators.Add(MakePattern1);
        turnGenerators.Add(MakePattern1);
        turnGenerators.Add(MakeIdleTurn);
        turnGenerators.Add((ai) => MakeBossDown(ai, HexTileManager.Instance.GetTileByCube(new Vector3Int(0, 0, 0))));
    }

    private HexTile centerTile;

    public override void OnStartPattern(BossAI ai)
    {
        isTileFixed = false;
        base.OnStartPattern(ai);
    }

    private BossPatternTurnInfo MakePattern1(BossAI ai)
    {
        HexTile randomTile = HexTileManager.Instance.GetRandomTile(HexTileManager.Instance.GetAllTiles());
        HexTile playerTile = ai.bossController.GetPlayerTile();

        List<HexTile> attackRange = new();

        attackRange.Add(randomTile);
        attackRange.AddRange(randomTile.neighbors);

        attackRange.Add(playerTile);
        attackRange.AddRange(playerTile.neighbors);

        return BossPatternBuilder.Create(attackRange).SetDamage(30).Build();

    }

    public override void PerformActionAnimation(BossAnimation animation)
    {

        return;
    }
}