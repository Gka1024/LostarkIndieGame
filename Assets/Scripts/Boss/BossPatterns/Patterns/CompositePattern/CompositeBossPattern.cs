using System.Collections.Generic;
using UnityEngine;

public class CompositeBossPattern : BossPattern
{
    private BossPattern mainPattern;
    private BossPattern subPattern;

    public CompositeBossPattern(BossPattern main, BossPattern sub)
    {
        this.mainPattern = main;
        this.subPattern = sub;
        
        // 부모의 데이터를 초기화 (기본적으로 main 패턴의 설정을 따름)
        this.isTileFixed = main.IsTileFixed();
    }

    public override void OnStartPattern(BossAI ai)
    {
        base.OnStartPattern(ai);
        mainPattern.OnStartPattern(ai);
        subPattern.OnStartPattern(ai);
    }

    public override BossPatternTurnInfo GenerateTurn(BossAI ai)
    {
        // 1. 두 패턴의 현재 턴 데이터를 각각 생성
        BossPatternTurnInfo mainInfo = mainPattern.GenerateTurn(ai);
        BossPatternTurnInfo subInfo = subPattern.GenerateTurn(ai);

        // 2. 만약 한쪽이 널이라면(패턴 길이가 다름) 다른 한쪽만 반환
        if (mainInfo == null) return subInfo;
        if (subInfo == null) return mainInfo;

        // 3. 데이터 병합 (mainInfo를 기준으로 subInfo의 타일들을 합침)
        // 새로운 객체를 생성하여 원본 데이터 오염 방지
        BossPatternTurnInfo combinedInfo = BossPatternBuilder.Create(new List<HexTile>(mainInfo.TargetTiles))
            .SetDamage(mainInfo.Damage) // 기본 데미지는 메인 패턴 기준 (필요 시 로직 수정)
            .Build();

        // 서브 패턴의 타일들을 추가
        foreach (var tile in subInfo.TargetTiles)
        {
            if (!combinedInfo.TargetTiles.Contains(tile))
            {
                combinedInfo.TargetTiles.Add(tile);
            }
        }

        // 상태값 동기화 (예: 한쪽이라도 즉사기면 즉사기로 취급)
        if (subInfo.IsSpecial) combinedInfo.SetSpecial();

        this.currentTurnInfo = combinedInfo;
        return combinedInfo;
    }

    public override void CompleteTurn()
    {
        // 두 패턴의 턴 카운트를 동시에 올림
        mainPattern.CompleteTurn();
        subPattern.CompleteTurn();

        // 부모(Composite)의 상태 업데이트
        this.currentTurn++;
        
        // 메인 패턴이 끝나면 이 복합 패턴도 끝난 것으로 간주
        if (mainPattern.IsFinished)
        {
            this.isFinished = true;
        }
    }

    public override void PerformActionAnimation(BossAnimation animation)
    {
        // 메인 공격 애니메이션 실행
        mainPattern.PerformActionAnimation(animation);

        // 서브 패턴은 보통 외곽 폭발 같은 '환경 요소'이므로 
        // 애니메이션 대신 이펙트 매니저를 통해 별도 연출을 실행하는 것이 좋습니다.
        // 만약 subPattern에도 애니메이션이 있다면 여기서 추가 호출
        subPattern.PerformActionAnimation(animation);
    }

    public override void OnAfterTurnExecuted(BossAI ai)
    {
        mainPattern.OnAfterTurnExecuted(ai);
        subPattern.OnAfterTurnExecuted(ai);
    }

    public override void OnPatternEnd(BossAI ai)
    {
        mainPattern.OnPatternEnd(ai);
        subPattern.OnPatternEnd(ai);
    }

    public override void OnInterrupted(BossAI ai)
    {
        mainPattern.OnInterrupted(ai);
        subPattern.OnInterrupted(ai);
    }
}