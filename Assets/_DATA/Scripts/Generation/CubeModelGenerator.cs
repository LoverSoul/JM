using System.Collections.Generic;
using JM.Config;
using JM.Models;

namespace JM.Generation
{
    public class CubeModelGenerator
    {
        private readonly IGameConfigProvider _config;

        public CubeModelGenerator(IGameConfigProvider config)
        {
            _config = config;
        }

        public List<CubeModel> Generate()
        {
            var result = new List<CubeModel>();

            var sprites = _config.GetGameCubeSprites();
            var count = _config.GetCubeCount();

            if (sprites == null || sprites.Length == 0)
                return result;

            for (int i = 0; i < count; i++)
            {
                int index = i % sprites.Length;
                result.Add(new CubeModel(sprites[index].name));
            }

            return result;
        }
    }
}