using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class CollectibleEffectFactory : ICollectibleEffectFactory
{
    private RaccoonViewModel raccoon;
    private GameStateData gameState;
    private StickViewModel stick;
    private HorizontalCharacterController raccoonController;
    private CollectibleSpawner spawner;
    private AudioManager audioManager;
    private CollectibleSpawnerData collectibleData;

    [Inject]
    public void Construct(GameStateData gameState, RaccoonViewModel raccoon, StickViewModel stick, HorizontalCharacterController controller, CollectibleSpawner spawner, AudioManager audio,
        CollectibleSpawnerData collectibleData)
    {
        this.raccoon = raccoon;
        this.gameState = gameState;
        this.stick = stick;
        this.raccoonController = controller;
        this.spawner = spawner;
        this.audioManager = audio;
        this.collectibleData = collectibleData;
    }

    public Action CreateEffect(Collider collider)
    {
        if (collider.CompareTag("Stick"))
        {
            return () => StickEffect(collider);
        }
        else if (collider.CompareTag("Diamond"))
        {
            return () => DiamondEffect(collider);
        }
        else if (collider.CompareTag("Cutter"))
        {
            return () => CutterEffect(collider);
        }
        else if (collider.CompareTag("Hole"))
        {
            return () => HoleEffect(collider);
        }

        return () => { };
    }

    private void HoleEffect(Collider collider)
    {
        if (stick.StickSize < collider.bounds.size.x)
        {
            gameState.gameOver = true;
            raccoon.Fall();
            PlaySound(collider);
        }
        else
        {
            audioManager.Play(collectibleData.savedAudioClip);
        }
    }

    private void CutterEffect(Collider collider)
    {
        spawner.Despawn(collider.gameObject);
        stick.ChangeSize(stick.StickSize - stick.StickSizeDelta);
        PlaySound(collider);
    }

    private void DiamondEffect(Collider collider)
    {
        collider.gameObject.SetActive(false);
        spawner.Despawn(collider.gameObject);
        gameState.diamonds += 1;
        PlaySound(collider);
    }

    private void StickEffect(Collider collider)
    {
        collider.gameObject.SetActive(false);
        stick.ChangeSize(stick.StickSize + stick.StickSizeDelta);
        spawner.Despawn(collider.gameObject);
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
