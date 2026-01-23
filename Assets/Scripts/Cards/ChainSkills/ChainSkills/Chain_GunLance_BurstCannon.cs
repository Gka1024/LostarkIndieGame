using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainSkill_GunLance_BurstCannon : ChainSkill
{
    public override HexTile GetTargetTile(HexTile tile)
    {
        return base.GetTargetTile(tile);
    }

    public override List<HexTile> GetDamageTiles(List<HexTile> tiles)
    {
        return base.GetDamageTiles(tiles);
    }

    public override IEnumerator ExecuteChain(SkillQueueData data)
    {
        PlayerMove playerSC = GameManager.Instance.GetPlayer().GetComponent<PlayerMove>();
        HexTile playerTile = playerSC.GetCurrentTile();
        HexTile targetTile = SkillManager.Instance.selectedTile; // 스킬이 사용된 타일
        
        // 플레이어의 앞쪽 타일 계산

        HexTile frontTile = HexTileManager.Instance.tileFrontHelper.GetFrontCube(playerTile, targetTile);

        Debug.Log($"FrontTile : {frontTile}, TileCoord : {frontTile.CubeCoord}");

        // 뒤쪽 타일이 존재하고, 보스 타일이 아닐 경우 이동

        if (frontTile != null && !GameManager.Instance.hexTileManager.IsBossTile(frontTile))
        {
            Debug.Log("Move Accepted");
            playerSC.MoveToTile(new PlayerMoveInfo(targetTile));
        }

        yield return null;
    }


}





