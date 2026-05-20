using System.Collections.Generic;

public class AreaDamageEffect : FieldEffect
{
    float damage;

    public AreaDamageEffect(List<HexTile> tiles, float damage, int duration)
    {
        this.tiles = tiles;
        this.damage = damage;
        this.duration = duration;
    }

    public override void OnTurnStart()
    {
        base.OnTurnStart();
        bool isBossinArea = HexTileManager.Instance.IsBossTile(this.tiles);

        if (isBossinArea)
        {
            BossDamageData data = DamageSystem.Instance.ProcessDamage(new BossDamageData(damage, 0));
            FieldEffectManager.Instance.boss.bossController.GetBossDamageData(data);
        }

    }

}