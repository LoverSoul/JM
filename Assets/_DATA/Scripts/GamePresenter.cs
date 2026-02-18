using System.Collections.Generic;
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
        private readonly  IGameStateSaver _gameStateSaver;

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
            _gameStateSaver = gameStateSaver;
            _animations = animations;
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


        private void HandleBeginDrag(CubeView view)
        {
            _messagePresenter.Show("drag_start");
            if (_towerZone.ContainsCube(view))
            {
                List<CubeView> onTopCubes = _towerZone.GetCubesOnTopOf(view);
                foreach (CubeView onTopCube in onTopCubes)
                {
                    RectTransform rect = onTopCube.RectTransform;
                    Vector3 pos = rect.localPosition -
                                  new Vector3(0, rect.rect.height, 0);
                    
                    DropTowerCubes(onTopCube, pos);
                }

                _towerZone.RemoveCube(view);
            }
        }

        private async void DropTowerCubes(CubeView view, Vector3 position)
        {
            await _animations.FallAsync(view, position);
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

            float halfWidth = topRect.rect.width * 0.5f;
            float tolerance = halfWidth; 

            float distanceX = Mathf.Abs(
                viewRect.localPosition.x - topRect.localPosition.x);

            if (distanceX > tolerance)
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


    }
}
