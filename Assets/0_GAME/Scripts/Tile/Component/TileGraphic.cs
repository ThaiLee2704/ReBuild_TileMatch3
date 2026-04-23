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
        this.icon.sprite = icon;

        this.bg.sortingOrder = orderLayer * 10;
        this.icon.sortingOrder = orderLayer * 10 + 1;
    }

    public void UpdateColor(bool haventUpperTiles)
    {
        icon.color = haventUpperTiles ? Color.white : Color.gray;
        bg.color = haventUpperTiles ? Color.white : Color.gray;
    }
}
