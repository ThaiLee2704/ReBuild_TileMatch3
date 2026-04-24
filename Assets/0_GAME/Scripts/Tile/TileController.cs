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
    public float bouceDuration = 1f;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();

        if (TileData == null)
            TileData = GetComponent<TileData>();
        if (TileGraphic == null)
            TileGraphic = GetComponentInChildren<TileGraphic>();
    }

    public void SetUp(int id, int orderLayer, Sprite icon)
    {
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
        moveTween = Tween.PositionAtSpeed(transform, slot.position, moveSpeed, Ease.Linear)
            .OnComplete( () => Tween.Scale(transform, 0.8f, bouceDuration, Ease.OutBounce));
    }

    public void Slide(Transform slot)
    {
        moveTween.Stop();
        moveDuration = Vector3.Distance(transform.position, slot.position) / moveSpeed;
        moveTween = Tween.PositionAtSpeed(transform, slot.position, moveSpeed, Ease.Linear)
            .OnComplete(() => Tween.Scale(transform, 0.8f, bouceDuration, Ease.OutBounce));
    }

    public void UpdateTileData()
    {
        foreach (var lowerTile in TileData.LowerTiles)
        {
            lowerTile.TileData.RemoveUpperTile(this);
            lowerTile.UpdateState();
        }

        TileData.ClearLowerTiles();
    }
}
