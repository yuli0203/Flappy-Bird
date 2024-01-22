using Cysharp.Threading.Tasks;
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
    private RunnerViewModel raccoon;
    private GameStateData gameState;
    private HorizontalCharacterController controller;
    private ICollectibleEffectFactory effectFatory;
    private bool finishGame;
    private UniTask? currentAction;
    private Collider currentCollider;

    [Inject]
    public void Construct(GameStateData gameState, RunnerViewModel raccoon, CollectibleSpawner spawner, HorizontalCharacterController controller, ICollectibleEffectFactory effectFatory)
    {
        this.spawner = spawner;
        this.raccoon = raccoon;
        this.gameState = gameState;
        this.controller = controller;
        this.effectFatory = effectFatory;

        gameState.diamonds = 0;
        gameState.highScore = PlayerPrefs.GetInt(HighScore, gameState.pipes);

        raccoon.OnCollision += OnCollision;
    }

    private void OnCollision(Collider collider)
    {
        if (collider == null || collider == currentCollider)
        {
            return;
        }

        currentCollider = collider;

        // Create a new UniTask only if the previous one has completed
        currentAction = effectFatory.CreateEffect(collider);

        // Setup a continuation to set currentAction to null after the UniTask completes
        currentAction.Value.ContinueWith(() => currentAction = null).Forget();
    }



    protected override void MakeTick()
    {
        var controllable = gameState.gameRunning && !gameState.gameOver;
        controller.SetControllable(controllable);
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
        gameState.highScore = gameState.pipes;
        PlayerPrefs.SetInt(HighScore, gameState.pipes);
    }
}
