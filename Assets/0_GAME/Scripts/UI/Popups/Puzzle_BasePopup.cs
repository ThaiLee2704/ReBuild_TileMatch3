#if DOTWEEN
using DG.Tweening;
#elif PRIME_TWEEN
using PrimeTween;
#endif
using UnityEngine;
using UnityEngine.UI;

public class Puzzle_BasePopup : MonoBehaviour
{
    [Header("Base Popup")]
    [SerializeField] protected CanvasGroup main;
    [SerializeField] private Button closeBtn;

    public bool isShow;

    protected virtual void Awake()
    {
        if(closeBtn != null) 
            closeBtn.onClick.AddListener(Hide);
    }

    protected virtual void Start()
    {

    }

    public virtual void Show()
    {
        isShow = true;
        gameObject.SetActive(true);
#if DOTWEEN
        main.DOFade(1f, .5f).From(0);
        main.DOScale(1f, .5f).From(.3f).SetEase(Ease.OutBack);
#elif PRIME_TWEEN
        Tween.Alpha(main, 0, 1f, .5f);
        Tween.Scale(main.transform, Vector3.one * .3f, Vector3.one, .5f, Ease.OutBack);
#endif
    }

    public virtual void Hide()
    {
#if DOTWEEN
        main.DOScale(.3f, .5f).SetEase(Ease.InBack);
        main.DOFade(0f, .5f).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
#elif PRIME_TWEEN 
        Tween.Scale(main.transform, Vector3.one, Vector3.one * .3f, .5f, Ease.InBack);
        Tween.Alpha(main, 0f, .5f).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
#endif
        isShow = false;
    }
}
