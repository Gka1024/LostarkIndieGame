using Unity.VisualScripting;
using UnityEngine;

public class DamageSystem : MonoBehaviour
{
    public static DamageSystem Instance { get; private set; }

    public PlayerStats playerStats;
    public BossStats bossStats;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }

    public BossDamageData ApplyPlayerStats(BossDamageData data)
    {
        BossDamageData value = new BossDamageData(data.damage, data.stagger, data.destroy, data.isTrueDamage, data.isCounter);

        value.damage *= playerStats.GetCurrentAttack();
        value.stagger *= playerStats.GetStaggerMultiflyer();

        return value;
    }
}