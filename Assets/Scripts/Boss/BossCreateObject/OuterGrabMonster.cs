using System.Collections.Generic;
using UnityEngine;

public class OuterGrabMonster : MonoBehaviour
{
    private PatternR_Outer_Grab ownerPattern;
    private BossAI bossAI;

    public List<HexTile> MyAttackTiles = new(); // 이 분신이 공격할 범위
    public HexTile MyTile;

    public void Init(PatternR_Outer_Grab pattern, HexTile tile, List<HexTile> tiles, BossAI ai)
    {
        ownerPattern = pattern;
        MyTile = tile;
        bossAI = ai;
        MyAttackTiles = tiles;
        MyTile.RegisterBossObject(this);

    }

    public void OnHitByPlayer(bool isCounterAttack)
    {
        if (isCounterAttack)
        {
            // 패턴 클래스에게 내 공격 범위를 제거해달라고 요청
            ownerPattern.RemoveAttackRange(MyAttackTiles);
            // 보스 버프 감소
            bossAI.bossStatus.RemoveBossBuffStack((int)BuffID_Boss.BUFF_VALTAN_ARMOR);
            // 분신 제거 (연출 후 파괴 권장)
            Destroy(gameObject);
        }
    }
}