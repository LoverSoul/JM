using System.Collections.Generic;
using JM.DragDrop;
using JM.Game;
using JM.Models;
using JM.Views;
using UnityEngine;

namespace  JM.Generation
{
    public class CubeSpawner
    {
        private readonly ICubeFactory _factory;
        private readonly GamePresenter _presenter;

        public CubeSpawner(
            ICubeFactory factory,
            GamePresenter presenter)
        {
            _factory = factory;
            _presenter = presenter;
        }

        public List<CubeView> Spawn(
            List<CubeModel> models,
            Transform parent)
        {
            var views = new List<CubeView>();

            foreach (var model in models)
            {
                var view = _factory.Create(model);
                view.transform.SetParent(parent, false);

                var drag = view.GetComponent<CubeDragHandler>();
                _presenter.BindCube(drag);

                views.Add(view);
            }

            return views;
        }
    }
}