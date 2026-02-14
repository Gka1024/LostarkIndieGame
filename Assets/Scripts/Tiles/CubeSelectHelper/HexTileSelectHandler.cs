using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexTileSelectHandler : MonoBehaviour
{
    public GameObject player;
    public HexTileManager tileManager;

    private PlayerMove playerMove;
    private HexTile curHexTile;
    private List<HexTile> previousTilesInRange = new();
    private List<HexTile> tilesInRange = new();

    public bool isSelecting = false;
    public bool isSelectStopped = false;
    public bool needTileColorReset = false;
    public bool isTileSelected = false;

    public Color appropriateColor;
    public Color rangeColor;
    public Color inappropriateColor;

    public HexTile selectedTile;
    public List<HexTile> selectedTiles = new();

    // Selection Settings
    private bool isDistanceNeeded, isAngleNeeded, isAroundNeeded, isRayNeeded, isHexRayNeeded;
    private int selectDistance, selectDistanceRange;
    private int selectAngle, selectAngleRange;
    private int aroundRange;
    private int selectRaydistance, selectRaywidth;

    private readonly Vector3[] HexDir = new Vector3[] {
        new Vector3(1, 0, 0),
        new Vector3(0.5f, 0, Mathf.Sqrt(3)/2),
        new Vector3(-0.5f, 0, Mathf.Sqrt(3)/2),
        new Vector3(-1, 0, 0),
        new Vector3(-0.5f, 0, -Mathf.Sqrt(3)/2),
        new Vector3(0.5f, 0, -Mathf.Sqrt(3)/2)
    };

    private void Awake()
    {
        playerMove = player.GetComponent<PlayerMove>();
    }

    private void Update()
    {
        if (isSelecting && !isSelectStopped) SearchTiles();
        else if (needTileColorReset) ResetAllTileColor();
    }

    public HexTile GetSelectedTile()
    {
        return selectedTile;
    }

    public List<HexTile> GetSelectedTiles()
    {
        return selectedTiles;
    }

    private void SearchTiles()
    {
        ResetTileColorPrevious();

        if (isAngleNeeded) SearchByAngle();
        else if (isDistanceNeeded) SearchByDistance();
        else if (isAroundNeeded) SearchByAround();
        else if (isRayNeeded) SearchByRay();

        previousTilesInRange = new List<HexTile>(tilesInRange);
    }

    public void RegisterHextile(GameObject obj)
    {
        ResetTileColorPrevious();
        curHexTile = obj.GetComponent<HexTile>();
    }

    private void SearchByDistance()
    {
        if (curHexTile == null) return;

        var fromPlayer = tileManager.GetTilesWithinRange(playerMove.GetCurrentTile(), selectDistance);
        tilesInRange = tileManager.GetTilesWithinRange(curHexTile, selectDistanceRange);

        foreach (var tile in fromPlayer)
            tile.meshRenderer.material.color = rangeColor;

        foreach (var tile in tilesInRange)
        {
            int distance = tileManager.GetTileDistance(curHexTile, playerMove.GetCurrentTile());
            ColorTile(tile, distance <= selectDistance);
        }
    }

    private void SearchByAngle()
    {
        if (curHexTile == null || playerMove.GetCurrentTile() == curHexTile) return;

        Vector3 playerPos = playerMove.GetCurrentTile().transform.position;
        Vector3 forwardDir = (curHexTile.transform.position - playerPos).normalized;

        tilesInRange = tileManager.GetTilesWithinRange(playerMove.GetCurrentTile(), selectAngleRange);
        tilesInRange.Remove(playerMove.GetCurrentTile());

        foreach (var tile in tilesInRange)
        {
            Vector3 dirToTile = (tile.transform.position - playerPos).normalized;
            float angle = Mathf.Acos(Mathf.Clamp(Vector3.Dot(forwardDir, dirToTile), -1f, 1f)) * Mathf.Rad2Deg;
            if (angle <= selectAngle * 25f) ColorTile(tile, true);
        }
    }

    private void SearchByAround()
    {
        curHexTile = playerMove.GetCurrentTile();
        tilesInRange = tileManager.GetTilesWithinRange(curHexTile, aroundRange);

        foreach (var tile in tilesInRange)
            ColorTile(tile, true);
    }

    private void SearchByRay()
    {
        if (curHexTile == null || playerMove.GetCurrentTile() == null) return;
        if (tileManager.GetTileDistance(curHexTile, playerMove.GetCurrentTile()) > selectRaydistance) return;

        var playerTile = playerMove.GetCurrentTile();
        Vector3 forwardDir = (curHexTile.transform.position - playerTile.transform.position).normalized;

        if (isHexRayNeeded && !IsHexAligned(forwardDir)) return;

        tilesInRange = tileManager.GetTilesWithinRange(playerTile, selectRaydistance);

        foreach (var tile in tilesInRange)
        {
            Vector3 tileDir = (tile.transform.position - playerTile.transform.position).normalized;
            if (Vector3.Dot(forwardDir, tileDir) > 0.8f && IsTileWithinWidth(playerTile, curHexTile, tile, selectRaywidth))
                ColorTile(tile, true);
        }
    }

    private bool IsHexAligned(Vector3 dir)
    {
        foreach (var hex in HexDir)
            if (Vector3.Dot(dir, hex) > 0.999f) return true;
        return false;
    }

    private bool IsTileWithinWidth(HexTile start, HexTile end, HexTile target, float width)
    {
        float distance = DistancePointToLine(target.transform.position, start.transform.position, end.transform.position);
        return distance <= width;
    }

    private float DistancePointToLine(Vector3 point, Vector3 start, Vector3 end)
    {
        Vector3 lineDir = (end - start).normalized;
        float projLength = Vector3.Dot(point - start, lineDir);
        Vector3 closestPoint = start + projLength * lineDir;
        return Vector3.Distance(point, closestPoint);
    }

    private void ColorTile(HexTile tile, bool isAppropriate)
    {
        tile.meshRenderer.material.color = isAppropriate ? appropriateColor : inappropriateColor;
    }

    private void ResetTileColorPrevious()
    {
        foreach (var tile in previousTilesInRange)
            tile.ResetColor();
        previousTilesInRange.Clear();
    }

    public void ResetVariables()
    {
        isDistanceNeeded = isAngleNeeded = isAroundNeeded = isRayNeeded = isHexRayNeeded = false;
        selectDistance = selectDistanceRange = selectAngle = selectAngleRange = aroundRange = 0;
        selectRaydistance = selectRaywidth = 0;
        isSelecting = false;
        isTileSelected = false;
        selectedTile = null;
        selectedTiles.Clear();
    }

    public void SaveSelectedTiles()
    {
        if (!isSelecting || isSelectStopped) return;
        selectedTile = curHexTile;
        foreach (var tile in tilesInRange)
            if (tile.meshRenderer.material.color == appropriateColor)
                selectedTiles.Add(tile);

        isSelecting = false;
        isTileSelected = true;
        needTileColorReset = true;
    }

    // Selection Entry Points
    public void SelectTileByAngle(int angle, int range) { isAngleNeeded = true; selectAngle = angle; selectAngleRange = range; isSelecting = true; }
    public void SelectTileByDistance(int distance, int range) { isDistanceNeeded = true; selectDistance = distance; selectDistanceRange = range; isSelecting = true; }
    public void SelectTileByAround(int range) { isAroundNeeded = true; aroundRange = range; isSelecting = true; }
    public void SelectTileByRay(int distance, int width) { isRayNeeded = true; selectRaydistance = distance; selectRaywidth = width; isSelecting = true; }
    public void SelectTileByHexRay(int distance, int width) { SelectTileByRay(distance, width); isHexRayNeeded = true; }

    public void StartSelection(CardStats stats)
    {
        ResetVariables();

        switch (stats.tileSelectType)
        {
            case TileSelectType.Angle:
                SelectTileByAngle(stats.skillAngle, stats.skillAngleRange);
                break;

            case TileSelectType.Distance:
                SelectTileByDistance(stats.skillDistance, stats.skillDistanceRange);
                break;

            case TileSelectType.Ray:
                SelectTileByAround(stats.aroundRange);
                break;

            case TileSelectType.HexRay:
                isHexRayNeeded = true;
                SelectTileByRay(stats.rayDistance, stats.rayWidth);
                break;

            default: break;
        }
    }

    public void StartSelection(ChainStats stat)
    {
        ResetVariables();

        switch (stat.tileSelectType)
        {
            case TileSelectType.Angle:
                SelectTileByAngle(stat.skillAngle, stat.skillAngleRange);
                break;

            case TileSelectType.Distance:
                SelectTileByDistance(stat.skillDistance, stat.skillDistanceRange);
                break;

            case TileSelectType.Ray:
                SelectTileByAround(stat.aroundRange);
                break;

            case TileSelectType.HexRay:
                isHexRayNeeded = true;
                SelectTileByRay(stat.rayDistance, stat.rayWidth);
                break;

            default: break;
        }
    }

    public void StartSelection(EstherSkill skill)
    {
        ResetVariables();
        if (skill.isAngleSkill) SelectTileByAngle(skill.skillAngle, skill.skillAngleRange);
        if (skill.isDistanceSkill) SelectTileByDistance(skill.skillDistance, skill.skillDistanceRange);
        if (skill.isRaySkill) SelectTileByRay(skill.rayDistance, skill.rayWidth);
        if (skill.isHexRaySkill) isHexRayNeeded = true;
    }

    public void StartSelectionItemGranades()
    {
        ResetVariables();
        SelectTileByDistance(3, 1);
    }

    public void StartSelectionItemCampFire()
    {
        ResetVariables();
        SelectTileByDistance(1, 0);
    }

    public void StartSelectionItemSpecial() => SelectTileByDistance(1, 0);

    public bool IsTileValid()
    {
        if (isDistanceNeeded && curHexTile != null)
        {
            int dist = tileManager.GetTileDistance(curHexTile, playerMove.GetCurrentTile());
            return dist <= selectDistance;
        }
        return true;
    }

    public void SelectStop(bool show)
    {
        isSelectStopped = !show;
        ResetAllTileColor();
    }
    public void ResetAllTileColor() { tileManager.ResetTileColor(); needTileColorReset = false; }
    public void CancelSelection() { isSelecting = false; isSelectStopped = false; ResetTileColorPrevious(); ResetVariables(); }
}

public enum TileSelectType
{
    None,
    Angle,
    Distance,
    Around,
    Ray,
    HexRay,
}
