using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class EstherSkill : MonoBehaviour
{
    public EstherManager estherManager;
    public EstherAnimationController estherAnimationController;

    public int EstherSkillTurnMax;  // 최대 턴 수 (예: 5)
    private int currentTurn = 0;

    // 타일 선택 조건들
    public bool needToSelectTile;

    public bool isAngleSkill;
    public int skillAngle;
    public int skillAngleRange;

    public bool isDistanceSkill;
    public int skillDistance;
    public int skillDistanceRange;

    public bool isRaySkill;
    public bool isHexRaySkill;
    public int rayDistance;
    public int rayWidth;

    // 각 턴마다 실행할 액션 정의
    protected Dictionary<int, Action> turnTriggers = new();

    public void SelectTile()
    {
        GameManager.Instance.hexTileSelectHandler.StartSelection(this);
    }

    public void SpawnToGround(HexTile tile)
    {
        estherAnimationController.SpawnToGround(tile);
    }

    public void RegisterTurnAction(int turnNumber, Action action)
    {
        if (turnTriggers.ContainsKey(turnNumber))
            turnTriggers[turnNumber] += action;
        else
            turnTriggers[turnNumber] = action;
    }

    public void OnTurnPassed()
    {
        currentTurn++;

        if (turnTriggers.TryGetValue(currentTurn, out var action))
        {
            action?.Invoke();
        }

        if (currentTurn >= EstherSkillTurnMax)
        {
            Debug.Log($"DestroySkill | CurrentTurn : {currentTurn} | estherSkillTurnMax : {EstherSkillTurnMax}");
            DestroyGameObject();
        }
    }

    protected void DestroyGameObject()
    {
        if (estherManager != null)
            estherManager.ClearEstherSkill();
        estherAnimationController.Disappear();
        Destroy(gameObject);
    }

    public abstract void Execute();  // 실행 시 반드시 수동 호출
}
