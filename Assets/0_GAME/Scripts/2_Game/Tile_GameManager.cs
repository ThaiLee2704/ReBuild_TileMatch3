using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_GameManager : Tile_Singleton<Tile_GameManager>
{
    [Header("References")]
    [SerializeField] private TileSpawner tileSpawner;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private TrayManager trayManager;

    public void HandlePickTile(TileController newTile)
    {
        //Đã xác nhận được Tile hợp lệ, trayDomain chưa đầy thì chắc chắn
        //có thể insert tile vào tray và lấy slot
        //Gọi TrayManager xử lý việc đưa tile vào TrayDomain và TrayVisual
        trayManager.InsertTileInTray(newTile);

        var slot = trayManager.TrayPushSlot();

        //Slide những tile sau insertIndex sang phải 1 slot
        trayManager.TraySlideTilesAfterTileInserted();

        newTile.GetComponent<BoxCollider2D>().enabled = false;
        newTile.UpdateTileData();

        tileSpawner.ClearTile(newTile);

        newTile.MoveToSlot(slot);

        trayManager.CheckMatch3(newTile);
    }

    [Button]
    public void HandleUndoTile()
    {
        var undoHistory = TrayManager.Instance.TrayVisual.undoHistory;

        if (undoHistory.Count == 0) return;

        int lastIndex = undoHistory.Count - 1;
        TileController targetTile = undoHistory[lastIndex];

        int removeIndex = TrayManager.Instance.TrayVisual.visualTilesInTray.IndexOf(targetTile);

        undoHistory.RemoveAt(lastIndex);
        TrayManager.Instance.TrayDomain.realTilesInTray.Remove(targetTile);
        TrayManager.Instance.TrayVisual.visualTilesInTray.Remove(targetTile);

        tileSpawner.ReturnTile(targetTile);

        targetTile.MoveBackUndo();
        targetTile.UndoTileData();
        targetTile.GetComponent<BoxCollider2D>().enabled = true;


        for (int i = removeIndex; i < TrayManager.Instance.TrayVisual.visualTilesInTray.Count; i++)
        {
            var visualTilesInTray = TrayManager.Instance.TrayVisual.visualTilesInTray;
            var slots = TrayManager.Instance.TrayVisual.slots;
            visualTilesInTray[i].Slide(slots[i], 0.2f);
        }
    }

    //Test Auto Match3 V2
    [Button]
    public void HandleAutoMatch3()
    {
        // 1. Phân tích khay để tìm ID ưu tiên và số lượng cần lấy
        int targetId = -1;
        int amountNeeded = 0;

        Dictionary<int, int> trayTileCounts = TrayManager.Instance.TrayDomain.GetTrayTileFrequencies();

        // IN LOG KIỂM TRA DATA KHAY
        string trayLog = "<color=orange>[Booster] Dữ liệu Khay: </color>";
        foreach (var p in trayTileCounts) trayLog += $"ID {p.Key}({p.Value} viên) | ";
        Debug.Log(trayLog);

        //Ưu tiên tìm thằng nào đang có 2 viên trong khay
        foreach (var pair in trayTileCounts)
        {
            if (pair.Value == 2)
            {
                int leftOnBoard = tileSpawner.CountTilesOnBoard(pair.Key);
                Debug.Log($"<color=cyan>[Booster] Đang xét ID {pair.Key} (Ưu tiên 1). Trên bàn cờ tìm thấy: {leftOnBoard} viên.</color>");

                if (leftOnBoard >= 1)
                {
                    targetId = pair.Key;
                    amountNeeded = 1;
                    break;
                }
                else
                {
                    Debug.Log($"<color=yellow>[Booster] ĐÃ BỎ QUA ID {pair.Key} vì trên bàn cờ đã hết hàng!</color>");
                }
            }
        }

        //Nếu không có thằng 2 viên, tìm thằng 1 viên
        if (targetId == -1)
        {
            foreach (var pair in trayTileCounts)
            {
                if (pair.Value == 1)
                {
                    int leftOnBoard = tileSpawner.CountTilesOnBoard(pair.Key);
                    Debug.Log($"<color=cyan>[Booster] Đang xét ID {pair.Key} (Ưu tiên 2). Trên bàn cờ tìm thấy: {leftOnBoard} viên.</color>");

                    if (leftOnBoard >= 2)
                    {
                        targetId = pair.Key;
                        amountNeeded = 2;
                        break;
                    }
                    else
                    {
                        Debug.Log($"<color=yellow>[Booster] ĐÃ BỎ QUA ID {pair.Key} vì trên bàn cờ không đủ 2 viên!</color>");
                    }
                }
            }
        }

        //Nếu khay trống hoặc không thể bù lỗ, chọn 1 ID bất kỳ trên bàn cờ có đủ 3 viên
        if (targetId == -1)
        {
            targetId = tileSpawner.FindRandomIdWithEnoughTiles(3);
            amountNeeded = 3;
        }

        //NẾU VẪN KHÔNG TÌM ĐƯỢC (Bàn cờ hết gạch hoặc không có bộ 3 nào) -> Huỷ dùng skill
        if (targetId == -1)
        {
            Debug.Log("Không có bộ gạch nào hợp lệ để dùng Booster!");
            return;
        }

        Debug.Log($"<color=green>[Booster] Đã chốt đơn! Target ID: {targetId} | Cần lấy thêm: {amountNeeded} viên.</color>");

        // 2. Thực thi hút gạch
        StartCoroutine(ExecuteBoosterPull(targetId, amountNeeded));
    }

    private IEnumerator ExecuteBoosterPull(int targetId, int amountNeeded)
    {
        List<TileController> pulledTiles = new List<TileController>();

        //Quét lần 1: Ưu tiên lấy các viên gạch đang không bị đè (haventUpperTiles == true)
        foreach (TileController tile in tileSpawner.tilesOnBoard)
        {
            if (tile.TileData.Id == targetId && tile.gameObject.activeSelf)
            {
                if (tile.TileData.HaventUpperTiles())
                {
                    pulledTiles.Add(tile);
                    if (pulledTiles.Count == amountNeeded)
                        break;
                }
            }
        }

        //Quét lần 2: Nếu thiếu, lấy luôn cả gạch bị đè (Booster xịn là phải bốc được gạch dưới đáy)
        if (pulledTiles.Count < amountNeeded)
        {
            foreach (TileController tile in tileSpawner.tilesOnBoard)
            {
                if (tile.TileData.Id == targetId && tile.gameObject.activeSelf)
                {
                    //Phải đảm bảo viên này chưa bị nhặt ở lần 1
                    if (!pulledTiles.Contains(tile))
                    {
                        pulledTiles.Add(tile);
                        if (pulledTiles.Count == amountNeeded)
                            break;
                    }
                }
            }
        }

        // LỚP BẢO VỆ CUỐI: Chắc chắn đã tóm đủ mục tiêu
        Debug.Log($"<color=cyan>[Booster - Bước 2] Đã tóm được {pulledTiles.Count}/{amountNeeded} viên gạch ID {targetId}. Bắt đầu hút!</color>");

        if (pulledTiles.Count == 0)
        {
            yield break; // Thoát Coroutine nếu có lỗi Data ảo
        }

        //Handle Pick Tile y hệt như khi người chơi click tay
        for (int i = 0; i < pulledTiles.Count; i++)
        {
            TileController tile = pulledTiles[i];

            tile.TileGraphic.SetColorWhite();
            HandlePickTile(tile);

            yield return new WaitForSeconds(0.15f); // Đợi 0.15s rồi mới hút viên tiếp theo để tạo cảm giác nối đuôi
        }
    }

    //Test Shuffle Tiles
    [Button]
    public void HandleShuffle()
    {
        StartCoroutine(IEShuffle());
    }

    private IEnumerator IEShuffle()
    {
        InputManager.Instance.enabled = false;

        bool canShuffle = false;

        for (int attempt = 0; attempt < 5; attempt++) // Thử shuffle tối đa 5 lần để tránh trường hợp hiếm gặp không có nước đi sau shuffle
        {
            List<TileController> activeTiles = GetActiveTiles();
            if (activeTiles.Count == 0) break; // Không có viên gạch nào trên bàn, thoát luôn

            PlayShuffleTween(activeTiles);
            yield return new WaitForSeconds(.5f); // Đợi tween kết thúc

            List<TileShuffleData> shuffleData = InitShuffleData(activeTiles);
            ShuffleData(shuffleData);
            ApplyShuffle(activeTiles, shuffleData);

            canShuffle = HasPlayableMatch();
            if (canShuffle) break; // Nếu đã có nước đi sau shuffle thì thôi, không cần thử lại
        }

        InputManager.Instance.enabled = true;
    }

    //1. Lấy danh sách các tile đang active trên bàn cờ
    private List<TileController> GetActiveTiles()
    {
        List<TileController> activeTiles = new List<TileController>();

        foreach (var tile in tileSpawner.tilesOnBoard)
        {
            if (tile.gameObject.activeSelf)
            {
                activeTiles.Add(tile);
            }
        }

        return activeTiles;
    }

    //2. Tween Shuffle
    private void PlayShuffleTween(List<TileController> activeTiles)
    {
        if (activeTiles.Count == 0) return;

        Vector3 min = activeTiles[0].transform.position;
        Vector3 max = min;

        for (int i = 1; i < activeTiles.Count; i++)
        {
            Vector3 pos = activeTiles[i].transform.position;
            min = Vector3.Min(min, pos);
            max = Vector3.Max(max, pos);
        }

        Vector3 center = (min + max) / 2f;

        foreach (var tile in activeTiles)
        {
            tile.MoveShuffle(center);
        }
    }

    //3. Lấy data của các active tile đó (ID + Sprite) vào 1 list tạm
    private List<TileShuffleData> InitShuffleData(List<TileController> activeTiles)
    {
        List<TileShuffleData> shuffleDatas = new List<TileShuffleData>(activeTiles.Count);

        foreach (var tile in activeTiles)
        {
            shuffleDatas.Add(new TileShuffleData(
                tile.TileData.Id,
                tile.TileGraphic.Icon.sprite
            ));
        }

        return shuffleDatas;
    }

    //4. Xáo trộn list data tạm đó
    private void ShuffleData(List<TileShuffleData> shuffleData)
    {
        for (int j = shuffleData.Count - 1; j > 0; j--)
        {
            int k = Random.Range(0, j + 1);
            var temp = shuffleData[j];
            shuffleData[j] = shuffleData[k];
            shuffleData[k] = temp;
        }
    }

    //5. Áp data đã xáo trộn vào lại các tile trên bàn cờ theo đúng thứ tự tile trong list active ban đầu (đảm bảo vị trí tile không đổi, chỉ đổi ID và Sprite)
    private void ApplyShuffle(List<TileController> activeTiles, List<TileShuffleData> shuffleData)
    {
        for (int i = 0; i < activeTiles.Count; i++)
        {
            activeTiles[i].SetupTileAfterShuffle(shuffleData[i].Id, shuffleData[i].Icon);
        }
    }

    //6. Kiểm tra nếu sau khi shuffle vẫn còn bộ 3 nào đó để chơi thì mới cho phép shuffle, không thì thôi (tránh trường hợp shuffle xong mà hết nước đi)
    private bool HasPlayableMatch()
    {
        Dictionary<int, int> trayCounts = TrayManager.Instance.TrayDomain.GetTrayTileFrequencies();
        Dictionary<int, int> boardCounts = tileSpawner.CountTilesEachId();

        foreach (var pair in boardCounts)
        {
            int trayCount = trayCounts.ContainsKey(pair.Key) ? trayCounts[pair.Key] : 0;

            if (trayCount + pair.Value >= 3)
            {
                return true; // Có ít nhất một ID có thể tạo thành bộ 3
            }
        }

        return false; // Không có ID nào có thể tạo thành bộ 3
    }

    //Test thử chỉ test duyệt trong clickableTiles xem có ID nào có thể kết hợp với khay để tạo thành
    //bộ 3 không, sẽ nhanh hơn nhiều so với duyệt toàn bộ bàn cờ (đặc biệt khi bàn cờ có nhiều gạch)
    //private bool HasPlayableMatch()
    //{
    //    Dictionary<int, int> trayCounts = TrayManager.Instance.TrayDomain.GetTrayTileFrequencies();
    //    Dictionary<int, int> clickableCounts = new Dictionary<int, int>();

    //    foreach (var tile in tileSpawner.clickableTiles)
    //    {
    //        if (tile == null || !tile.gameObject.activeSelf) continue;

    //        int id = tile.TileData.Id;
    //        if (clickableCounts.ContainsKey(id))
    //            clickableCounts[id]++;
    //        else
    //            clickableCounts[id] = 1;
    //    }

    //    foreach (var pair in clickableCounts)
    //    {
    //        int trayCount = trayCounts.ContainsKey(pair.Key) ? trayCounts[pair.Key] : 0;

    //        if (trayCount + pair.Value >= 3)
    //        {
    //            return true;
    //        }
    //    }

    //    return false;
    //}

    private struct TileShuffleData
    {
        public int Id { get; }
        public Sprite Icon { get; }

        public TileShuffleData(int id, Sprite icon)
        {
            Id = id;
            Icon = icon;
        }
    }
}
