using Animations;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;

public class PathViewModel : PoolViewModel
{
    public float scrollSpeed = 0.1f;
    public Renderer _renderer;
    private GameStateData gameStateData;
    private IAnimationService animationService;

    [Inject]
    public void Construct(GameStateData gameStateData, IAnimationService animationService)
    {
        this.gameStateData = gameStateData;
        this.animationService = animationService;
        animationService.Subscribe(gameObject, Rotate);
    }

    void Rotate(Transform trans)
    {
        if (gameStateData == null || !gameStateData.gameRunning)
        {
            return;
        }

        // Calculate the new offset based on time and speed
        float offset = Time.time * scrollSpeed;

        // Create a new Vector2 with the calculated offset
        Vector2 offsetVector = new Vector2(offset, 0);

        // Apply the offset to the material's main texture using SetTextureOffset
        _renderer.material.mainTextureOffset = offsetVector;
    }

    private void OnDestroy()
    {
        animationService.UnSubscribe(gameObject);
    }
}