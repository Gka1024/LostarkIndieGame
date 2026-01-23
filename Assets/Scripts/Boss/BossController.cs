using System.Collections;
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

    // 타일 관련 로직



    // ==== 패턴 보스 제어

    public void MakeBossGroggy(int turns)
    {
        bossStatus.MakeBossGroggy(turns);
        bossAI.ResetCurrentPattern();
    }

    public void MakeBossDestroyable(int turns, int destroyGoals)
    {
        bossStats.SetBossDestroyAvailable(destroyGoals, turns);
    }

    public void StaggerSuccess()
    {
        // todo
    }

    private void RecoverFromGroggy()
    {

    }

    // 보스 데미지, 무력화, 파괴 및 디버프

    public void GetBossDamage(BossDamageInfo info)
    {
        bossStats.ReceiveDamage(new BossDamageData(info.damage));
        bossStats.GetBossStagger(info.stagger);
        bossStats.GetBossDestroy(info.destroy);
    }

    public void GetBossDebuff(BossDebuff debuff)
    {
        bossStatus.AddBossDebuff(debuff);
    }




}
