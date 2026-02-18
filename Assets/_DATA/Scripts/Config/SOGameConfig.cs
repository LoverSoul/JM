using System.Collections.Generic;
using JM.Rules;
using UnityEngine;

namespace JM.Config
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "JM/Configs/GameConfig", order = 1)]
    public class SOGameConfig : ScriptableObject
    {
        public int cubeCount = 20;
        [Header("Cubes")] 
        public Sprite[] sprites;
        public GameRule[]  rules;
    }

}
