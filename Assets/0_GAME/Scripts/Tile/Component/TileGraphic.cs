using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGraphic : MonoBehaviour
{
    [SerializeField] SpriteRenderer bg;
    [SerializeField] SpriteRenderer icon;
    public SpriteRenderer Bg => bg;
    public SpriteRenderer Icon => icon;

    public void SetUpGraphic(Sprite icon, int orderLayer)
    {
        ResetGraphic();

        this.icon.sprite = icon;

        this.bg.sortingOrder = orderLayer * 10;
        this.icon.sortingOrder = orderLayer * 10 + 1;
    }

    public void UpdateColor(bool haventUpperTiles)
    {
        icon.color = haventUpperTiles ? Color.white : Color.gray;
        bg.color = haventUpperTiles ? Color.white : Color.gray;
    }

    public void FadeOut(float delay = 0)
    {
        BringUpSortingOrder();

        TileController tile = this.GetComponentInParent<TileController>();

        Tween.Alpha(icon, 0f, 0.5f, Ease.Linear, startDelay: delay);

        Tween.Alpha(bg, 0f, 0.5f, Ease.Linear, startDelay: delay)
            .OnComplete(tile, targetTile =>
            {
                targetTile.gameObject.SetActive(false);
                //TileSpawner.Instance.ClearTile(targetTile);
            });
    }

    public void BringUpSortingOrder()
    {
        icon.sortingOrder += 1000;
        bg.sortingOrder += 1000;
    }

    public void BringDownSortingOrder()
    {
        icon.sortingOrder -= 1000;
        bg.sortingOrder -= 1000;
    }

    public void SetColorWhite()
    {
        icon.color = Color.white;
        bg.color = Color.white;
    }

    public void ResetGraphic()
    {
        SetColorWhite();
        BringDownSortingOrder();
    }
}
