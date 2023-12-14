using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GamePlayManager : TickableSubscriber
{
    private CollectibleSpawner spawner;
    private RaccoonViewModel raccoon;
    private GameStateData gameState;
    private StickViewModel stick;
    private HorizontalCharacterController raccoonController;
    private ICollectibleEffectFactory effectFatory;

    [Inject]
    public void Construct(GameStateData gameState, RaccoonViewModel raccoon, CollectibleSpawner spawner, HorizontalCharacterController controller, ICollectibleEffectFactory effectFatory)
    {
        this.spawner = spawner;
        this.raccoon = raccoon;
        this.gameState = gameState;
        this.raccoonController = controller;
        this.effectFatory = effectFatory;

        gameState.diamonds = 0;

        raccoon.OnCollision += OnCollision;
    }

    private void OnCollision(Collider collider)
    {
        effectFatory.CreateEffect(collider)?.Invoke();
    }

    protected override void MakeTick()
    {
        var controllable = gameState.gameRunning && !gameState.gameOver;
        raccoonController.SetControllable(controllable);
        raccoon.Walk(controllable);
        spawner.Spawn(gameState.gameRunning);
    }
}
