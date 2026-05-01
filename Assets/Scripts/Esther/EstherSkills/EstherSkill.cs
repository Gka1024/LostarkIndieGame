using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class EstherSkill : MonoBehaviour
{
    public EstherManager estherManager;
    public EstherAnimationController estherAnimationController;

    protected int EstherSkillTurnMax;
    protected int currentTurn = 0;
    public bool isFinished;

    // 타일 선택 조건들
    public TileSelectType tileSelectType;
    public bool needToSelectTile;

    public int skillAngle;
    public int skillAngleRange;

    public int aroundRange;

    public int skillDistance;
    public int skillDistanceRange;

    public int rayDistance;
    public int rayWidth;

    public GameObject EstherModeling;

    // 각 턴마다 실행할 액션 정의
    protected Dictionary<int, Action> turnTriggers = new();

    public virtual void Init(HexTile spawnTile, GameObject modeling)
    {
        EstherModeling = modeling;
        isFinished = false;
    }

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

    public virtual void OnTurnPassed()
    {
        currentTurn++;

        if (turnTriggers.TryGetValue(currentTurn, out var action))
        {
            action?.Invoke();
        }

        if (currentTurn >= EstherSkillTurnMax)
        {
            Debug.Log($"DestroySkill | CurrentTurn : {currentTurn} | estherSkillTurnMax : {EstherSkillTurnMax}");
        }
    }

    public void FinishSkill()
    {
        Destroy(EstherModeling);
        Destroy(gameObject, 0.5f);
    }

    public abstract void Execute(HexTile tile, List<HexTile> targetTiles);  // 실행 시 반드시 수동 호출
}
