using JM.Config;
using JM.DragDrop;
using JM.Models;
using JM.Views;
using UnityEngine;
using Zenject;

namespace JM.Generation
{
    public class CubeFactory : ICubeFactory
    {
        private readonly DiContainer _container;
        private readonly Transform _parent;
        private readonly CubeView _prefab;
        private readonly IGameConfigProvider _provider;

        public CubeFactory(
            CubeView prefab,
            DiContainer container,
            BottomBarView bottomBar,
            IGameConfigProvider  provider)
        {
            _provider = provider;
            _prefab = prefab;
            _container = container;
            _parent = bottomBar.Content;
        }

        public CubeView Create(CubeModel model)
        {
            var view = _container.InstantiatePrefabForComponent<CubeView>(
                _prefab,
                _parent
            );
            view.SetModel(model);
            view.SetSprite(_provider.GetSpriteByID(model.NameID));

            var drag = view.GetComponent<CubeDragHandler>();
            drag.Initialize(view.GetComponentInParent<Canvas>().rootCanvas.transform);

            return view;
        }


    }
}

