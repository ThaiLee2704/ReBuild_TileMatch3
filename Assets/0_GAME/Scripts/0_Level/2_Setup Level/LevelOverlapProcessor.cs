using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LevelOverlapProcessor
{
    public static void CaculateOverlaps(List<TileController> tilesInLevel)
    {
        Dictionary<Vector2, List<TileController>> spatialGrid = new Dictionary<Vector2, List<TileController>>();

        foreach (TileController tile in tilesInLevel)
        {
            Vector2 cellPos = tile.transform.position;

            //Một ô cell có thể chứa nhiều tile, nên ta dùng List để lưu trữ các tile trong cùng một ô
            if (!spatialGrid.ContainsKey(cellPos))
            {
                spatialGrid[cellPos] = new List<TileController>();
            }
            spatialGrid[cellPos].Add(tile);

        }

        foreach (TileController tile in tilesInLevel)
        {
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    Vector2 cellCheck = new Vector2(tile.transform.position.x + x, tile.transform.position.y + y);

                    if (spatialGrid.TryGetValue(cellCheck, out List<TileController> neighborTiles))
                    {
                        CheckOverlap(tile, neighborTiles);
                    }
                }
            }
        }
    }

    private static void CheckOverlap(TileController tile, List<TileController> neighborTiles)
    {
        foreach (TileController neighborTile in neighborTiles)
        {
            if (tile == neighborTile || tile.TileData.OrderLayer >= neighborTile.TileData.OrderLayer)
                continue;

            float distanceX = Mathf.Abs(tile.transform.position.x - neighborTile.transform.position.x);
            float distanceY = Mathf.Abs(tile.transform.position.y - neighborTile.transform.position.y);

            if (distanceX < 2f && distanceY < 2f)
            {
                tile.TileData.UpperTiles.Add(neighborTile);
                neighborTile.TileData.LowerTiles.Add(tile);
            }
        }
    }
}