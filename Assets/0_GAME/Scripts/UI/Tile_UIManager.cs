using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile_UIManager : Tile_Singleton<Tile_UIManager>
{
    [Header("Panels")]
    public MainMenuPanel MainMenuPanel;

    [Header("Popups")]
    public WinPopup WinPopup;
    public LosePopup LosePopup;
    public SettingPopup SettingPopup;

    [Header("GUI")]
    [SerializeField] private Button settingBtn;

    [Header("Booster Buttons")]
    [SerializeField] private Button undoBtn;
    [SerializeField] private Button magnetBtn;
    [SerializeField] private Button shuffleBtn;

    protected override void Awake()
    {
        base.Awake();

        settingBtn.onClick.AddListener(OnSettingBtnClicked);

        undoBtn.onClick.AddListener(OnUndoBtnClicked);
        magnetBtn.onClick.AddListener(OnMagnetBtnClicked);
        shuffleBtn.onClick.AddListener(OnShuffleBtnClicked);
    }

    private void OnSettingBtnClicked()
    {
        SettingPopup.Show();
    }

    private void OnUndoBtnClicked()
    {
        Tile_GameManager.Instance.HandleUndoTile();
    }

    private void OnMagnetBtnClicked()
    {
        Tile_GameManager.Instance.HandleAutoMatch3();
    }

    private void OnShuffleBtnClicked()
    {
        //Tile_GameManager.Instance.HandleShuffle();
    }
}
