using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TrayManager : Tile_Singleton<TrayManager>
{
    [Header("References")]
    public TrayDomainProcessor TrayDomain;

    public List<Transform> slots = new List<Transform>();
    public List<TileController> tilesInTray = new List<TileController>();

    private Queue<IEnumerator> match3Queue = new Queue<IEnumerator>();

    private static int TRAY_SIZE = TrayDomainProcessor.TRAY_SIZE;

    protected override void Awake()
    {
        base.Awake();
        TrayDomain = GetComponent<TrayDomainProcessor>();

        StartCoroutine(ProcessMatch3Queue());
    }

    private IEnumerator ProcessMatch3Queue()
    {
        while (true)
        {
            if (match3Queue.Count > 0)
                yield return StartCoroutine(match3Queue.Dequeue());
            else
                yield return null;
        }
    }

    public Transform TryGetSlot(TileController newTile)
    {
        if (tilesInTray.Count >= TRAY_SIZE)
        {
            if (TrayDomain.IsFull)
                return null;
        }

        int insertIndex = tilesInTray.Count;

        for (int i = tilesInTray.Count - 1; i >= 0; i--)
        {
            if (tilesInTray[i].TileData.Id == newTile.TileData.Id)
            {
                insertIndex = i + 1;
                break;
            }
        }

        tilesInTray.Insert(insertIndex, newTile);

        for (int i = insertIndex + 1; i < tilesInTray.Count; i++)
        {
            tilesInTray[i].Slide(slots[i]);
        }

        int countOfSameId = 0;
        foreach (var t in tilesInTray)
        {
            if (t.TileData.Id == newTile.TileData.Id)
                countOfSameId++;
        }

        if (countOfSameId >= 3)
        {
            //Match3 Handle
            match3Queue.Enqueue(IEMatch3(newTile));
        }

        return slots[insertIndex];
    }

    private IEnumerator IEMatch3(TileController newTile)
    {
        yield return null; //Đợi 1 frame để đảm bảo tile gọi được MoveToSlot
        yield return null; //Đợi thêm 1 frame để Tile thứ 4 cùng ID chạy MoveToSlot chèn sau cái Tile thứ 3 cùng ID trước khi Tile thứ 3 này Match3
        yield return new WaitForSeconds(newTile.moveDuration); //Đợi cho đến khi tile hoàn thành việc di chuyển
        EatMatch3();
    }

    private void EatMatch3()
    {
        int count = 1;

        for (int i = 0; i < tilesInTray.Count - 1; i++)
        {
            if (tilesInTray[i].TileData.Id == tilesInTray[i + 1].TileData.Id)
            {
                count++;

                if (count == 3)
                {
                    //Remove
                    tilesInTray[i - 1].gameObject.SetActive(false);
                    tilesInTray[i].gameObject.SetActive(false);
                    tilesInTray[i + 1].gameObject.SetActive(false);

                    tilesInTray.RemoveAt(i + 1);
                    tilesInTray.RemoveAt(i);
                    tilesInTray.RemoveAt(i - 1);

                    //Slide
                    for (int j = i - 1; j < tilesInTray.Count; j++)
                    {
                        tilesInTray[j].Slide(slots[j]);
                    }

                    break;
                }
            }
            else
                count = 1;
        }
    }
}
