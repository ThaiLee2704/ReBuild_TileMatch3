using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileAnimation : MonoBehaviour
{
    private TileGraphic tileGraphic;
    [SerializeField] private int[] cacheOrder = new int[2];

    private Vector3 baseScale;
    private float originalLocalPositionY;

    private Tween zoomTween;
    private Tween initShakeTween;
    private Sequence shakeSequence;
    private Sequence moveSequence;

    private void Awake()
    {
        tileGraphic = GetComponentInChildren<TileGraphic>();

        baseScale = this.transform.localScale;
        originalLocalPositionY = this.transform.localPosition.y;
    }

    public void PlayHoldTileAnim()
    {
        StopTween();

        tileGraphic.BringUpSortingOrder();

        zoomTween = Tween.Scale(this.transform, baseScale * 1.3f, .1f, Ease.OutQuad);

        Vector3 rightAngle = new Vector3(0, 0, -8f);
        Vector3 leftAngle = new Vector3(0, 0, 8f);

        initShakeTween = Tween.LocalEulerAngles(transform, Vector3.zero, rightAngle, .5f, Ease.OutSine)
            .OnComplete(() =>
            {
                shakeSequence = Sequence.Create(-1)
                    .Chain(Tween.LocalEulerAngles(transform, rightAngle, leftAngle, 1f, Ease.Linear))
                    .Chain(Tween.LocalEulerAngles(transform, leftAngle, rightAngle, 1f, Ease.Linear));
            });

        moveSequence = Sequence.Create(-1)
            .Chain(Tween.LocalPositionY(this.transform, originalLocalPositionY + 0.5f, .5f, Ease.Linear))
            .Chain(Tween.LocalPositionY(this.transform, originalLocalPositionY, .5f, Ease.Linear));
    }

    public void StopHoldTileAnim()
    {
        StopTween();

        tileGraphic.BringDownSortingOrder();

        zoomTween = Tween.Scale(this.transform, baseScale, 1f, Ease.OutQuad);
        Tween.LocalRotation(this.transform, Quaternion.identity, .2f, Ease.OutQuad);
        Tween.LocalPositionY(this.transform, originalLocalPositionY, .2f, Ease.OutQuad);
    }

    private void StopTween()
    {
        zoomTween.Stop();
        initShakeTween.Stop();
        shakeSequence.Stop();
        moveSequence.Stop();
    }
}
