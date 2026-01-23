using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CardStats), true)]
public class CardStatsEditor : Editor
{
    SerializedProperty script;

    private void OnEnable()
    {
        script = serializedObject.FindProperty("m_Script");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        CardStats card = (CardStats)target;

        // m_Script 숨기기 (실제로 그리지는 않음)

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("==== 기본 카드 설정 ====", EditorStyles.boldLabel);
        card.CardID = EditorGUILayout.IntField("Card ID", card.CardID);
        card.isCardSkill = EditorGUILayout.Toggle("Is Card Skill", card.isCardSkill);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("==== 딜레이 변수 ====", EditorStyles.boldLabel);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("==== 타일 선택 관련 변수 ====", EditorStyles.boldLabel);
        card.needToSelectTile = EditorGUILayout.Toggle("Need To Select Tile", card.needToSelectTile);
        if (card.needToSelectTile)
        {
            card.isAngleSkill = EditorGUILayout.Toggle("Is Angle Skill", card.isAngleSkill);
            if (card.isAngleSkill)
            {
                EditorGUI.indentLevel++;
                card.skillAngle = EditorGUILayout.IntField("Skill Angle", card.skillAngle);
                card.skillAngleRange = EditorGUILayout.IntField("Skill Angle Range", card.skillAngleRange);
                EditorGUI.indentLevel--;
            }

            card.isDistanceSkill = EditorGUILayout.Toggle("Is Distance Skill", card.isDistanceSkill);
            if (card.isDistanceSkill)
            {
                EditorGUI.indentLevel++;
                card.skillDistance = EditorGUILayout.IntField("Skill Distance", card.skillDistance);
                card.skillDistanceRange = EditorGUILayout.IntField("Skill Distance Range", card.skillDistanceRange);
                EditorGUI.indentLevel--;
            }

            card.isAroundSkill = EditorGUILayout.Toggle("Is Around Skill", card.isAroundSkill);
            if (card.isAroundSkill)
            {
                EditorGUI.indentLevel++;
                card.aroundRange = EditorGUILayout.IntField("Around Range", card.aroundRange);
                EditorGUI.indentLevel--;
            }

            card.isRaySkill = EditorGUILayout.Toggle("Is Ray Skill", card.isRaySkill);
            if (card.isRaySkill)
            {
                EditorGUI.indentLevel++;
                card.isHexRaySkill = EditorGUILayout.Toggle("Is Hex Ray Skill", card.isHexRaySkill);
                card.rayDistance = EditorGUILayout.IntField("Ray Distance", card.rayDistance);
                card.rayWidth = EditorGUILayout.IntField("Ray Width", card.rayWidth);
                EditorGUI.indentLevel--;
            }
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("==== 스킬 상세 변수 ====", EditorStyles.boldLabel);
        card.skill_damage = EditorGUILayout.FloatField("Skill Damage", card.skill_damage);
        card.manaUse = EditorGUILayout.FloatField("Mana Cost", card.manaUse);
        card.identityGain = EditorGUILayout.FloatField("Identity Cost", card.identityGain);
        card.stagger = EditorGUILayout.FloatField("Stagger", card.stagger);
        card.coolDownTurn = EditorGUILayout.IntField("Cooldown Turn", card.coolDownTurn);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("==== 스킬 속성 관련 변수 ====", EditorStyles.boldLabel);
        card.isCounterAble = EditorGUILayout.Toggle("Is Counter Able", card.isCounterAble);
        card.isHeadAttack = EditorGUILayout.Toggle("Is Head Attack", card.isHeadAttack);
        card.isBackAttack = EditorGUILayout.Toggle("Is Back Attack", card.isBackAttack);
        card.isSuperArmor = EditorGUILayout.Toggle("Is Super Armor", card.isSuperArmor);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("==== 자식 클래스 추가 변수 ====", EditorStyles.boldLabel);

        // 모든 프로퍼티 돌면서 기본적으로 그리기, 단 m_Script는 제외
        SerializedProperty prop = serializedObject.GetIterator();
        bool enterChildren = true;
        while (prop.NextVisible(enterChildren))
        {
            enterChildren = false;
            if (prop.name == "m_Script")
                continue;

            if (!IsAlreadyDrawnField(prop.name))
            {
                EditorGUILayout.PropertyField(prop, true);
            }
        }

        serializedObject.ApplyModifiedProperties();

        // 변경 감지
        if (GUI.changed)
        {
            EditorUtility.SetDirty(card);
        }
    }

    private bool IsAlreadyDrawnField(string fieldName)
    {
        // 수동으로 그린 필드들은 true를 반환해서 중복 방지
        switch (fieldName)
        {
            case nameof(CardStats.CardID):
            case nameof(CardStats.isCardSkill):
            case nameof(CardStats.beforeActTurn):
            case nameof(CardStats.afterActTurn):
            case nameof(CardStats.needToSelectTile):
            case nameof(CardStats.isAngleSkill):
            case nameof(CardStats.skillAngle):
            case nameof(CardStats.skillAngleRange):
            case nameof(CardStats.isDistanceSkill):
            case nameof(CardStats.skillDistance):
            case nameof(CardStats.skillDistanceRange):
            case nameof(CardStats.isAroundSkill):
            case nameof(CardStats.aroundRange):
            case nameof(CardStats.isRaySkill):
            case nameof(CardStats.isHexRaySkill):
            case nameof(CardStats.rayDistance):
            case nameof(CardStats.rayWidth):
            case nameof(CardStats.skill_damage):
            case nameof(CardStats.manaUse):
            case nameof(CardStats.identityGain):
            case nameof(CardStats.stagger):
            case nameof(CardStats.coolDownTurn):
            case nameof(CardStats.isCounterAble):
            case nameof(CardStats.isHeadAttack):
            case nameof(CardStats.isBackAttack):
                return true;
            default:
                return false;
        }
    }
}
