using JM.Views;
using UnityEngine;

namespace JM.DragDrop
{
    public class HoleDropZone : DropZone, IDropZone
    {
        [SerializeField] private RectTransform _hole;
        
        public bool CrossHole(CubeView cubeView, out Vector2 holePoint)
        {
            RectTransform cube = cubeView.RectTransform;
            RectTransform cubeParent = cube.parent as RectTransform;

            holePoint = GetBottomPosition(cubeView);

            Vector3[] cubeCorners = new Vector3[4];
            Vector3[] holeCorners = new Vector3[4];

            cube.GetWorldCorners(cubeCorners);
            _hole.GetWorldCorners(holeCorners);

            // Переводим в локальное пространство родителя куба
            for (int i = 0; i < 4; i++)
            {
                cubeCorners[i] = cubeParent.InverseTransformPoint(cubeCorners[i]);
                holeCorners[i] = cubeParent.InverseTransformPoint(holeCorners[i]);
            }

            float cubeLeft = cubeCorners[0].x;
            float cubeRight = cubeCorners[2].x;

            float holeLeft = holeCorners[0].x;
            float holeRight = holeCorners[2].x;
            float holeTop = holeCorners[1].y;

            bool intersectsX = cubeRight >= holeLeft && cubeLeft <= holeRight;
            if (!intersectsX)
                return false;

            float cubeCenterX = (cubeLeft + cubeRight) * 0.5f;

            holePoint = new Vector2(cubeCenterX, holeTop);

            return true;
        }

    }
}