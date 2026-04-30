using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawner : Tile_Singleton<TileSpawner>
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] public Sprite[] fruitSprites;
    //[SerializeField] private float offsetYSpawn = 30f;
    //[SerializeField] private float offsetXSpawn = 30f;

    [Header("Tile Spawn in level")]
    public List<TileController> tilesOnBoard = new List<TileController>();
    public List<TileController> clickableTiles = new List<TileController>();

    private Vector2 center;
    public Vector2 CenterPos => center;

    #region Spawn Tiles
    public void SpawnTilesInLevel(List<RawTileData> rawTileDatas)
    {
        ClearTilesInLevel();

        //Kéo Spawner vể (0,0) để không lệch Camera
        center = CalculateSpawnCenter(rawTileDatas);
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
            Vector3 originalTilePos = new Vector3(data.X, data.Y, 0f);

            tile.SetUp(data.Id, data.OrderLayer, icon, originalTilePos);

            tilesOnBoard.Add(tile);
        }

        LevelOverlapProcessor.CaculateOverlaps(tilesOnBoard);

        InitStateAllTiles(tilesOnBoard);
    }
    #endregion

    public void InitStateAllTiles(List<TileController> tiles)
    {
        foreach (TileController tile in tiles)
        {
            tile.UpdateState();
        }
    }

    public void UpdateClickableTile(TileController tile, bool canClick)
    {
        if (tile == null) return;

        if (!tile.gameObject.activeSelf)
        {
            clickableTiles.Remove(tile);
            return;
        }

        if (canClick && !clickableTiles.Contains(tile))
        {
            clickableTiles.Add(tile);
        }
        else if (!canClick && clickableTiles.Contains(tile))
        {
            clickableTiles.Remove(tile);
        }
    }

    public void ClearTile(TileController tile)
    {
        tilesOnBoard.Remove(tile);
        clickableTiles.Remove(tile);
    }

    public void ReturnTile(TileController tile)
    {
        tilesOnBoard.Add(tile);
    }

    public int CountTilesOnBoard(int targetIndex)
    {
        int count = 0;
        foreach (TileController tile in tilesOnBoard)
        {
            if (tile.TileData.Id == targetIndex)
            {
                count++;
            }
        }

        return count;
    }

    public Dictionary<int, int> CountTilesEachId()
    {
        //Đếm số luợng tile của từng Id trong level
        Dictionary<int, int> countTilesEachId = new Dictionary<int, int>();

        foreach (TileController tile in tilesOnBoard)
        {
            if (tile.gameObject.activeSelf)
            {
                int id = tile.TileData.Id;
                if (countTilesEachId.ContainsKey(id))
                    countTilesEachId[id]++;
                else
                    countTilesEachId[id] = 1;
            }
        }

        return countTilesEachId;
    }

    public int FindRandomIdWithEnoughTiles(int amount)
    {
        Dictionary<int, int> countTilesEachId = CountTilesEachId();

        List<int> validIds = new List<int>();

        foreach (var tileId in countTilesEachId.Keys)
        {
            if (countTilesEachId[tileId] >= amount)
                validIds.Add(tileId);
        }

        if (validIds.Count == 0)
            return -1;

        int randomValidId = Random.Range(0, validIds.Count);
        return validIds[randomValidId];
    }

    #region RESET
    public void ClearTilesInLevel()
    {
        for (int i = 0; i < tilesOnBoard.Count; i++)
        {
            if (tilesOnBoard[i] != null)
                tilesOnBoard[i].gameObject.SetActive(false);
        }

        tilesOnBoard.Clear();
        //Mới thêm
        clickableTiles.Clear();
    }
    #endregion

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
