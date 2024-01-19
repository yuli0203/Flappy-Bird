using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class CollectibleEffectFactory : ICollectibleEffectFactory
{
    private RaccoonViewModel raccoon;
    private GameStateData gameState;
    private HorizontalCharacterController raccoonController;
    private CollectibleSpawner spawner;
    private AudioManager audioManager;
    private CollectibleSpawnerData collectibleData;

    [Inject]
    public void Construct(GameStateData gameState, RaccoonViewModel raccoon, HorizontalCharacterController controller, CollectibleSpawner spawner, AudioManager audio,
        CollectibleSpawnerData collectibleData)
    {
        this.raccoon = raccoon;
        this.gameState = gameState;
        this.raccoonController = controller;
        this.spawner = spawner;
        this.audioManager = audio;
        this.collectibleData = collectibleData;
    }

    public Action CreateEffect(Collider collider)
    {
        if (collider == null)
        {
            return null;
        }

        if (collider.CompareTag("Diamond"))
        {
            return () => DiamondEffect(collider);
        }
        else if (collider.CompareTag("Hole"))
        {
            return () => HoleEffect(collider);
        }        
        else if (collider.CompareTag("Pipes"))
        {
            var pipes = collider.GetComponent<PipesViewModel>();
            bool skipEffectCreation = pipes != null && pipes.IsInSafeZone(raccoon.Collider);
            if (skipEffectCreation)
            {
                return null;
            }
            return () => HoleEffect(collider);
        }

        return () => { };
    }

    private void HoleEffect(Collider collider)
    {
        gameState.gameOver = true;
        raccoon.Fall();
        PlaySound(collider);
    }

    private void DiamondEffect(Collider collider)
    {
        collider.gameObject.SetActive(false);
        spawner.Despawn(collider.gameObject);
        gameState.diamonds += 1;
        PlaySound(collider);
    }

    private void PlaySound(Collider collider)
    {
        foreach(var collectible in collectibleData.supportedTypes)
        {
            if (collectible.collectibleId == collider.tag)
            {
                audioManager.Play(collectible.sound);
                return;
            }
        }
    }
}
