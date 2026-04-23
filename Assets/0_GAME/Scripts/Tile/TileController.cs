using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    private BoxCollider2D boxCollider;

    [Header("Reference")]
    public TileData tileData;
    public TileGraphic tileGraphic;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();

        if (tileData == null)
            tileData = GetComponent<TileData>();
        if (tileGraphic == null)
            tileGraphic = GetComponentInChildren<TileGraphic>();
    }

    public void SetUp(int id, int orderLayer, Sprite icon)
    {
        tileData.SetUpData(id, orderLayer);
        tileGraphic.SetUpGraphic(icon, orderLayer);
    }

    public void UpdateState()
    {
        bool haventUpperTiles = tileData.HaventUpperTiles();

        tileGraphic.UpdateColor(haventUpperTiles);

        if (haventUpperTiles)
            boxCollider.enabled = true;
        else
            boxCollider.enabled = false;
    }
}
