using System.Collections.Generic;
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
            isStunAttack: IsStunAttack,
            stunDuration: StunDuration,
            isDownAttack: IsDownAttack,
            downDuration: DownDuration,
            isSilenceAttack: IsSilenceAttack,
            silenceDuration: SilenceDuraion
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

    internal void SetKnockback(int distance)
    {
        IsKnockback = true;
        KnockbackDistance = distance;
    }

    internal void SetGrab() => IsGrab = true;
    internal void SetBreakWalls() => BreakWalls = true;
    internal void SetSpecial() => IsSpecial = true;
}

public class BossPatternTurnBuilder
{
    private BossPatternTurnInfo info;

    private BossPatternTurnBuilder(List<HexTile> tiles)
    {
        info = new BossPatternTurnInfo();
        info.SetTargetTiles(tiles);
    }

    public static BossPatternTurnBuilder Create(List<HexTile> tiles)
    {
        return new BossPatternTurnBuilder(tiles);
    }

    public BossPatternTurnBuilder SetDamage(float damage)
    {
        info.SetDamage(damage);
        return this;
    }

    public BossPatternTurnBuilder SetDown(int duration)
    {
        info.SetDown(duration);
        return this;
    }

    public BossPatternTurnBuilder SetStun(int duration)
    {
        info.SetStun(duration);
        return this;
    }

    public BossPatternTurnBuilder SetSilence(int duration)
    {
        info.SetSilence(duration);
        return this;
    }

    public BossPatternTurnBuilder SetKnockback(int distance)
    {
        info.SetKnockback(distance);
        return this;
    }

    public BossPatternTurnBuilder SetGrab()
    {
        info.SetGrab();
        return this;
    }

    public BossPatternTurnBuilder SetBreakWalls()
    {
        info.SetBreakWalls();
        return this;
    }

    public BossPatternTurnBuilder SetSpecial()
    {
        info.SetSpecial();
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

        var builder = BossPatternTurnBuilder
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

}
