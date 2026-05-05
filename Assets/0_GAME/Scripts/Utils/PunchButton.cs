using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using PrimeTween;

public class PunchButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("Press Settings")]
    [SerializeField] private float pressScaleFactor = 0.9f;
    [SerializeField] private Color pressColor = new Color(.75f, .75f, .75f, 1f);
    [SerializeField] private float pressDuration = 0.1f;

    [Header("Punch Settings")]
    [SerializeField] private Vector3 punchStrength = new Vector3(1f, 1f, 1f);
    [SerializeField] private float punchDuration = 0.1f;
    [SerializeField] private float punchFrequency = 0.5f; //Tần số nảy

    private Vector3 originalScale;
    private Color originalColor;
    private Graphic targetGraphic;

    private Tween scaleTween;
    private Tween colorTween;

    private void Awake()
    {
        originalScale = transform.localScale;
        targetGraphic = GetComponent<Graphic>();

        if (targetGraphic != null)
        {
            originalColor = targetGraphic.color;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        scaleTween.Stop();
        colorTween.Stop();

        //1. Thu nhỏ
        scaleTween = Tween.Scale(transform, originalScale * pressScaleFactor, pressDuration, Ease.OutQuad);

        //2. Đổi màu
        if (targetGraphic != null)
        {
            colorTween = Tween.Color(targetGraphic, pressColor, pressDuration, Ease.OutQuad);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        scaleTween.Stop();
        colorTween.Stop();

        //1. Phục hồi màu sắc
        if (targetGraphic != null)
        {
            colorTween = Tween.Color(targetGraphic, originalColor, pressDuration, Ease.OutQuad);
        }

        transform.localScale = originalScale; // Đảm bảo phục hồi về kích thước ban đầu trước khi punch

        //2. Punch
        scaleTween = Tween.PunchScale(transform, punchStrength, punchDuration, punchFrequency);
    }

    private void OnDisable()
    {
        scaleTween.Stop();
        colorTween.Stop();

        transform.localScale = originalScale;

        if (targetGraphic != null)
            targetGraphic.color = originalColor;
    }
}
