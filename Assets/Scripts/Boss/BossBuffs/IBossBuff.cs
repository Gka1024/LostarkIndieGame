public interface IBossBuff
{
    int ID { get; }
    EffectSide Type { get; }
    int Stack { get; set; }
    int Duration { get; set; }

    BossBuffData Data { get; set; }

    void OnApply(BossController boss);
    void OnTick(BossController boss);
    void OnRemove(BossController boss);
}


