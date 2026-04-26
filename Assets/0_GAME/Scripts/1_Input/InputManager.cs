using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : Tile_Singleton<InputManager>
{
    [Header("Input Settings")]
    [SerializeField] private Camera mainCam;

    [SerializeField] private LayerMask tileLayerMask;

    [Header("Runtime Data")]
    [SerializeField] private TileController currentTile;

    protected override void Awake()
    {
        base.Awake();
        mainCam = Camera.main;
    }

    private void Update()
    {
        if (TrayManager.Instance == null) return;

        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;

        if (Input.GetMouseButtonDown(0))
            HandleMouseDownClick();
        else if (Input.GetMouseButtonUp(0))
            HandleMouseUpClick();
    }

    private void HandleMouseDownClick()
    {
        TileController tile = GetTile();
        Vector2 mouPos = mainCam.ScreenToWorldPoint(Input.mousePosition);

        if (tile == null)
        {
            //Particle Empty Click

            currentTile = null;
            return;
        }

        currentTile = tile;
    }

    private void HandleMouseUpClick()
    {
        TileController tile = GetTile();

        if (currentTile == null) 
             return;

        if (currentTile != tile)
        {
            currentTile = null;
            return;
        }

        //Handle Tile Click
        if (currentTile == tile && !TrayManager.Instance.TrayDomain.IsFull)
            Tile_GameManager.Instance.HandlePickTile(currentTile);

        currentTile = null;
    }

    private TileController GetTile()
    {
        TileController tile = null;
        Vector2 mouPos = mainCam.ScreenToWorldPoint(Input.mousePosition);

        Collider2D hit = Physics2D.OverlapPoint(mouPos, tileLayerMask);

        if (hit != null && hit.TryGetComponent(out TileController tileClicked))
        {
            tile = tileClicked;
        }

        return tile;
    }
}
