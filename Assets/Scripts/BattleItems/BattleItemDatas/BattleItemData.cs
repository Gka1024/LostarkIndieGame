using UnityEngine;

[CreateAssetMenu(fileName = "New BattleItemData", menuName = "BattleItem/BattleItem")]
public class BattleItemData : ScriptableObject
{
    public int itemID;

    public Sprite itemIcon;
    public ItemType itemType;

    [Header("Item Stats")]
    public float damage;
    public int destruction;
    public float stagger;

    [Header("Boss Effects")]
    public bool hasDebuff;
    public BuffID buffID;
    public float effectValue;
    public int duration;

    [Header("Item Type")]
    public GranadeType granadeType;
    public PotionType potionType;
    public SpecialType specialType;

}

