using Unity.Mathematics;
using UnityEngine;

public class BossEffectController : MonoBehaviour
{
    [SerializeField] private Transform HitEffectAnchor;
    [SerializeField] private Transform StunEffectAnchor;

    [SerializeField] private GameObject GuardEffect;
    [SerializeField] private GameObject HitEffect1;
    [SerializeField] private GameObject HitEffect2;
    [SerializeField] private GameObject HitEffect3;
    [SerializeField] private GameObject HitEffect4;

    [SerializeField] private GameObject StunEffect;



    public void PlayGroggyEffect(bool value)
    {
        StunEffect.SetActive(value);
    }


    public void SpawnHitEffect()
    {
        Instantiate(HitEffect2, HitEffectAnchor.position, Quaternion.identity, HitEffectAnchor);
    }

    public void SpawnGaurdEffect()
    {
        Instantiate(GuardEffect, HitEffectAnchor.position, Quaternion.identity, HitEffectAnchor);
    }
}
