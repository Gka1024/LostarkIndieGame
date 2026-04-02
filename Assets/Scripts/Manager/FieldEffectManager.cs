using System.Collections.Generic;
using UnityEngine;

public class FieldEffectManager : MonoBehaviour
{
    public static FieldEffectManager Instance { get; private set; }

    public Boss boss;
    public Player player;

    private List<FieldEffect> effects;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    public void AddEffect(FieldEffect effect)
    {
        effects.Add(effect);
    }

    public void OnTurnStart()
    {
        foreach(FieldEffect effect in effects)
        {
            effect.OnTurnStart();
        }
    }

    public void OnTurnEnd()
    {
        foreach(FieldEffect effect in effects)
        {
            effect.OnTurnEnd();
        }
    }



}