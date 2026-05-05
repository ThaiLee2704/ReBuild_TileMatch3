using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanel : Puzzle_BasePanel
{
    [Header("Buttons")]
    [SerializeField] private Button playBtn;
    [SerializeField] private Button settingBtn;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI mainMenuLevelTxt;

    [Header("Popups")]
    [SerializeField] private MMSettingsPopup mmSettingsPopup;

    private void Awake()
    {
        playBtn.onClick.AddListener(OnPlayBtnClicked);
        settingBtn.onClick.AddListener(OnSettingBtnClicked);
    }

    [Button]
    private void OnPlayBtnClicked()
    {
        GAME_EVENTS.OnPlayGameBtnClicked?.Invoke();

        Hide();
    }

    private void OnSettingBtnClicked()
    {
        mmSettingsPopup.Show();
    }

    private void UpdateMainMenuLevelTxt()
    {
        //mainMenuLevelTxt.text = $"Level + {}";
    }
}
