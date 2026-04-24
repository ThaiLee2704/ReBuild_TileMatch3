using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileData : MonoBehaviour
{
    public int Id { get; private set; }
    public int OrderLayer { get; private set; }
    public List<TileController> UpperTiles { get; private set; } = new List<TileController>();
    public List<TileController> LowerTiles { get; private set; } = new List<TileController>();

    public void SetUpData(int id, int orderLayer)
    {
        Id = id;
        OrderLayer = orderLayer;

        UpperTiles.Clear();
        LowerTiles.Clear();
    }

    public bool HaventUpperTiles() => UpperTiles.Count == 0;

    public void RemoveUpperTile(TileController tile)
    {
        UpperTiles.Remove(tile);
    }

    public void ClearLowerTiles()
    {
        LowerTiles.Clear();
    }
}
