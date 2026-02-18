using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JM.Rules;
using UnityEngine;

namespace JM.Config
{
    public class SOGameConfigProvider : IGameConfigProvider
    {
        private readonly SOGameConfig _config;

        public SOGameConfigProvider(SOGameConfig config)
        {
            _config = config;
        }

        public int GetCubeCount()
        {
            return _config.cubeCount;
        }

        public Sprite[] GetGameCubeSprites()
        {
            return _config.sprites;
        }
        
        public Sprite GetSpriteByID(string id)
        {
            return _config.sprites.First(x => x.name == id);
        }

        public GameRule[] GetGameRules()
        {
            return _config.rules;
        }
    }
}
