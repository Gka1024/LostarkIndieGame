using UnityEditor;
using UnityEngine;

public class AnimationClipReverser
{
    [MenuItem("Tools/Reverse Selected Animation")]
    static void ReverseAnimation()
    {
        var selected = Selection.activeObject as AnimationClip;
        if (selected == null)
        {
            Debug.LogError("애니메이션 클립을 선택해줘!");
            return;
        }

        string path = AssetDatabase.GetAssetPath(selected);
        AnimationClip newClip = new AnimationClip
        {
            frameRate = selected.frameRate
        };

        EditorCurveBinding[] curveBindings = AnimationUtility.GetCurveBindings(selected);
        foreach (var binding in curveBindings)
        {
            AnimationCurve curve = AnimationUtility.GetEditorCurve(selected, binding);
            Keyframe[] keys = curve.keys;
            float clipLength = keys[keys.Length - 1].time;

            for (int i = 0; i < keys.Length; i++)
            {
                keys[i].time = clipLength - keys[i].time;
            }

            System.Array.Sort(keys, (a, b) => a.time.CompareTo(b.time));

            AnimationCurve reversedCurve = new AnimationCurve(keys);
            AnimationUtility.SetEditorCurve(newClip, binding, reversedCurve);
        }

        string newPath = path.Replace(".anim", "_Reversed.anim").Replace(".fbx", "_Reversed.anim");
        AssetDatabase.CreateAsset(newClip, newPath);
        AssetDatabase.SaveAssets();
        Debug.Log("역재생 애니메이션 생성 완료: " + newPath);
    }
}
