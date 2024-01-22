using Animations;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    [SerializeField] GameStateSo gameStateSo;

    protected override void Configure(IContainerBuilder builder)
    {
        // Model
        builder.RegisterInstance(gameStateSo.gameState);

        // VM
        builder.RegisterComponentInHierarchy<EventSystemViewModel>();
        builder.RegisterComponentInHierarchy<TickableManager>();

        // Services
        builder.RegisterComponentInHierarchy<AudioManager>();
        builder.Register<SceneService>(Lifetime.Singleton);
        builder.Register<IAnimationService, AnimationService>(Lifetime.Singleton);

        builder.RegisterEntryPoint<SetupManager>(Lifetime.Scoped);
    }
}
