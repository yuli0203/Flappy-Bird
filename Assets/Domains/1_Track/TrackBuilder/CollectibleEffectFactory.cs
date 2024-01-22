using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using VContainer;

public class CollectibleEffectFactory : ICollectibleEffectFactory
{
    private RunnerViewModel runner;
    private GameStateData gameState;
    private HorizontalCharacterController runnerController;
    private CollectibleSpawner spawner;
    private AudioManager audioManager;
    private CollectibleSpawnerData collectibleData;

    [Inject]
    public void Construct(GameStateData gameState, RunnerViewModel runner, HorizontalCharacterController controller, CollectibleSpawner spawner, AudioManager audio,
        CollectibleSpawnerData collectibleData)
    {
        this.runner = runner;
        this.gameState = gameState;
        this.runnerController = controller;
        this.spawner = spawner;
        this.audioManager = audio;
        this.collectibleData = collectibleData;
    }

    public async UniTask CreateEffect(Collider collider)
    {
        if (collider == null)
        {
            return;
        }

        if (collider.CompareTag("Diamond"))
        {
            await DiamondEffect(collider);
        }
        else if (collider.CompareTag("Hole"))
        {
            await HoleEffect(collider);
        }        
        else if (collider.CompareTag("Pipes"))
        {
            await PipesEffect(collider);
        }

        return;
    }

    private async Task PipesEffect(Collider collider)
    {
        var pipes = collider.GetComponent<PipesViewModel>();
        while (pipes.IsWithinPipes(runner.Collider))
        {
            bool skipEffectCreation = pipes != null && pipes.IsInSafeZone(runner.Collider);
            if (skipEffectCreation)
            {
                await UniTask.Yield();
            }

            else
            {
                await HoleEffect(collider);
                return;
            }
        }

        gameState.pipes += 1;
    }

    private UniTask HoleEffect(Collider collider)
    {
        gameState.gameOver = true;
        runner.Fall();
        PlaySound(collider);
        return UniTask.CompletedTask;
    }

    private UniTask DiamondEffect(Collider collider)
    {
        collider.gameObject.SetActive(false);
        spawner.Despawn(collider.gameObject);
        gameState.diamonds += 1;
        PlaySound(collider);
        return UniTask.CompletedTask;
    }

    private UniTask PlaySound(Collider collider)
    {
        foreach(var collectible in collectibleData.supportedTypes)
        {
            if (collectible.collectibleId == collider.tag)
            {
                audioManager.Play(collectible.sound);
            }
        }

        return UniTask.CompletedTask;
    }
}
