using JM.Animations;
using JM.Config;
using JM.Game;
using JM.Generation;
using JM.Localization;
using JM.Notifications;
using JM.Saves;
using UnityEngine;
using Zenject;

namespace JM.DI
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private SOGameConfig _gameConfig;
        public override void InstallBindings()
        {
            Container.Bind<SOGameConfig>()
                .FromInstance(_gameConfig)
                .AsSingle();
            
            Container.Bind<ICubeAnimationHandler>()
                .To<CubeAnimationHandler>()
                .AsSingle();
            
            Container.Bind<ICubeAnimationService>()
                .To<DOTweenCubeAnimationService>()
                .AsSingle();

            
            Container.Bind<IGameConfigProvider>().To<SOGameConfigProvider>().AsSingle();
            Container.Bind<ILocalizationService>().To<PrimitiveLocalizationService>().AsSingle();
            Container.Bind<ISaveService>().To<PlayerPrefsSaveService>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<GamePresenter>().AsSingle();
            Container.BindInterfacesAndSelfTo<CubeModelGenerator>().AsSingle();
            Container.BindInterfacesAndSelfTo<CubeSpawner>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameBootstrap>().AsSingle();
            Container.Bind<IGameStateSaver>()
                .To<GameStateSaver>()
                .AsSingle();


        }
    }
}
