using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    private BoxCollider2D boxCollider;

    [Header("Reference")]
    public TileData TileData;
    public TileGraphic TileGraphic;

    [Header("Tile move settings")]
    private Tween moveTween;
    [SerializeField] private float moveSpeed = 5f;
    public float moveDuration { get; private set; } = 0f;
    public float slideDuration { get; private set; } = 0f;
    public float bouceDuration = 1f;
    public Vector3 OriginalPos { get; private set; }

    [Header("Shuffle settings")]
    public float ShuffleMoveDuration;
    private Tween shuffleTween;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();

        if (TileData == null)
            TileData = GetComponent<TileData>();
        if (TileGraphic == null)
            TileGraphic = GetComponentInChildren<TileGraphic>();
    }

    public void SetUp(int id, int orderLayer, Sprite icon, Vector3 originalPos)
    {
        ResetScale();

        OriginalPos = originalPos;

        TileData.SetUpData(id, orderLayer);
        TileGraphic.SetUpGraphic(icon, orderLayer);
    }

    public void UpdateState()
    {
        bool haventUpperTiles = TileData.HaventUpperTiles();

        TileGraphic.UpdateColor(haventUpperTiles);

        if (haventUpperTiles)
            boxCollider.enabled = true;
        else
            boxCollider.enabled = false;

        TileSpawner.Instance.UpdateClickableTile(this, haventUpperTiles);
    }

    public void MoveToSlot(Transform slot)
    {
        moveDuration = Vector3.Distance(transform.position, slot.position) / moveSpeed;

        TileGraphic.BringUpSortingOrder();

        moveTween = Tween.PositionAtSpeed(transform, slot.position, moveSpeed, Ease.OutQuad)
            .OnComplete(() => Tween.Scale(transform, 0.8f, bouceDuration, Ease.OutBounce));
    }

    public void Slide(Transform slot, float delay)
    {
        moveTween.Stop();

        slideDuration = Vector3.Distance(transform.position, slot.position) / moveSpeed;
        float slideSpeed = 15f;
        moveTween = Tween.PositionAtSpeed(transform, slot.position, slideSpeed, Ease.Linear, startDelay: delay)
            .OnComplete(() => Tween.Scale(transform, 0.8f, bouceDuration, Ease.OutBounce));
    }

    public void UpdateTileData()
    {
        foreach (var lowerTile in TileData.LowerTiles)
        {
            lowerTile.TileData.RemoveUpperTile(this);
            lowerTile.UpdateState();
        }

        //TileData.ClearLowerTiles();
    }

    public void MoveBackUndo()
    {
        moveTween.Stop();

        moveTween = Tween.LocalPositionAtSpeed(transform, OriginalPos, moveSpeed, Ease.Linear)
            .OnComplete(() => Tween.Scale(transform, 1f, bouceDuration, Ease.OutBounce));
    }

    public void UndoTileData()
    {
        foreach (var lowerTile in TileData.LowerTiles)
        {
            lowerTile.TileData.UpperTiles.Add(this);
            lowerTile.UpdateState();
        }

        TileGraphic.BringDownSortingOrder();
        this.UpdateState();
    }

    //Test shuffle tile
    public void StopMoveTween()
    {
        moveTween.Stop();
    }

    public void SetupTileAfterShuffle(int newId, Sprite newIcon)
    {
        TileData.SetId(newId);

        TileGraphic.Icon.sprite = newIcon;
        TileGraphic.Bg.sortingOrder = TileData.OrderLayer * 10;
        TileGraphic.Icon.sortingOrder = TileData.OrderLayer * 10 + 1;

        UpdateState();
    }

    public void MoveShuffle(Vector3 center)
    {
        moveTween.Stop();
        shuffleTween.Stop();

        //Tween.Scale(tile.transform, 0.9f, 0.1f, Ease.InOutQuad)
        //    .OnComplete(tile.transform, t => Tween.Scale(t, 1f, 0.1f, Ease.OutQuad));

        Vector3 startPos = transform.position;
        Vector3 centerPos = new Vector3(center.x, center.y, startPos.z);

        Sequence shuffleSequence = Sequence.Create()
            .Chain(Tween.Position(transform, centerPos, ShuffleMoveDuration / 2f, Ease.InOutQuad))
            .Group(Tween.Alpha(TileGraphic.Icon, 0f, ShuffleMoveDuration / 2f, Ease.Linear))
            .Group(Tween.Alpha(TileGraphic.Bg, 0f, ShuffleMoveDuration / 2f, Ease.Linear))
            .Chain(Tween.Position(transform, startPos, ShuffleMoveDuration / 2f, Ease.InOutQuad))
            .Group(Tween.Alpha(TileGraphic.Icon, 1f, ShuffleMoveDuration / 2f, Ease.Linear))
            .Group(Tween.Alpha(TileGraphic.Bg, 1f, ShuffleMoveDuration / 2f, Ease.Linear));
    }

    private void ResetScale()
    {
        this.transform.localScale = Vector3.one;
    }
}
