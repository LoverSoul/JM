using System.Collections;
using System.Collections.Generic;
using JM.DragDrop;
using JM.Models;
using UnityEngine;

namespace JM.Rules
{
    public interface IGameRule
    {
        public bool Validate(TowerDropZone towerDropZone, CubeModel cubeModel, out string failExplanation);
    }
}
