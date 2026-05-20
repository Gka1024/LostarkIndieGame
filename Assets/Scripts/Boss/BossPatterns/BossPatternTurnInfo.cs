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
