using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrayVisualProcessor : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float offsetDelayMatch3 = 0.2f;

    public List<Transform> slots = new List<Transform>();
    public List<TileController> visualTilesInTray = new List<TileController>();
    public List<TileController> undoHistory = new List<TileController>();

    private int insertIndex;
    float maxMoveDuration;
    private Queue<IEnumerator> match3Queue = new Queue<IEnumerator>();

    private void Awake()
    {
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


    public void InsertVisualTray(TileController newTile)
    {
        insertIndex = visualTilesInTray.Count;

        for (int i = visualTilesInTray.Count - 1; i >= 0; i--)
        {
            if (visualTilesInTray[i].TileData.Id == newTile.TileData.Id)
            {
                insertIndex = i + 1;
                break;
            }
        }

        visualTilesInTray.Insert(insertIndex, newTile);
        undoHistory.Add(newTile);
    }

    public Transform GetSlot()
    {
        return slots[insertIndex];
    }

    public void SlideTilesAfterTileInserted()
    {
        for (int i = insertIndex + 1; i < visualTilesInTray.Count; i++)
        {
            visualTilesInTray[i].Slide(slots[i], 0);
        }
    }

    public void CheckMatch3(TileController newTile)
    {
        int countOfSameId = 0;
        //maxMoveDuration = 0;
        foreach (var t in visualTilesInTray)
        {
            if (t.TileData.Id == newTile.TileData.Id)
            {
                maxMoveDuration = t.moveDuration > maxMoveDuration ? t.moveDuration : maxMoveDuration;
                countOfSameId++;
            }
        }

        if (countOfSameId >= 3)
        {
            //Match3 Handle
            match3Queue.Enqueue(IEMatch3(newTile, maxMoveDuration));
        }
    }

    private IEnumerator IEMatch3(TileController newTile, float maxMoveDuration)
    {
        yield return null; //Đợi 1 frame để đảm bảo tile gọi được MoveToSlot
        yield return null; //Đợi thêm 1 frame để Tile thứ 4 cùng ID chạy MoveToSlot chèn sau cái Tile thứ 3 cùng ID trước khi Tile thứ 3 này Match3
        yield return new WaitForSeconds(maxMoveDuration/*newTile.moveDuration*/ + offsetDelayMatch3); //Đợi cho đến khi tile hoàn thành việc di chuyển
        //yield return new WaitForSeconds(0.2f);
        EatMatch3();
        CheckWin();
    }

    private void EatMatch3()
    {
        int count = 1;

        for (int i = 0; i < visualTilesInTray.Count - 1; i++)
        {
            if (visualTilesInTray[i].TileData.Id == visualTilesInTray[i + 1].TileData.Id)
            {
                count++;

                if (count == 3)
                {
                    visualTilesInTray[i - 1].TileGraphic.FadeOut(0);
                    visualTilesInTray[i].TileGraphic.FadeOut(.0375f);
                    visualTilesInTray[i + 1].TileGraphic.FadeOut(.075f);

                    undoHistory.Remove(visualTilesInTray[i - 1]);
                    undoHistory.Remove(visualTilesInTray[i]);
                    undoHistory.Remove(visualTilesInTray[i + 1]);

                    visualTilesInTray.RemoveAt(i + 1);
                    visualTilesInTray.RemoveAt(i);
                    visualTilesInTray.RemoveAt(i - 1);

                    for (int j = i - 1; j < visualTilesInTray.Count; j++)
                        visualTilesInTray[j].Slide(slots[j], 0.2f);

                    break;
                }
            }
            else
                count = 1;
        }
    }

    private void CheckWin()
    {
        if (visualTilesInTray.Count == 0 && undoHistory.Count == 0 && match3Queue.Count == 0 && TileSpawner.Instance.tilesOnBoard.Count == 0)
        {
            Debug.Log("Win");
        }
    }

    public void ClearVisualTray()
    {
        for (int i = 0; i < visualTilesInTray.Count; i++)
        {
            if (visualTilesInTray[i] != null)
                visualTilesInTray[i].gameObject.SetActive(false);
        }

        visualTilesInTray.Clear();
        undoHistory.Clear();
    }
}
