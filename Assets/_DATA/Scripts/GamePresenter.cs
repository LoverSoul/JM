using System.Collections.Generic;
using System.Threading.Tasks;
using JM.Animations;
using JM.Config;
using JM.DragDrop;
using JM.Notifications;
using JM.Rules;
using JM.Saves;
using JM.Views;
using UnityEngine;

namespace JM.Game
{
    public class GamePresenter
    {
        private readonly IEnumerable<IGameRule> _gameRules;
        private readonly IMessagePresenter _messagePresenter;
        private readonly TowerDropZone _towerZone;
        private readonly HoleDropZone _holeZone;
        private readonly ICubeAnimationHandler _animations;
        private readonly IGameStateSaver _gameStateSaver;

        public GamePresenter(
            IGameConfigProvider configProvider,
            IMessagePresenter messagePresenter,
            TowerDropZone towerZone,
            HoleDropZone holeZone,
            ICubeAnimationHandler animations,
            IGameStateSaver gameStateSaver)
        {
            _gameRules = configProvider.GetGameRules();
            _messagePresenter = messagePresenter;
            _towerZone = towerZone;
            _holeZone = holeZone;
            _animations = animations;
            _gameStateSaver = gameStateSaver;
        }

        public void BindCube(CubeDragHandler drag)
        {
            drag.OnDropped += HandleDrop;
            drag.OnBeginDragged += HandleBeginDrag;
        }

        private bool ValidateTowerRules(CubeView view)
        {
            foreach (var rule in _gameRules)
            {
                if (!rule.Validate(_towerZone, view.Model, out string failExplanation))
                {
                    _messagePresenter.Show(failExplanation);
                    return false;
                }
            }

            return true;
        }

        private async void HandleBeginDrag(CubeView view)
        {
            _messagePresenter.Show("drag_start");

            if (!_towerZone.ContainsCube(view))
                return;

            List<CubeView> onTopCubes = _towerZone.GetCubesOnTopOf(view);
            _towerZone.RemoveCube(view);
            
            foreach (var cube in onTopCubes)
                _towerZone.RemoveCube(cube);

            CubeView currentBase = _towerZone.GetTopCube();

            foreach (CubeView fallingCube in onTopCubes)
            {
                Vector3 targetPos = CalculateTargetPosition(currentBase, fallingCube);

                bool survived = await DropTowerCubeValidated(
                    fallingCube,
                    targetPos,
                    currentBase);

                if (!survived)
                    continue;

                currentBase = fallingCube;
            }

            _gameStateSaver.Save();
        }

        private Vector3 CalculateTargetPosition(CubeView baseCube, CubeView cube)
        {
            if (baseCube == null)
                return _towerZone.GetBottomPosition(cube);

            RectTransform baseRect = baseCube.RectTransform;
            RectTransform cubeRect = cube.RectTransform;

            float targetY = baseRect.localPosition.y + baseRect.rect.height;

            return new Vector3(
                cubeRect.localPosition.x,
                targetY,
                0f);
        }

        private async Task<bool> DropTowerCubeValidated(
            CubeView cube,
            Vector3 targetPos,
            CubeView baseCube)
        {
            if (baseCube != null && !IsWithinTolerance(baseCube, cube))
            {
                await _animations.FallAndExplodeAsync(cube, targetPos);
                _animations.Reset(cube);
                return false;
            }

            await _animations.FallAsync(cube, targetPos);
            _towerZone.AddCubeOnTop(cube);

            return true;
        }
        

        private void HandleDrop(CubeView view, Vector2 position)
        {
            if (_towerZone.Contains(position))
            {
                HandleTowerDrop(view);
                return;
            }

            if (_holeZone.Contains(position))
            {
                HandleHoleDrop(view);
                return;
            }

            DropWithoutAnyEntrance(view);
        }

        private async void DropWithoutAnyEntrance(CubeView view)
        {
            await _animations.ExplodeAsync(view);
            _animations.Reset(view);
            _gameStateSaver.Save();
        }

        private async void HandleHoleDrop(CubeView view)
        {
            Vector2 holePos = _holeZone.GetBottomPosition(view);

            if (_holeZone.CrossHole(view, out holePos))
            {
                _messagePresenter.Show("hole_entered");
                await _animations.FallAndExplodeAsync(view, holePos);
            }
            else
            {
                _messagePresenter.Show("hole_missed");
                await _animations.ExplodeAsync(view);
            }

            _animations.Reset(view);
            _gameStateSaver.Save();
        }

        private async void HandleTowerDrop(CubeView view)
        {
            Vector3 basePos = _towerZone.GetBottomPosition(view);

            if (!ValidateTowerRules(view))
            {
                await _animations.FallAndExplodeAsync(view, basePos);
                _animations.Reset(view);
                _gameStateSaver.Save();
                return;
            }

            CubeView topCube = _towerZone.GetTopCube();

            if (topCube == null)
            {
                _towerZone.AddCubeOnTop(view);
                await _animations.FallAsync(view, basePos);
                await _animations.JumpAsync(view);
                _messagePresenter.Show("first_cube");
                _gameStateSaver.Save();
                return;
            }

            RectTransform topRect = topCube.RectTransform;
            RectTransform viewRect = view.RectTransform;

            float requiredMinY =
                topRect.localPosition.y + topRect.rect.height * 0.5f;

            float cubeBottomY =
                viewRect.localPosition.y - viewRect.rect.height * 0.5f;

            if (cubeBottomY <= requiredMinY)
            {
                _messagePresenter.Show("too_low");
                await _animations.FallAndExplodeAsync(view, basePos);
                _animations.Reset(view);
                _gameStateSaver.Save();
                return;
            }

            if (!IsWithinTolerance(topCube, view))
            {
                _messagePresenter.Show("cube_missed");
                await _animations.FallAndExplodeAsync(view, basePos);
                _animations.Reset(view);
                _gameStateSaver.Save();
                return;
            }

            float targetY =
                topRect.localPosition.y + topRect.rect.height;

            Vector3 targetPos = new Vector3(
                viewRect.localPosition.x,
                targetY,
                0f);

            _towerZone.AddCubeOnTop(view);
            _messagePresenter.Show("cube_placed");

            await _animations.FallAsync(view, targetPos);
            await _animations.JumpAsync(view);

            _gameStateSaver.Save();
        }
        

        private float CalculateTolerance(CubeView baseCube)
        {
            RectTransform baseRect = baseCube.RectTransform;
            return baseRect.rect.width * 0.5f;
        }

        private bool IsWithinTolerance(CubeView baseCube, CubeView cube)
        {
            RectTransform baseRect = baseCube.RectTransform;
            RectTransform cubeRect = cube.RectTransform;

            float tolerance = CalculateTolerance(baseCube);

            float distanceX = Mathf.Abs(
                cubeRect.localPosition.x - baseRect.localPosition.x);

            return distanceX <= tolerance;
        }
    }
}
