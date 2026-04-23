using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawner : Tile_Singleton<TileSpawner>
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private Sprite[] fruitSprites;
    //[SerializeField] private float offsetYSpawn = 30f;
    //[SerializeField] private float offsetXSpawn = 30f;

    [Header("Tile Spawn in level")]
    public List<TileController> tilesInLevel = new List<TileController>();
    public List<Vector3> tilesPosInLevel = new List<Vector3>();

    #region Spawn Tiles
    public /*List<TileController>*/ void SpawnTilesInLevel(List<RawTileData> rawTileDatas)
    {
        ClearData();

        //if (rawTileDatas == null || rawTileDatas.Count == 0)
        //    return tilesInLevel;

        //Kéo Spawner vể (0,0) để không lệch Camera
        Vector2 center = CalculateSpawnCenter(rawTileDatas);
        this.transform.position = new Vector3(-center.x, -center.y, transform.position.z);

        for (int i = 0; i < rawTileDatas.Count; i++)
        {
            RawTileData data = rawTileDatas[i];

            GameObject tileObject = Tile_ObjectPooling.Instance.GetObject(tilePrefab, this.transform);
            tileObject.name = $"Tile_{i}_{data.Id}";
            tileObject.transform.localPosition = new Vector3(data.X/* + offsetXSpawn*/, data.Y/* + offsetYSpawn*/, data.OrderLayer * 0.1f);
            tileObject.SetActive(true);

            TileController tile = tileObject.GetComponent<TileController>();
            Sprite icon = fruitSprites[data.VisualId];

            tile.SetUp(data.Id, data.OrderLayer, icon);

            tilesInLevel.Add(tile);
            tilesPosInLevel.Add(new Vector3(data.X, data.Y, 0f));
        }

        LevelOverlapProcessor.CaculateOverlaps(tilesInLevel);

        UpdateStateAllTiles(tilesInLevel);

        //return tilesInLevel;
    }
    #endregion

    public void UpdateStateAllTiles(List<TileController> tiles)
    {
        foreach (TileController tile in tiles)
        {
            tile.UpdateState();
        }
    }

    public void ClearData()
    {
        for (int i = 0; i < tilesInLevel.Count; i++)
        {
            if (tilesInLevel[i] != null)
            {
                tilesInLevel[i].gameObject.SetActive(false);
            }

            tilesInLevel.Clear();
            tilesPosInLevel.Clear();
        }
    }

    #region Helper Methods
    private Vector2 CalculateSpawnCenter(List<RawTileData> spawnDatas)
    {
        float minX = float.MaxValue;
        float maxX = float.MinValue;
        float minY = float.MaxValue;
        float maxY = float.MinValue;

        for (int i = 0; i < spawnDatas.Count; i++)
        {
            RawTileData data = spawnDatas[i];

            if (data.X < minX) minX = data.X;
            if (data.X > maxX) maxX = data.X;
            if (data.Y < minY) minY = data.Y;
            if (data.Y > maxY) maxY = data.Y;
        }

        return new Vector2((minX + maxX) * 0.5f, (minY + maxY) * 0.5f);
    }
    #endregion
}
