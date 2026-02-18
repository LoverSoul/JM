using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using JM.DragDrop;
using JM.Views;
using UnityEngine;

namespace JM.Animations
{
    public class CubeAnimationHandler : ICubeAnimationHandler
    {
        private readonly BottomBarView _bottomBar;
        private readonly ICubeAnimationService _animations;
        
        public CubeAnimationHandler(
            BottomBarView bottomBar,
            ICubeAnimationService animations)
        {
            _bottomBar = bottomBar;
            _animations = animations;
        }
        
        public async Task FallAsync(CubeView cube, Vector3 position)
        {
            await _animations.MoveTo(cube.RectTransform,position);
        }

        public async Task ExplodeAsync(CubeView cube)
        {
            await _animations.Explode(cube.RectTransform);
        }
        public async Task JumpAsync(CubeView cube)
        {
            await _animations.Jump(cube.RectTransform);
        }

        public async Task FallAndExplodeAsync(CubeView cube, Vector3 position)
        {
            await _animations.MoveTo(cube.RectTransform,position);
            await _animations.Explode(cube.RectTransform);
        }

        public void Reset(CubeView cube)
        {
            cube.RectTransform.localScale = Vector3.one;
            cube.transform.SetParent(_bottomBar.Content, false);

            var drag = cube.GetComponent<CubeDragHandler>();
            if (drag != null)
                drag.ResetPosition();
        }
    }
}
