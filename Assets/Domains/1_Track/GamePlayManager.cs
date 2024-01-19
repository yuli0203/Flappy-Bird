using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GamePlayManager : TickableSubscriber
{
    private const string HighScore = "HighScore";

    private CollectibleSpawner spawner;
    private RaccoonViewModel raccoon;
    private GameStateData gameState;
    private StickViewModel stick;
    private HorizontalCharacterController raccoonController;
    private ICollectibleEffectFactory effectFatory;
    private bool finishGame;
    private Action currentAction;
    private Collider currentCollider;

    [Inject]
    public void Construct(GameStateData gameState, RaccoonViewModel raccoon, CollectibleSpawner spawner, HorizontalCharacterController controller, ICollectibleEffectFactory effectFatory)
    {
        this.spawner = spawner;
        this.raccoon = raccoon;
        this.gameState = gameState;
        this.raccoonController = controller;
        this.effectFatory = effectFatory;

        gameState.diamonds = 0;
        gameState.highScore = PlayerPrefs.GetInt(HighScore, gameState.diamonds);

        raccoon.OnCollision += OnCollision;
    }

    private void OnCollision(Collider collider)
    {
        if (collider == null)
        {
            return;
        }

        if (collider != currentCollider)
        {
            currentCollider = collider;
            currentAction = null;
        }

        if (currentAction != null)
        {
            return;
        }

        currentAction = effectFatory.CreateEffect(collider);

        currentAction?.Invoke();
    }

    protected override void MakeTick()
    {
        var controllable = gameState.gameRunning && !gameState.gameOver;
        raccoonController.SetControllable(controllable);
        raccoon.Walk(controllable);
        spawner.Spawn(gameState.gameRunning);

        if (!finishGame && gameState.gameOver)
        {
            FinishGame();
        }
    }

    private void FinishGame()
    {
        finishGame = true;
        gameState.highScore = gameState.diamonds;
        PlayerPrefs.SetInt(HighScore, gameState.diamonds);
    }
}
