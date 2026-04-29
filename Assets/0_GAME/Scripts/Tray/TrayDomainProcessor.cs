using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrayDomainProcessor : MonoBehaviour
{
    public static int TRAY_SIZE = 7;
    public List<TileController> realTilesInTray = new List<TileController>(TRAY_SIZE);

    public bool IsFull => realTilesInTray.Count >= TRAY_SIZE;

    //private int theLastIdMatch3;
    //private int theLastIndex;
    private int insertRealIndex;

    public void InsertDomainTray(TileController newTile)
    {
        //if (theLastIdMatch3 == newTile.TileData.Id)
        //{
        //    realTilesInTray.Insert( theLastIndex, newTile);
        //    return;
        //}

        insertRealIndex = realTilesInTray.Count;

        for (int i = realTilesInTray.Count - 1; i >= 0; i--)
        {
            if (realTilesInTray[i].TileData.Id == newTile.TileData.Id)
            {
                insertRealIndex = i + 1;
                break;
            }
        }

        realTilesInTray.Insert(insertRealIndex, newTile);
    }

    public Transform GetRealSlot()
    {
        return TrayManager.Instance.TrayVisual.slots[insertRealIndex];
    }

    public void CheckMatch3(TileController newTile)
    {
        int countOfSameId = 0;
        foreach (var t in realTilesInTray)
        {
            if (t.TileData.Id == newTile.TileData.Id)
                countOfSameId++;
        }

        if (countOfSameId >= 3)
        {
            EatMatch3(newTile);
        }
        else if (IsFull)
        {
            StartCoroutine(IEGameOver(newTile));
        }
    }

    private IEnumerator IEGameOver(TileController newTile)
    {
        yield return null; //Đợi 1 frame để đảm bảo tile gọi được MoveToSlot
        yield return new WaitForSeconds(newTile.moveDuration + newTile.slideDuration);
        Debug.Log("GAME OVER");
    }

    private void EatMatch3(TileController newTile)
    {
        int targerId = newTile.TileData.Id;
        int count = 0;

        for (int i = 0; i < realTilesInTray.Count; i++)
        {
            if (realTilesInTray[i].TileData.Id == targerId)
                count++;
        }

        if (count >= 3)
        {
            int removedCount = 0;
            for (int i = realTilesInTray.Count - 1; i >= 0; i--)
            {
                if (realTilesInTray[i].TileData.Id == targerId)
                {
                    realTilesInTray.RemoveAt(i);
                    removedCount++;

                    //theLastIndex = i;

                    if (removedCount == 3)
                        break;
                }
            }
        }

        //theLastIdMatch3 = targerId;
    }

    public void ClearDomainTray()
    {
        //theLastIdMatch3 = -1;
        //theLastIndex = -1;
        realTilesInTray.Clear();
    }

    //Test Auto Match3 V1
    public Dictionary<int, int> GetTrayTileFrequencies()
    {
        Dictionary<int, int> frequencies = new Dictionary<int, int>();

        // Giả sử domainList của bạn là List<TileController> hoặc List chứa Data
        // Hãy sửa lại tên biến domainList cho đúng với code thực tế của bạn nhé
        foreach (var tile in realTilesInTray)
        {
            int id = tile.TileData.Id;

            if (frequencies.ContainsKey(id))
            {
                frequencies[id]++; // Đã có trong từ điển thì cộng thêm 1
            }
            else
            {
                frequencies[id] = 1; // Chưa có thì tạo mới với số lượng là 1
            }
        }

        return frequencies;
    }
}