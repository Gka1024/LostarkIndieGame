using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectClickHandler : MonoBehaviour
{
    public bool isClickAvailable;

    [SerializeField] private GameManager manager;
    [SerializeField] private CardManager cardManager;
    [SerializeField] private SkillManager skillManager;
    [SerializeField] private PlayerMove player;
    [SerializeField] private GameObject playerCursor;
    [SerializeField] private HexTileManager tileManager;
    [SerializeField] private HexTileSelectHandler hexTileSelectHandler;

    public bool isPlayerClicked;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isClickAvailable)
        {
            HandleClick();
        }
    }

    public void SetClickAvailable(bool value)
    {
        isClickAvailable = value;
    }

    private void HandleClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit)) return;

        if (hexTileSelectHandler.isSelecting)
        {
            TrySelectTile(hit);
            return;
        }

        if (hit.collider.TryGetComponent<PlayerMove>(out PlayerMove _))
        {
            if (skillManager.CanMove())
            {
                SetPlayerClickState(true);
            }
            else
            {
                Debug.Log("playerClicked but Player can't move");
            }
        }
        else if (hit.collider.TryGetComponent<HexTile>(out HexTile clickedTile))
        {
            if (isPlayerClicked)
            {
                HandleTileMovement(clickedTile);
            }
        }
    }

    private void TrySelectTile(RaycastHit hit)
    {
        if (hit.collider.TryGetComponent<HexTile>(out HexTile tile))
        {
            hexTileSelectHandler.SaveSelectedTiles();
        }
    }

    private void HandleTileMovement(HexTile tile)
    {
        SetPlayerClickState(false);

        if (!tileManager.IsTileMoveable(player.GetCurrentTile(), tile, player.moveAbleDistance))
        {
            Debug.Log("그곳으로는 움직일 수 없습니다.");
            tile.ResetColor();
            return;
        }

        if (!skillManager.CanMove())
        {
            Debug.Log("캐릭터 후딜레이 중입니다.");
            return;
        }

        player.MoveToTile(new PlayerMoveInfo(tile, isTurnEnd: true));
    }

    private void SetPlayerClickState(bool clicked)
    {
        isPlayerClicked = clicked;
        playerCursor.SetActive(clicked);
        tileManager.ChangeTileColorIfMoveable(clicked);
    }

    public bool IsPlayerClicked() => isPlayerClicked;
}
