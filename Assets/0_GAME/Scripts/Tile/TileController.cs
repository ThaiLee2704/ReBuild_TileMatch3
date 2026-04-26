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

    [Header("Tile info")]
    private Tween moveTween;
    [SerializeField] private float moveSpeed = 5f;
    public float moveDuration { get; private set; } = 0f;
    public float slideDuration { get; private set; } = 0f;
    public float bouceDuration = 1f;
    public Vector3 OriginalPos { get; private set; }

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

        moveTween = Tween.PositionAtSpeed(transform, slot.position, moveSpeed, Ease.Linear)
            .OnComplete(() => Tween.Scale(transform, 0.8f, bouceDuration, Ease.OutBounce));
    }

    public void Slide(Transform slot, float delay)
    {
        moveTween.Stop();

        slideDuration = Vector3.Distance(transform.position, slot.position) / moveSpeed;

        moveTween = Tween.PositionAtSpeed(transform, slot.position, moveSpeed, Ease.Linear, startDelay: delay)
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

    private void ResetScale()
    {
        this.transform.localScale = Vector3.one;
    }
}
