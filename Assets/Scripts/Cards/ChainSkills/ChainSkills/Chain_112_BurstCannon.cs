using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainSkill_GunLance_BurstCannon : ChainSkill
{
    public override IEnumerator ExecuteChain(SkillQueueData data, bool isBossHit)
    {
        PlayerMove move = Player.Instance.move;
        HexTile playerTile = move.GetCurrentTile();
        HexTile targetTile = data.mainTile;
        
        // 플레이어의 앞쪽 타일 계산

        Debug.Log("ExecuteChain : ChainSkill_GunLance_BurstCannon");

        HexTile frontTile = HexTileManager.Instance.tileFrontHelper.GetFrontTile(playerTile, targetTile);

        Debug.Log($"FrontTile : {frontTile}, TileCoord : {frontTile.CubeCoord}");

        // 앞쪽 타일이 존재하고, 보스 타일이 아닐 경우 이동

        if (frontTile != null && !GameManager.Instance.hexTileManager.IsBossTile(frontTile))
        {
            Debug.Log("Move Accepted");
            move.MoveToTile(new PlayerMoveInfo(frontTile, ignoreDistance: true));
        }

        yield return null;
    }


}





