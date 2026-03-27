using KidsGame.Scripts.Infrastructure.Factory;
using KidsGame.Scripts.Core.Application.Drop;
using KidsGame.Scripts.Core.Controller;
using KidsGame.Scripts.Core.Domain.Placement;
using KidsGame.Scripts.Infrastructure.Services.Animation;
using KidsGame.Scripts.Infrastructure.Services.Coordinates;
using KidsGame.Scripts.Infrastructure.Services.Feedback;
using KidsGame.Scripts.Infrastructure.Services.Localization;
using KidsGame.Scripts.Infrastructure.Services.Save;
using KidsGame.Scripts.Infrastructure.Services.State;
using KidsGame.Scripts.Infrastructure.Services.Tower;
using KidsGame.Scripts.View;
using KidsGame.Scripts.Config;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private BoxConfig _boxConfig;
    [SerializeField] private DragRulesConfig _dragRulesConfig;
    [SerializeField] private AnimationConfig _animationConfig;
    [SerializeField] private UICubeView _uiCubeView;
    [SerializeField] private CubeView _cubeView;
    public override void InstallBindings()
    {
        BindConfig();
        BindFactory();
        BindSignals();
        BindServices();
    }
    
    private void BindConfig()
    {
        Container.Bind<BoxConfig>().FromInstance(_boxConfig).AsSingle();
        Container.Bind<DragRulesConfig>().FromInstance(_dragRulesConfig).AsSingle();
        Container.Bind<AnimationConfig>().FromInstance(_animationConfig).AsSingle();
    }

    private void BindFactory()
    {
        Container.Bind<IUICubeFactory>().To<UiCubeFactory>().AsSingle().WithArguments(_uiCubeView);
        Container.Bind<ICubeFactory>().To<CubeFactory>().AsSingle().WithArguments(_cubeView);
        Container.Bind<ICubeDragBehaviourFactory>().To<CubeDragBehaviourFactory>().AsSingle();
    }

    private void BindSignals()
    {
        Container.Bind<ICubeSelectionChannel>().To<CubeSelectionChannel>().AsSingle();
    }

    private void BindServices()
    {
        Container.Bind<ILocalizationService>().To<LocalizationService>().AsSingle();
        Container.Bind<IGameSaveService>().To<PlayerPrefsGameSaveService>().AsSingle();
        Container.Bind<IActionFeedbackService>().To<TweenActionFeedbackService>().AsSingle();
        Container.Bind<ITowerStateService>().To<TowerStateService>().AsSingle();
        Container.Bind<IDragCoordinateService>().To<UnityDragCoordinateService>().AsSingle();
        Container.Bind<IZoneClampService>().To<ZoneClampService>().AsSingle();
        Container.Bind<ITowerQueryService>().To<TowerQueryService>().AsSingle();
        Container.Bind<ITowerCollapseAnimationService>().To<TowerCollapseAnimationService>().AsSingle();
        Container.Bind<IHoleFallAnimationService>().To<HoleFallAnimationService>().AsSingle();
        Container.Bind<ICubePlaceAnimationService>().To<CubePlaceAnimationService>().AsSingle();
        Container.Bind<ICubeMissAnimationService>().To<CubeMissAnimationService>().AsSingle();
        Container.Bind<IDropDecisionService>().To<DropDecisionService>().AsSingle();
        Container.Bind<IPlacementPolicy>().To<DefaultPlacementPolicy>().AsSingle();
    }
}
