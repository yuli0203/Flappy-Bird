using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class HudViewModel : MonoBehaviour
{
    [SerializeField] LabelButtonViewModel labelButton;
    [SerializeField] LabelButtonViewModel labelScoreButton;
    [SerializeField] Button tutorialButton;
    [SerializeField] Button gameOverButton;

    public event Action OnTutorialClick;
    public event Action OnGameOverClick;

    public void Awake()
    {
        tutorialButton.onClick.AddListener(() => OnTutorialClick?.Invoke());
        gameOverButton.onClick.AddListener(() => OnGameOverClick?.Invoke());
    }

    public void SetTutorial(bool enabled)
    {
        if (tutorialButton != null)
        {
            tutorialButton.gameObject?.SetActive(enabled);
        }
    }
    public void SetGameOver(bool enabled)
    {
        if (gameOverButton != null)
        {
            gameOverButton.gameObject?.SetActive(enabled);
        }
    }

    public void SetHighScore(int amount)
    {
        labelScoreButton?.Init($"High Score: {amount}");
    }

    public void SetDiamonds(int amount)
    {
        labelButton?.Init(amount.ToString());
    }

    private void OnDestroy()
    {
        tutorialButton?.onClick.RemoveAllListeners();
        gameOverButton?.onClick.RemoveAllListeners();
    }
}
