using System.Threading.Tasks;
using UnityEngine;

namespace JM.Animations
{
    public interface ICubeAnimationService
    {
        Task MoveWorld(RectTransform cube, Vector3 target);
        Task MoveTo(RectTransform cube, Vector2 target);
        Task Jump(RectTransform cube);
        Task Destroy(RectTransform cube);
        Task Explode(RectTransform cube);
        Task ScaleToZero(RectTransform cube);
    }
}