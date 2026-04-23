#if DOTWEEN
using DG.Tweening;
#elif PRIME_TWEEN
using PrimeTween;
#endif
using UnityEngine;

public class BasePopup : MonoBehaviour
{
    [SerializeField] protected CanvasGroup main;

    [SerializeField] private ButtonEffectLogic btnClose;

    public bool isShow;
    
    protected virtual void Awake()
    {
        if(btnClose != null) btnClose.onClick.AddListener(Hide);
    }

    public virtual void InitialiseUI()
    {

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
#elif PRIME_TWEEN
        Tween.Alpha(main, 0, 1f, .5f);
#endif
        ButtonCloseEffect();
    }

    public virtual void Hide()
    {
#if DOTWEEN
        main.DOFade(0f, .5f).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
#elif PRIME_TWEEN 
        Tween.Alpha(main, 0f, .5f).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
#endif
        isShow = false;
    }
    
    protected void ButtonCloseEffect()
    {
        if(btnClose == null) return;
        btnClose.transform.localScale = Vector3.zero;
#if DOTWEEN
        DOVirtual.DelayedCall(1.75f, () =>
        {
            if (isShow) btnClose.transform.DOScale(1f, .7f);
        });
#elif PRIME_TWEEN 
	    Tween.Delay(0.5f, () =>
	    {
		    if (isShow) Tween.Scale(btnClose.transform, 1f, .7f);
	    });
#endif
    }

    protected virtual void OnDisable()
    {
#if DOTWEEN
        btnClose.transform.DOKill();
        main.DOKill();
#elif PRIME_TWEEN
        if (btnClose != null)
            Tween.StopAll(btnClose.transform);
        if (main != null)
            Tween.StopAll(main);
#endif
    }
    
    #if UNITY_EDITOR

    [Button]
    public virtual void BASE_POPUP_INIT()
    {
        main = Utils.FindInChildren(gameObject, "Main").GetComponent<CanvasGroup>();
        btnClose = Utils.FindInChildren(gameObject, "ButtonClose").GetComponent<ButtonEffectLogic>();
        
        this.SetDirty();
    }
    
    #endif
}
