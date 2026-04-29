using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrayManager : Tile_Singleton<TrayManager>
{
    [Header("References")]
    public TrayDomainProcessor TrayDomain;
    public TrayVisualProcessor TrayVisual;

    protected override void Awake()
    {
        base.Awake();
        TrayDomain = GetComponent<TrayDomainProcessor>();
        TrayVisual = GetComponent<TrayVisualProcessor>();
    }

    public void InsertTileInTray(TileController newTile)
    {
        TrayDomain.InsertDomainTray(newTile);
        TrayVisual.InsertVisualTray(newTile);
    }

    public Transform TrayPushSlot()
    {
        return TrayVisual.GetSlot();
        //return TrayDomain.GetRealSlot();
    }

    public void TraySlideTilesAfterTileInserted()
    {
        TrayVisual.SlideTilesAfterTileInserted();
    }

    public void CheckMatch3(TileController newTile)
    {
        TrayDomain.CheckMatch3(newTile);
        TrayVisual.CheckMatch3(newTile);
    }

    public void ClearTray()
    {
        TrayDomain.ClearDomainTray();
        TrayVisual.ClearVisualTray();
    }
}
