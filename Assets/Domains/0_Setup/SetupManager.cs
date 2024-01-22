

using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class SetupManager : TickableSubscriber, IStartable
{
    private SceneService sceneLoader;
    private GameStateData gameState;

    [Inject]
    public void Construct(SceneService sceneLoader, GameStateData gameState)
    {
        this.sceneLoader = sceneLoader;
        this.gameState = gameState;
    }

    public void Start()
    {
        LoadGame();
    }

    protected override async void MakeTick()
    {
        if (gameState != null && gameState.restart)
        {
            ResetGameState();

            await UniTask.WhenAll(sceneLoader.UnloadSceneAsync(SceneNames.TRACK),
             sceneLoader.UnloadSceneAsync(SceneNames.HUD));

            LoadGame();
        }
    }

    private async void LoadGame()
    {
        ResetGameState();

        await sceneLoader.LoadSceneAsync(SceneNames.HUD);
        await sceneLoader.LoadSceneAsync(SceneNames.TRACK);
    }


    private void ResetGameState()
    {
        gameState.restart = false;
        gameState.gameRunning = false;
        gameState.gameOver = false;
        gameState.diamonds = 0;
        gameState.pipes = 0;
    }
}
