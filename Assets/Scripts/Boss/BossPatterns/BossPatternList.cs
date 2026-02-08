using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPatternList : MonoBehaviour
{
    [SerializeField] private Boss boss;
    [SerializeField] private BossAI bossAI;
    [SerializeField] private BossStats bossStats;

    public List<BossPattern> BossPatternList1 = new(); // 130줄(바훈투르) 전까지
    public List<BossPattern> BossPatternList2 = new(); // 85줄(지파) 전까지
    public List<BossPattern> BossPatternList3 = new(); // 45줄(2지파) 전까지
    public List<BossPattern> BossPatternList4 = new(); // 30줄(2지파) 이후
    public List<BossPattern> BossPatternList5 = new(); // 발탄 유령

}