using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_GameManager : Tile_Singleton<Tile_GameManager>
{
    [Header("References")]
    [SerializeField] private TileSpawner tileSpawner;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private TrayManager trayManager;

    public void HandlePickTile(TileController tile)
    {
        trayManager.TrayDomain.InsertRealTray(tile);
        var slot = trayManager.TryGetSlot(tile);
        
        if (slot != null)
        {
            //trayManager.realTilesInTray.Add(tile);

            tile.GetComponent<BoxCollider2D>().enabled = false;

            tile.MoveToSlot(slot);

            tile.UpdateTileData();

            tileSpawner.ClearTile(tile);
        }

    }
}
