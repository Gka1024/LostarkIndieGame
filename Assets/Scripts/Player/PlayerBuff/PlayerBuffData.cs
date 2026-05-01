using UnityEngine;

[CreateAssetMenu(menuName ="new PlayerBuffsData")]
public class PlayerBuffData : ScriptableObject
{
    public int buffID;
    public string buffName;

    public BuffSide buffSide;

    [TextArea(3,6)]
    public string description;
    public Sprite Icon;

    [SerializeReference]
    public PlayerBuff specificBuff;

}