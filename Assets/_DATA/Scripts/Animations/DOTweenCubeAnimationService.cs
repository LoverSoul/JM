using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace JM.Animations
{
    public class DOTweenCubeAnimationService : ICubeAnimationService
    {
        public async Task MoveWorld(RectTransform cube, Vector3 target)
        {
            await cube.DOMove(target, 0.25f).AsyncWaitForCompletion();
        }
        
        public async Task MoveTo(RectTransform cube, Vector2 target)
        {
            await cube.DOLocalMove(target, 0.2f).AsyncWaitForCompletion();
        }

        public async Task Jump(RectTransform cube)
        {
            await cube.DOAnchorPosY(cube.anchoredPosition.y + 10f, 0.1f)
                .SetLoops(2, LoopType.Yoyo)
                .AsyncWaitForCompletion();
        }

        public async Task Destroy(RectTransform cube)
        {
            await cube.DOScale(Vector3.zero, 0.3f).AsyncWaitForCompletion();
            Object.Destroy(cube.gameObject);
        }
        
        public async Task Explode(RectTransform cube)
        {

            await DOTween.Sequence()
                .Append(cube.DOScale(1.4f, 0.12f))
                .Append(cube.DOScale(0f, 0.18f))
                .AsyncWaitForCompletion();
            
        }

        public async Task ScaleToZero(RectTransform cube)
        {
            await cube.DOScale(Vector3.zero, 0.25f).AsyncWaitForCompletion();
        }

    }
}