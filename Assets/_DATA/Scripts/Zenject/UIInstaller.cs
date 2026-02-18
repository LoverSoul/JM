using JM.DragDrop;
using JM.Generation;
using JM.Notifications;
using JM.Views;
using UnityEngine;
using Zenject;

public class UIInstaller : MonoInstaller
{
    [SerializeField] private CubeView _cubePrefab;
    [SerializeField] private BottomBarView _bottomBar;
    [SerializeField] private TowerDropZone _towerDropZone;
    [SerializeField] private HoleDropZone _holeDropZone;
    public override void InstallBindings()
    {
        Container.BindInstance(_bottomBar);
        
        Container.Bind<ICubeFactory>()
            .To<CubeFactory>()
            .AsSingle()
            .WithArguments(_cubePrefab);
        
        Container.Bind<TowerDropZone>()
            .FromInstance(_towerDropZone)
            .AsSingle();

        Container.Bind<HoleDropZone>()
            .FromInstance(_holeDropZone)
            .AsSingle();
        
        Container.Bind<IMessagePresenter>()
            .To<GameMessagePresenter>()
            .FromComponentInHierarchy()
            .AsSingle();
        
        
        
    }
}
