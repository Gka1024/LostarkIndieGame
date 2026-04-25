using UnityEngine;
using System.Collections;

public class VFXAutoReturn : MonoBehaviour
{
    private int _effectId;
    private ParticleSystem _ps;

    private void Awake()
    {
        _ps = GetComponentInChildren<ParticleSystem>();
    }

    public void Initialize(int id, float duration)
    {
        _effectId = id;
        
        StopAllCoroutines();
        StartCoroutine(SoftExitRoutine(duration));
    }

    private IEnumerator SoftExitRoutine(float duration)
    {
        if (_ps != null)
        {
            _ps.Play(true);

            float emitTime = (duration > 0) ? duration : _ps.main.duration;
            yield return new WaitForSeconds(emitTime);

            _ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);

            yield return new WaitForSeconds(_ps.main.startLifetime.constantMax);
        }
        else
        {
            yield return new WaitForSeconds(2.0f);
        }

        VFXManager.Instance.ReturnToPool(_effectId, gameObject);
    }
}