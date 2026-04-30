using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LosePopup : Puzzle_BasePopup
{
    [Header("Buttons")]
    [SerializeField] private Button replayBtn;

    protected override void Awake()
    {
        base.Awake();
        replayBtn.onClick.AddListener(OnReplayBtnClicked);
    }

    private void OnReplayBtnClicked()
    {
        LevelManager.Instance.ReplayLevel();
        Hide();
    }
}
