using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinPopup : Puzzle_BasePopup
{
    [Header("Buttons")]
    [SerializeField] private Button nextLevelBtn;

    protected override void Awake()
    {
        base.Awake();
        nextLevelBtn.onClick.AddListener(OnNextLevelBtnClicked);
    }

    private void OnNextLevelBtnClicked()
    {
        LevelManager.Instance.NextLevel();
        Hide();
    }
}
