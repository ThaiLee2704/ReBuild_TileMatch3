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

    //// Kích thước ô lưới ảo phân chia khu vực đo đạc
    //private const float cellSize = 1f;

    //// Kích thước ô lưới ảo phân chia khu vực đo đạc
    //public static void CaculateOverlaps(List<TileController> tilesSpawn)
    //{
    //    //Tạo 1 Dict chứa vị trí Tile đó trong ô lưới ảo và danh sách các tile bị tile này đè lên và cả nó.
    //    Dictionary<Vector2Int, List<TileController>> spatialGrid = new Dictionary<Vector2Int, List<TileController>>();

    //    // Bước 1: Đưa từng tile vào list cell của nó 
    //    //Khảo sát địa hình và ghi danh từng viên gạch vào sổ địa chỉ lưới không gian
    //    foreach (TileController tile in tilesSpawn)
    //    {
    //        Vector2Int cellPos = GetCellPosition(tile.transform.position);

    //        if (!spatialGrid.ContainsKey(cellPos))
    //        {
    //            spatialGrid[cellPos] = new List<TileController>();
    //        }
    //        // Thêm chính tile vào danh sách này cái đã, sau đó quét lấp thì mới thêm các tile bị đè sau
    //        spatialGrid[cellPos].Add(tile);
    //    }

    //    // Bước 2: Quét danh sách 9 ô lưới ảo xung quanh tile đó để tìm các tile có thể bị đè lên
    //    foreach (TileController tile in tilesSpawn)
    //    {
    //        Vector2Int centerCell = GetCellPosition(tile.transform.position);

    //        for (int x = -1; x <= 1; x++)
    //        {
    //            for (int y = -1; y <= 1; y++)
    //            {
    //                Vector2Int checkCell = new Vector2Int(centerCell.x + x, centerCell.y + y);

    //                //Nếu cell đó có tile → lấy danh sách tile trong cell đó
    //                if (spatialGrid.TryGetValue(checkCell, out List<TileController> neighborTiles))
    //                {
    //                    CheckCollisonAABB(tile, neighborTiles);
    //                }
    //            }
    //        }
    //    }

    //    // Bước 3: Đánh thức toàn bộ gạch sau khi đã xác định xong quan hệ trên dưới
    //    UpdateStateAllTiles(tilesSpawn);
    //}

    //public static void UpdateStateAllTiles(List<TileController> tilesSpawn)
    //{
    //    foreach (TileController tile in tilesSpawn)
    //    {
    //        tile.UpdateState();
    //    }
    //}

    //// Thuật toán kiểm tra va chạm hình hộp giới hạn AABB
    //private static void CheckCollisonAABB(TileController currentTile, List<TileController> neighborTiles)
    //{
    //    foreach (TileController neighbor in neighborTiles)
    //    {
    //        // Lọc bỏ chính nó hoặc các viên gạch nằm ở lớp thấp hơn hoặc bằng
    //        if (currentTile == neighbor || currentTile.tileData.OrderLayer >= neighbor.tileData.OrderLayer) continue;

    //        float distanceX = Mathf.Abs(currentTile.transform.position.x - neighbor.transform.position.x);
    //        float distanceY = Mathf.Abs(currentTile.transform.position.y - neighbor.transform.position.y);

    //        if (distanceX < 2f && distanceY < 2f)
    //        {
    //            currentTile.tileData.UpperTiles.Add(neighbor);
    //            neighbor.tileData.LowerTiles.Add(currentTile);
    //        }
    //    }
    //}

    //// Hàm toán học chuyển đổi tọa độ thực tế thành tọa độ ô lưới ảo
    //private static Vector2Int GetCellPosition(Vector3 pos)
    //{
    //    return new Vector2Int(Mathf.FloorToInt(pos.x / cellSize), Mathf.FloorToInt(pos.y / cellSize));
    //}
}