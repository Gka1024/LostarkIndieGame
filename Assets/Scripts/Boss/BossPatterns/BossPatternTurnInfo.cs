using System.Collections.Generic;
using System.Drawing;
using System.Linq;

public class BossPatternTurnInfo
{
    public List<HexTile> TargetTiles { get; private set; }
    public float Damage { get; private set; }

    public bool IsDownAttack { get; private set; }
    public int DownDuration { get; private set; }

    public bool IsStunAttack { get; private set; }
    public int StunDuration { get; private set; }

    public bool IsSilenceAttack { get; private set; }
    public int SilenceDuraion { get; private set; }

    public bool IsKnockback { get; private set; }
    public int KnockbackDistance { get; private set; }
    public bool IsKnockbackToDeath { get; private set; }

    public bool IsGrab { get; private set; }
    public bool BreakWalls { get; private set; }
    public bool IsSpecial { get; private set; }

    public PlayerGetDamageInfo ToPlayerDamageInfo()
    {
        return new PlayerGetDamageInfo(
            Damage,
            false,
            isKnockbackAttack: IsKnockback,
            knockbackDistance: KnockbackDistance,
            isKnockbackToDeath : IsKnockbackToDeath,
            isStunAttack: IsStunAttack,
            stunDuration: StunDuration,
            isDownAttack: IsDownAttack,
            downDuration: DownDuration,
            isSilenceAttack: IsSilenceAttack,
            silenceDuration: SilenceDuraion,
            isGrabAttack: IsGrab
        );
    }

    internal BossPatternTurnInfo() { }

    internal void SetTargetTiles(List<HexTile> tiles) => TargetTiles = tiles;
    internal void SetDamage(float damage) => Damage = damage;

    internal void SetDown(int duration)
    {
        IsDownAttack = true;
        DownDuration = duration;
    }

    internal void SetStun(int duration)
    {
        IsStunAttack = true;
        StunDuration = duration;
    }

    internal void SetSilence(int duration)
    {
        IsSilenceAttack = true;
        SilenceDuraion = duration;
    }

    internal void SetKnockback(int distance, bool death = false)
    {
        IsKnockback = true;
        KnockbackDistance = distance;
        IsKnockbackToDeath = death;
    }

    internal void SetGrab() => IsGrab = true;
    internal void SetBreakWalls() => BreakWalls = true;
    internal void SetSpecial() => IsSpecial = true;
}

public class BossPatternBuilder
{
    private BossPatternTurnInfo info;

    private BossPatternBuilder(List<HexTile> tiles)
    {
        info = new BossPatternTurnInfo();
        info.SetTargetTiles(tiles);
    }

    public static BossPatternBuilder Create(List<HexTile> tiles)
    {
        return new BossPatternBuilder(tiles);
    }

    public BossPatternBuilder SetDamage(float damage)
    {
        info.SetDamage(damage);
        return this;
    }

    public BossPatternBuilder SetDown(int duration)
    {
        info.SetDown(duration);
        return this;
    }

    public BossPatternBuilder SetStun(int duration)
    {
        info.SetStun(duration);
        return this;
    }

    public BossPatternBuilder SetSilence(int duration)
    {
        info.SetSilence(duration);
        return this;
    }

    public BossPatternBuilder SetKnockback(int distance, bool death = false)
    {
        info.SetKnockback(distance, death);
        return this;
    }

    public BossPatternBuilder SetGrab()
    {
        info.SetGrab();
        return this;
    }

    public BossPatternBuilder SetBreakWalls()
    {
        info.SetBreakWalls();
        return this;
    }

    public BossPatternBuilder SetSpecial()
    {
        info.SetSpecial();
        return this;
    }

    public BossPatternBuilder SetPreview(Color color)
    {
        return this;
    }

    public BossPatternTurnInfo Build()
    {
        return info;
    }


}

public class PatternUtility
{
    public static BossPatternTurnInfo CreatePatternByDistance(
        BossAI ai,
        (int direction, int count, bool clockwise)[] patterns,
        float damage,
        int downDuration = 0,
        int stunDuration = 0,
        int silenceDuration = 0,
        int knockbackDistance = 0,
        bool isGrab = false,
        bool breakWalls = false,
        bool isSpecial = false
        )
    {
        HashSet<HexTile> attackRangeSet = new();

        var current = ai.bossController.GetCurrentTile();
        var facing = ai.bossController.GetPlayerTile();

        foreach (var (dir, count, clockwise) in patterns)
        {
            attackRangeSet.UnionWith(
                HexTileManager.Instance.tileDirectionHelper
                    .GetDistanceTiles(current, facing, dir, count, clockwise)
            );
        }

        var builder = BossPatternBuilder
            .Create(new List<HexTile>(attackRangeSet))
            .SetDamage(damage);

        if (downDuration > 0)
            builder.SetDown(downDuration);

        if (stunDuration > 0)
            builder.SetStun(stunDuration);

        if (silenceDuration > 0)
            builder.SetSilence(silenceDuration);

        if (knockbackDistance > 0)
            builder.SetKnockback(knockbackDistance);

        if (isGrab)
            builder.SetGrab();

        if (breakWalls)
            builder.SetBreakWalls();

        if (isSpecial)
            builder.SetSpecial();

        return builder.Build();
    }

    public static List<HexTile> GetAttackRangeByDistance(
    BossAI ai,
    (int direction, int count, bool clockwise)[] patterns)
    {
        HashSet<HexTile> attackRangeSet = new();

        var current = ai.bossController.GetCurrentTile();
        var facing = ai.bossController.GetPlayerTile();

        foreach (var (dir, count, clockwise) in patterns)
        {
            var tiles = HexTileManager.Instance.tileDirectionHelper
                .GetDistanceTiles(current, facing, dir, count, clockwise);

            if (tiles != null)
                attackRangeSet.UnionWith(tiles);
        }

        return new List<HexTile>(attackRangeSet);
    }

}
