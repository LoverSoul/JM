using System.Linq;
using JM.DragDrop;
using JM.Saves;
using JM.Views;
using UnityEngine;

namespace JM.Game
{
    public class GameStateSaver : IGameStateSaver
    {
        private readonly ISaveService _saveService;
        private readonly TowerDropZone _towerZone;
        private readonly BottomBarView _bottomBarView;

        public GameStateSaver(
            ISaveService saveService,
            TowerDropZone towerZone,
            BottomBarView bottomBarView)
        {
            _saveService = saveService;
            _towerZone = towerZone;
            _bottomBarView = bottomBarView;
        }

        public void Save()
        {
            var progress = new GameProgressDTO
            {
                TowerSnapshot = CreateTowerSnapshot(),
                BottomBarSnapshot = CreateBottomSnapshot()
            };

            _saveService.Save(progress);
         //   Debug.Log("Saved " + progress);
        }

        private Snapshot CreateTowerSnapshot()
        {
            var snapshot = new Snapshot();

            foreach (var cube in _towerZone.Cubes)
            {
                snapshot.Cubes.Add(new CubeSnapshot
                {
                    Id = cube.Model.NameID,
                    LocalPosition = cube.RectTransform.localPosition
                });
            }

            return snapshot;
        }

        private Snapshot CreateBottomSnapshot()
        {
            var snapshot = new Snapshot();

            foreach (CubeView cube in _bottomBarView.Content.GetComponentsInChildren<CubeView>())
            {
                snapshot.Cubes.Add(new CubeSnapshot
                {
                    Id = cube.Model.NameID,
                    LocalPosition = cube.RectTransform.localPosition
                });
            }

            return snapshot;
        }
    }
}