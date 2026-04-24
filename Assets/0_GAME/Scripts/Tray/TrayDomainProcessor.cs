using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrayDomainProcessor : MonoBehaviour
{
    public List<TileController> realTilesInTray = new List<TileController>(TRAY_SIZE);

    public static int TRAY_SIZE = 7;
    public bool IsFull => realTilesInTray.Count >= TRAY_SIZE;

    public void InsertRealTray(TileController newTile)
    {
        int insertRealIndex = realTilesInTray.Count;

        for (int i = realTilesInTray.Count - 1; i >= 0; i--)
        {
            if (realTilesInTray[i].TileData.Id == newTile.TileData.Id)
            {
                insertRealIndex = i + 1;
                break;
            }
        }

        realTilesInTray.Insert(insertRealIndex, newTile);

        HandleMatch3Domain(newTile);
    }

    private void HandleMatch3Domain(TileController newTile)
    {
        int realCountOfSameId = 0;

        foreach (var t in realTilesInTray)
        {
            if (t.TileData.Id == newTile.TileData.Id)
                realCountOfSameId++;
        }

        if (realCountOfSameId >= 3)
        {
            int count = 1;
            for (int i = 0; i < realTilesInTray.Count; i++)
            {
                if (realTilesInTray[i].TileData.Id == realTilesInTray[i + 1].TileData.Id)
                {
                    count++;

                    if (count == 3)
                    {
                        realTilesInTray.RemoveAt(i + 1);
                        realTilesInTray.RemoveAt(i);
                        realTilesInTray.RemoveAt(i - 1);
                        break;
                    }
                }
                else
                    count = 1;
            }
        }
        else
        {
            if (IsFull)
            {
                StartCoroutine(IEGameOver(newTile));
            }
        }
    }

    private IEnumerator IEGameOver(TileController newTile)
    {
        yield return null; //Đợi 1 frame để đảm bảo tile gọi được MoveToSlot
        yield return new WaitForSeconds(newTile.moveDuration);
        Debug.Log("GAME OVER");
    }
}