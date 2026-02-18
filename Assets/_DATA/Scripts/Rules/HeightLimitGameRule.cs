using JM.DragDrop;
using JM.Models;
using JM.Views;
using UnityEngine;

namespace JM.Rules
{
    [CreateAssetMenu(fileName = "HeightLimitGameRule", menuName = "JM/Rules/HeightLimitGameRule")]
    public class HeightLimitGameRule : GameRule
    {
        public override bool Validate(TowerDropZone towerDropZone, CubeModel cubeModel,out string failExplanation)
        {
            failExplanation = failExplanationText;
            if (towerDropZone.CubeCount == 0)
                return true;

            CubeView topCube = towerDropZone.GetTopCube();
            if (topCube == null)
                return true;

            RectTransform topRect = topCube.RectTransform;
            RectTransform zoneRect = towerDropZone.Rect;
            
            Vector3[] topCorners = new Vector3[4];
            Vector3[] zoneCorners = new Vector3[4];

            topRect.GetWorldCorners(topCorners);
            zoneRect.GetWorldCorners(zoneCorners);

            float currentTopY = topCorners[1].y; 
            float cubeHeight = topRect.rect.height;

            float nextTopY = currentTopY + cubeHeight;
            float zoneTopY = zoneCorners[1].y;

            return nextTopY <= zoneTopY;
        }
    }
}