using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanel : Puzzle_BasePanel
{
    [Header("Buttons")]
    [SerializeField] private Button PlayBtn;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI mainMenuLevelTxt;

    private void Awake()
    {
        PlayBtn.onClick.AddListener(OnPlayBtnClicked);
    }

    [Button]
    private void OnPlayBtnClicked()
    {
        GAME_EVENTS.OnPlayGameBtnClicked?.Invoke();

        Hide();
    }
}
