using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using VContainer.Unity;

public class SceneService
{
    readonly LifetimeScope currentScope;

    public SceneService(LifetimeScope currentScope)
    {
        this.currentScope = currentScope; // Inject the LifetimeScope to which this class belongs
    }

    public async UniTask LoadSceneAsync(string sceneName, LifetimeScope parentScope = null)
    {
        var scope = parentScope == null ? currentScope : parentScope;
        using (LifetimeScope.EnqueueParent(scope))
        {
            await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }
    }
    public async UniTask UnloadSceneAsync(string sceneName)
    {
        await SceneManager.UnloadSceneAsync(sceneName);
    }
}