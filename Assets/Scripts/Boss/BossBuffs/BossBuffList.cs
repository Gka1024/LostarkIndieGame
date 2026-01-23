using UnityEngine;

public class BuffRegistry : MonoBehaviour
{
    public BossBuffData[] allBuffDatas;
    public BossBuffData[] allDebuffDatas;

    void Awake()
    {
        foreach(var buff in allBuffDatas)
        {
            BossBuffFactory.RegisterBuff(buff);
        }

        foreach(var buff in allDebuffDatas)
        {
            BossBuffFactory.RegisterDebuff(buff);
        }
    }
}