using System.Collections;
using System.Collections.Generic;
using JM.DragDrop;
using JM.Models;
using JM.Rules;
using JM.Views;
using UnityEngine;

namespace  JM.Rules
{
    [CreateAssetMenu(fileName = "SameColorGameRule", menuName = "JM/Rules/SameColorGameRule")]
    public class SameColorGameRule : GameRule
    {
        public override bool Validate(TowerDropZone towerDropZone, CubeModel cubeView, out string failExplanation)
        {
            failExplanation = failExplanationText;
            
            if (towerDropZone.GetTopCube().Model.NameID == cubeView.NameID)
                return true;
            return false;
        }
    }
}
