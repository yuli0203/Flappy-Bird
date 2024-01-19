using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class HudManager : TickableSubscriber
{
    private GameStateData gameState;
    private HudViewModel hud;

    private void Awake()
    {
        hud.SetGameOver(false);
        hud.SetTutorial(true);

    }

    [Inject]
    public void Construct(GameStateData gameState, HudViewModel hud)
    {
        this.gameState = gameState;
        this.hud = hud;

        hud.OnTutorialClick += OnTutorialFinished;
        hud.OnGameOverClick += OnGameOver;
        hud.SetHighScore(gameState.highScore);
    }

    protected override void MakeTick()
    {
        hud?.SetDiamonds(gameState.diamonds);
        if (gameState != null && gameState.gameOver)
        {
            hud.SetGameOver(true);
            hud.SetHighScore(gameState.highScore);
        }
    }

    private void OnTutorialFinished()
    {
        hud.SetTutorial(false);
        gameState.gameRunning = true;
    }
    private void OnGameOver()
    {
        gameState.restart = true;
    }

    void OnDestroy()
    {
        if (hud == null)
        {
            return;
        }
        hud.OnTutorialClick -= OnTutorialFinished;
        hud.OnGameOverClick -= OnTutorialFinished;
    }
}
