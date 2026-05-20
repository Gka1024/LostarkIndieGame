using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PatternF_Chain_Destruction_Fist : BossPattern
{ // 연환파신권 패턴 미구현
    public PatternF_Chain_Destruction_Fist()
    {
        turnGenerators.Add(MakeBossAir); // 0 : 사라짐
        turnGenerators.Add(MakeIdleTurn); // 1 : 공백
        turnGenerators.Add((ai) => MakeBossDown(ai)); // 1 : 공백
        turnGenerators.Add(MakePattern1); // 2 : 중앙 + 6시 찍기
        turnGenerators.Add(MakePattern2); // 3 : 사방향 찍기
        turnGenerators.Add(MakePattern3); // 4 : 사방향 찍기
        turnGenerators.Add(MakePattern4); // 5 : 사방향 찍기
        turnGenerators.Add(MakePattern5); // 6 : 사방향 찍기
        turnGenerators.Add(MakeIdleTurn); // 7  : 공백
        turnGenerators.Add(MakePattern6); // 8 : 내려치기
        turnGenerators.Add(MakeIdleTurn); // 9 : 내려치기
        turnGenerators.Add(MakePattern6); // 10 : 내려치기
        turnGenerators.Add(MakeIdleTurn); // 11 : 내려치기
        turnGenerators.Add(MakePattern6); // 12 : 내려치기
        turnGenerators.Add(MakePattern7); // 13 : 크게 내려치기
        turnGenerators.Add(MakePattern8); // 14 : 기둥 생성
        turnGenerators.Add(MakeIdleTurn); // 15 : 기둥 생성
        turnGenerators.Add(MakePattern9); // 16 : 주변 공격
        turnGenerators.Add(MakePattern10); // 17 : 기둥 터짐
    }

    public override void OnStartPattern(BossAI ai)
    {
        isTileFixed = false;
        base.OnStartPattern(ai);
    }

    public override void OnAfterTurnExecuted(BossAI ai)
    {
        if (currentTurn == 3)
        {
            isTileFixed = true;
        }
        if (currentTurn == 7)
        {
            isTileFixed = false;
        }
    }

    public override void OnPatternEnd(BossAI ai)
    {
        // 필요 시 정리
    }

    private HashSet<HexTile> explosionRange = new();

    public BossPatternTurnInfo MakePattern1(BossAI ai)
    {
        ai.SetAirborne(false);
        HexTile centerTile = HexTileManager.Instance.GetTileByCube(new Vector3Int(0, 0, 0));
        HexTile attackTile = HexTileManager.Instance.GetTileByCube(new Vector3Int(1, 1, -2));

        HashSet<HexTile> attackRange = new();

        foreach (HexTile tile in HexTileManager.Instance.GetTilesWithinRange(centerTile, 1))
        {
            attackRange.Add(tile);
        }

        foreach (HexTile tile in HexTileManager.Instance.GetTilesWithinRange(attackTile, 2))
        {
            attackRange.Add(tile);
        }

        return BossPatternBuilder.Create(attackRange.ToList()).SetDamage(30f).Build();
    }

    HexTile originalPlayerTile;

    public BossPatternTurnInfo MakePattern2(BossAI ai)
    {
        originalPlayerTile = ai.bossController.GetPlayerTile();
        List<HexTile> attackRange = TileDirectionHelper.Instance.GetSectorTiles(ai.bossInteraction.GetCurrentTile(), ai.bossController.GetPlayerTile(), 5, 90);
        return BossPatternBuilder.Create(attackRange).SetDamage(30f).SetKnockback(1).Build();
    }

    public BossPatternTurnInfo MakePattern3(BossAI ai)
    {
        List<HexTile> attackRange = TileDirectionHelper.Instance.GetSectorTiles(ai.bossInteraction.GetCurrentTile(), originalPlayerTile, 5, 90, 180);
        return BossPatternBuilder.Create(attackRange).SetDamage(30f).SetKnockback(1).Build();
    }

    public BossPatternTurnInfo MakePattern4(BossAI ai)
    {
        List<HexTile> attackRange = TileDirectionHelper.Instance.GetSectorTiles(ai.bossInteraction.GetCurrentTile(), originalPlayerTile, 5, 90, 270);
        return BossPatternBuilder.Create(attackRange).SetDamage(30f).SetKnockback(1).Build();
    }

    public BossPatternTurnInfo MakePattern5(BossAI ai)
    {
        List<HexTile> attackRange = TileDirectionHelper.Instance.GetSectorTiles(ai.bossInteraction.GetCurrentTile(), originalPlayerTile, 5, 90, 90);
        return BossPatternBuilder.Create(attackRange).SetDamage(30f).SetKnockback(1).Build();
    }

    public BossPatternTurnInfo MakePattern6(BossAI ai)
    {
        List<HexTile> attackRange = HexTileManager.Instance.GetTilesWithinRange(Player.Instance.move.GetCurrentTile(), 2);
        explosionRange.AddRange(attackRange);

        return BossPatternBuilder.Create(explosionRange.ToList()).SetDamage(70f).SetKnockback(1).Build();
    }

    public BossPatternTurnInfo MakePattern7(BossAI ai)
    {
        HexTile attackTile = HexTileManager.Instance.GetTileByCube(new Vector3Int(1, 1, -2));
        explosionRange.AddRange(HexTileManager.Instance.GetTilesWithinRange(attackTile, 3));
        return BossPatternBuilder.Create(explosionRange.ToList()).SetDamage(90f).SetKnockback(1).Build();
    }

    private int pillarNum;

    public BossPatternTurnInfo MakePattern8(BossAI ai)
    {
        pillarNum = ai.bossPatternHelper.CreatePillars(2);
        return BossPatternBuilder.Create(new List<HexTile>()).Build();
    }

    public BossPatternTurnInfo MakePattern9(BossAI ai)
    {
        List<HexTile> attackrange = HexTileManager.Instance.GetInvertedTiles(ai.bossPatternHelper.GetPillarSafeTilesWithoutNum(pillarNum));
        return BossPatternBuilder.Create(attackrange).SetDamage(10f).SetKnockback(5).Build();
    }

    public BossPatternTurnInfo MakePattern10(BossAI ai)
    {
        List<HexTile> pillarsToBreak = new();
        HashSet<HexTile> attackRange = new();

        foreach (HexTile tile in HexTileManager.Instance.GetAllTiles())
        {
            if (tile.currentTileState == TileState.IsPillar)
                pillarsToBreak.Add(tile);

            attackRange.AddRange(HexTileManager.Instance.GetTilesWithinRange(tile, 3));
            GameManager.Instance.objectManager
                .DestroyObjectBySpecificTile(tile);
        }

        return BossPatternBuilder.Create(attackRange.ToList()).SetDamage(60f).SetKnockback(5, true).Build();
    }

    public override void PerformActionAnimation(BossAnimation animation)
    {
        // 전멸 패턴 전용 애니메이션 필요 시 여기
    }
}