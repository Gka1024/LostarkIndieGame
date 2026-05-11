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
    public BossDamagePopup bossDamagePopup;

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
        bossDamagePopup = boss.bossDamagePopup;

        player = boss.player.GetComponent<Player>();

        RegisterCurrentTile();
    }

    public void OnGameStart()
    {
        AddBuff(BossBuffFactory.CreateBuff(BuffID_Boss.BUFF_VALTAN_ARMOR, 2, -1));
        //AddBuff(BossBuffFactory.CreateBuff(BuffID_Boss.DEBUFF_FRAGILE, 2, -1));
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

    public void Stun(int duration)
    {
        bossStatus.MakeBossGroggy(duration);
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

        HexTile tile = HexTileManager.Instance.tileBackHelper.GetBackTile(PlayerTile, BossTile, KnockbackDistance);
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

    // 보스 데미지 및 디버프

    public void AddBuff(BossBuff buff)
    {
        bossStatus.AddBossBuff(buff);
    }

    public void GetBossDamageData(BossDamageData data)
    {
        if(!bossAI.IsAirborne)
        {
            bossDamagePopup.ShowDamage(bossStats.ApplyDamageData(data));
        }
    }

    public void ShowAttackPreview(BossPatternTurnInfo info)
    {
        List<HexTile> tiles = info.TargetTiles;

        HexTileManager.Instance.RegisterAttackPreview(tiles);

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

    public void GiveBossDamageForDebug(float damage)
    {
        BossDamageData data = new(damage);
        GetBossDamageData(data);
    }


}
