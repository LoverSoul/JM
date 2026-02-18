using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JM.DragDrop;
using JM.Generation;
using JM.Models;
using JM.Saves;
using JM.Views;
using UnityEngine;
using Zenject;

namespace JM.Game
{
   public class GameBootstrap : IInitializable
{
    private readonly ISaveService _saveService;
    private readonly CubeModelGenerator _modelGenerator;
    private readonly CubeSpawner _spawner;
    private readonly BottomBarView _bottomBarView;
    private readonly TowerDropZone _towerZone;

    public GameBootstrap(
        ISaveService saveService,
        CubeModelGenerator modelGenerator,
        CubeSpawner spawner,
        BottomBarView bottomBarView,
        TowerDropZone towerZone)
    {
        _saveService = saveService;
        _modelGenerator = modelGenerator;
        _spawner = spawner;
        _bottomBarView = bottomBarView;
        _towerZone = towerZone;
    }

    public void Initialize()
    {
        var progress = _saveService.Load();

        if (progress == null)
        {
            StartNewGame();
            return;
        }

        RestoreBottom(progress);
        RestoreTower(progress);
    }

    private void StartNewGame()
    {
        var models = _modelGenerator.Generate();
        _spawner.Spawn(models, _bottomBarView.Content);
    }

    private void RestoreBottom(GameProgressDTO progress)
    {
        if (progress.BottomBarSnapshot?.Cubes == null ||
            progress.BottomBarSnapshot.Cubes.Count == 0)
        {
            StartNewGame();
            return;
        }

        var models = progress.BottomBarSnapshot.Cubes
            .Select(c => new CubeModel(c.Id))
            .ToList();

        var views = _spawner.Spawn(models, _bottomBarView.Content);

        for (int i = 0; i < views.Count; i++)
        {
            views[i].RectTransform.localPosition =
                progress.BottomBarSnapshot.Cubes[i].LocalPosition;
        }
    }

    private void RestoreTower(GameProgressDTO progress)
    {
        if (progress.TowerSnapshot?.Cubes == null ||
            progress.TowerSnapshot.Cubes.Count == 0)
            return;

        var models = progress.TowerSnapshot.Cubes
            .Select(c => new CubeModel(c.Id))
            .ToList();

        var views = _spawner.Spawn(models, _towerZone.Canvas);

        for (int i = 0; i < views.Count; i++)
        {
            views[i].RectTransform.localPosition =
                progress.TowerSnapshot.Cubes[i].LocalPosition;

            _towerZone.AddCubeOnTop(views[i]);
        }
    }
}

}
