using System.Reflection;
using UnityEngine;

public static class CardUtils
{
    public static void CopyStats(CardStats source, CardStats target)
    {
        var flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        foreach (var field in source.GetType().GetFields(flags))
        {
            field.SetValue(target, field.GetValue(source));
        }
    }
}
