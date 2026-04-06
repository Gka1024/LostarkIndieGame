using System.Collections.Generic;

public class AreaHealEffect : FieldEffect
{
    float healAmount;

    public AreaHealEffect(List<HexTile> tiles, float healAmount, int duration)
    {
        this.tiles = tiles;
        this.healAmount = healAmount;
        this.duration = duration;

    }

    public override void OnTurnStart()
    {
        base.OnTurnStart();
        bool isPlayerInArea;

        Player player = GameManager.Instance.GetPlayer().GetComponent<Player>();
        HexTile playerTile = player.move.GetCurrentTile();

        isPlayerInArea = tiles.Contains(playerTile);

        if (isPlayerInArea)
        {
            player.stats.Heal(healAmount);
        }

    }

}