using System.Collections;
using System.Collections.Generic;
using JM.DragDrop;
using JM.Models;
using JM.Rules;
using UnityEngine;

namespace JM.Rules
{
    public class GameRule : ScriptableObject, IGameRule
    {
        [SerializeField] protected string failExplanationText = "";
        public virtual bool Validate(TowerDropZone towerDropZone, CubeModel cubeModel, out string failExplanation)
        {
            failExplanation = failExplanationText;
            return true;
        }
    }
}
