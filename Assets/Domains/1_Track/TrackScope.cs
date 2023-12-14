

using Animations;
using System.ComponentModel.Design;
using VContainer.Unity;
using VContainer;
using UnityEngine;
using UnityEditor.Experimental.GraphView;

public class TrackScope : LifetimeScope
{
    [SerializeField] CollectibleSpawnerSO collectibleSpawnerSo;
    [SerializeField] GameStateSo gameStateSo;

    protected override void Configure(IContainerBuilder builder)
    {
        // Model
        builder.RegisterInstance(collectibleSpawnerSo.spawnerData);
        builder.RegisterInstance(gameStateSo.gameState);

        // View Model
        builder.RegisterComponentInHierarchy<EventSystemViewModel>();
        builder.RegisterComponentInHierarchy<CharacterViewModel>();
        builder.RegisterComponentInHierarchy<StickViewModel>();
        builder.RegisterComponentInHierarchy<RaccoonViewModel>();
        builder.RegisterComponentInHierarchy<PathViewModel>();
        builder.RegisterComponentInHierarchy<PoolViewModel>();

        // Controllers
        builder.RegisterComponentInHierarchy<TickableManager>();
        builder.Register<HorizontalCharacterController>(Lifetime.Scoped);
        builder.Register<CollectibleSpawner>(Lifetime.Scoped);
        builder.Register<ICollectibleEffectFactory, CollectibleEffectFactory>(Lifetime.Scoped);

        builder.RegisterEntryPoint<GamePlayManager>(Lifetime.Scoped);
    }
}
