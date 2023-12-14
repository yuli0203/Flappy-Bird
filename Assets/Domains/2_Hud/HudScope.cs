

using Animations;
using System.ComponentModel.Design;
using VContainer.Unity;
using VContainer;
using UnityEngine;

public class HudScope : LifetimeScope
{
    [SerializeField] GameStateSo gameStateSo;
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterInstance(gameStateSo.gameState);

        builder.RegisterComponentInHierarchy<EventSystemViewModel>();
        builder.Register<IAnimationService, AnimationService>(Lifetime.Scoped);

        builder.RegisterComponentInHierarchy<HudViewModel>();

        builder.RegisterComponentInHierarchy<TickableManager>();
        builder.RegisterEntryPoint<HudManager>();
    }
}
