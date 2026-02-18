using System.Collections;
using System.Collections.Generic;
using JM.Rules;
using UnityEngine;

namespace JM.Config
{
    public interface IGameConfigProvider
    {
        int GetCubeCount();
         Sprite[]  GetGameCubeSprites();
         Sprite GetSpriteByID(string id);
        
         GameRule[]  GetGameRules();
    }
}
