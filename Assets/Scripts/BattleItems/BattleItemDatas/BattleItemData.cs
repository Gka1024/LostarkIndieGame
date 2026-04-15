using UnityEngine;

[CreateAssetMenu(fileName = "New BattleItemData", menuName = "BattleItem/BattleItem")]
public class BattleItemData : ScriptableObject
{
    public int itemID;

    public Sprite itemIcon;
    public ItemType itemType;

    [Header("Item Stats")]
    public float value;
    public int duration;

    public float HP_decrease; // 아트로핀용
    public int additional_move; // 추가 이동 아이템용

    [Header("Boss Effects")]
    public float damage;
    public int destruction;
    public float stagger;

    public bool hasDebuff;
    public BuffID buffID;
    public int buff_duration;

    [Header("Item Type")]
    public GranadeType granadeType;
    public PotionType potionType;
    public SpecialType specialType;

}

