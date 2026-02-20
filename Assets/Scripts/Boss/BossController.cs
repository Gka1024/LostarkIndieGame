using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class BossController : MonoBehaviour
{
    public Boss boss;
    public BossStats bossStats;
    public BossAI bossAI;
    public BossInteraction bossInteraction;
    public BossAnimation bossAnimation;
    public BossStatus bossStatus;

    public HexTile curHexTile;
    public Player player;
    [SerializeField] private HexTile curPlayerTile;

    void Start()
    {
        this.boss = GetComponent<Boss>();
        bossStats = boss.stats;
        bossAI = boss.ai;
        bossInteraction = boss.interaction;
        bossAnimation = boss.animaton;
        bossStatus = boss.status;

        player = boss.player.GetComponent<Player>();

        RegisterCurrentTile();
    }

    private void RegisterCurrentTile()
    {
        if (curHexTile == null && bossInteraction.currentTile != null)
        {
            curHexTile = bossInteraction.GetCurrentTile();
        }
    }

    public void OnTurnEnd()
    {
        bossAI.OnTurnEnd();
        bossStatus.OnTurnEnd();
    }

    public void FindPlayer()
    {
        curPlayerTile = player.move.GetCurrentTile();
        RotateToTile(curPlayerTile);
    }

    public HexTile GetPlayerTile()
    {
        FindPlayer();
        return curPlayerTile;
    }

    public void Taunt(GameObject obj, int duration)
    {
        bossStatus.Taunt(obj, duration);
    }

    public void PlayerKnockBack(int KnockbackDistance, HexTile PlayerTile = null, HexTile BossTile = null)
    {
        if (PlayerTile == null)
        {
            PlayerTile = player.move.GetCurrentTile();
        }

        if (BossTile == null)
        {
            BossTile = GetCurrentTile();
        }

        HexTile tile = HexTileManager.Instance.tileBackHelper.GetBackCube(PlayerTile, BossTile, KnockbackDistance);
        player.move.PlayerKnockBack(tile);
    }

    public void RotateToTile(HexTile tile)
    {
        bossAnimation.RotateToTile(tile);
    }


    public HexTile GetCurrentTile()
    {
        return bossInteraction.GetCurrentTile();
    }

    // 보스 데미지, 무력화, 파괴 및 디버프

    public void GetBossDamageData(BossDamageData data)
    {
        GetBossDamage(data);
        GetBossStagger(data);
        GetBossDestroy(data);
    }

    public void GetBossDamage(BossDamageData data)
    {
        bossStats.ApplyDamage(data);
    }

    public void GetBossStagger(BossDamageData data)
    {
        bossStats.ApplyStagger(data);
    }

    public void GetBossDestroy(BossDamageData data)
    {
        bossStats.ApplyDestroy(data);
    }

    public void GetBossDebuff(BossDebuff debuff)
    {
        bossStatus.AddBossDebuff(debuff);
    }


    public void ShowAttackPreview(BossPatternTurnInfo info)
    {
        List<HexTile> tiles = info.TargetTiles;

        foreach (HexTile tile in tiles)
        {
            tile.isBossAttackRange = true;
            tile.ResetColor();
        }
    }

    public void ClearAttackPreview(BossPatternTurnInfo info)
    {
        List<HexTile> tiles = info.TargetTiles;

        foreach (HexTile tile in tiles)
        {
            tile.isBossAttackRange = false;
            tile.ResetColor();
        }
    }


}
