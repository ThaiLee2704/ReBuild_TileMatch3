using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MMSettingsPopup : Puzzle_BasePopup
{
    [Header("Buttons")]
    [SerializeField] private Button musicBtn;
    [SerializeField] private Button soundBtn;
    [SerializeField] private Button vibrationBtn;
    [SerializeField] private Button languageBtn;

    [SerializeField] private Sprite onSprite;
    [SerializeField] private Sprite offSprite;

    private bool isMusicOn = true;
    private bool isSoundOn = true;
    private bool isVibrationOn = true;

    protected override void Awake()
    {
        base.Awake();

        musicBtn.onClick.AddListener(OnMusicBtnClicked);
        soundBtn.onClick.AddListener(OnSoundBtnClicked);
        vibrationBtn.onClick.AddListener(OnVibrationBtnClicked);
        languageBtn.onClick.AddListener(OnLanguageBtnClicked);
    }

    private void OnMusicBtnClicked()
    {
        isMusicOn = !isMusicOn;
        musicBtn.image.sprite = isMusicOn ? onSprite : offSprite;
    }

    private void OnSoundBtnClicked()
    {
        isSoundOn = !isSoundOn;
        soundBtn.image.sprite = isSoundOn ? onSprite : offSprite;
    }

    private void OnVibrationBtnClicked()
    {
        isVibrationOn = !isVibrationOn;
        vibrationBtn.image.sprite = isVibrationOn ? onSprite : offSprite;
    }

    private void OnLanguageBtnClicked()
    {
        
    }
}
