using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPopup : Puzzle_BasePopup
{
    [Header("Buttons")]
    [SerializeField] private Button homeBtn;
    [SerializeField] private Button replayBtn;

    protected override void Awake()
    {
        base.Awake();
        homeBtn.onClick.AddListener(OnHomeBtnClicked);
        replayBtn.onClick.AddListener(OnReplayBtnClicked);
    }

    private void OnHomeBtnClicked()
    {
        LevelManager.Instance.ClearOldLevelData();
        Hide();
        Tile_UIManager.Instance.MainMenuPanel.Show();
    }

    private void OnReplayBtnClicked()
    {
        LevelManager.Instance.ReplayLevel();
        Hide();
    }
}
