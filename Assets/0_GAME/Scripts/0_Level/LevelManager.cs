using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Tile_Singleton<LevelManager>
{
    [Header("Level Data")]
    [SerializeField] int level = 1;
    [SerializeField] int currentLevel;
    [SerializeField] int idLevel = 1;
    private int CurrentMinorLevel = 1;

    protected override void Awake()
    {
        currentLevel = level;
    }

    private void Start()
    {
        LoadLevel();
    }

    public void LoadLevel()
    {
        currentLevel = level;

        RawLevelData rawLevelData = FileReader.GetRawLevelData(level, CurrentMinorLevel, idLevel);
        if (!rawLevelData.IsValid)
            return;

        List<RawTileData> rawTileDatas = FileDecoder.GetRawTileDatas(rawLevelData);
        if (rawTileDatas == null || rawTileDatas.Count == 0)
            return;

        TileSpawner.Instance.SpawnTilesInLevel(rawTileDatas);
    }

    [Button]
    public void NextLevel()
    {
        ClearOldLevelData();
        level++;
        LoadLevel();
    }

    [Button]
    public void ReplayLevel()
    {
        ClearOldLevelData();
        level = currentLevel;
        LoadLevel();
    }

    private void ClearOldLevelData()
    {
        TileSpawner.Instance.ClearTilesInLevel();
        TrayManager.Instance.ClearTray();
    }
}
