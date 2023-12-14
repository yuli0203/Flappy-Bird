using UnityEngine;
using VContainer;

public class PathViewModel : PoolViewModel
{
    public float scrollSpeed = 0.1f;
    private GameStateData gameStateData;

    [Inject]
    public void Construct(GameStateData gameStateData)
    {
        this.gameStateData = gameStateData;
    }

    void Update()
    {
        if (gameStateData == null || !gameStateData.gameRunning)
        {
            return;
        }

        // Calculate the new offset based on time and speed
        float offset = Time.time * scrollSpeed;

        // Create a new Vector2 with the calculated offset
        Vector2 offsetVector = new Vector2(0, offset);

        // Apply the offset to the material's main texture
        GetComponent<Renderer>().material.mainTextureOffset = offsetVector;
    }
}