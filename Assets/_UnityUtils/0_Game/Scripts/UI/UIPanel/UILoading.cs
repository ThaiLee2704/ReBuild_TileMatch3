using System;
using UnityEngine;
using UnityEngine.UI;
public class UILoading : MonoBehaviour
{
    [Header("LOADING_PLAY")]
    [SerializeField] private Image bgPlay;
    [SerializeField] private Image process;
    [SerializeField] private Sprite[] spriteBg;

    [Header("LOADING_PANEL_FADE")]
    [SerializeField] private Image loadingFadeImage;
    [SerializeField] private float loadingHalfDuration;
#if UNITY_EDITOR

    private float _timeLoadingGame = 1;
#else
    private float _timeLoadingGame = 3;
#endif

    private bool _isLoadingPlayGame;
    private bool _isLoading;


    private bool on_off_banner_in_loading;

    private void Awake()
    {
        on_off_banner_in_loading = PrefData.on_off_banner_in_loading;
    }


    public void CallLoadingPlayGame(int indexGame, Action actionHide, bool autoHide = true)
    {
#if ADS_AVAILABLE
        AdsAdapter.Instance.ShowMrec();
#endif
        if (!on_off_banner_in_loading)
        {
#if ADS_AVAILABLE
            AdsAdapter.Instance.HideBanner();
#endif
        }
        if (_isLoadingPlayGame) return;
        _isLoadingPlayGame = true;
        bgPlay.gameObject.SetActive(true);

    }

    public void CallLoadingShowFade(Action action, bool autoHide = true)
    {
        if (_isLoading) return;
        _isLoading = true;
        loadingFadeImage.gameObject.SetActive(true);
    }

    public void CallLoadingPlayGHide()
    {
        bgPlay.gameObject.SetActive(false);
        Hide();
#if ADS_AVAILABLE
	    AdsAdapter.Instance.HideMrec();
#endif
    }

    public void CallLoadingFadeHide()
    {

    }

    private void Hide()
    {
        _isLoadingPlayGame = false;
        _isLoading = false;
    }

}
