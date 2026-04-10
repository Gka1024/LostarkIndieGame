using System.Collections.Generic;
using UnityEngine;

public class FieldEffectManager : MonoBehaviour
{
    public static FieldEffectManager Instance { get; private set; }

    public Boss boss;
    public Player player;

    private List<FieldEffect> effects = new();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    public void AddEffect(FieldEffect effect)
    {
        effect.Init();
        effects.Add(effect);
    }

    public void OnTurnStart()
    {
        if (effects.Count == 0) return;

        foreach (FieldEffect effect in effects)
        {
            effect.OnTurnStart();
        }
    }

    public void OnTurnEnd()
    {
        for (int i = effects.Count - 1; i >= 0; i--)
        {
            var effect = effects[i];
            effect.OnTurnEnd();

            if (effect.IsFinished())
            {
                effects.RemoveAt(i);
            }
            else
            {
                effects[i] = effect;
            }
        }
    }

}