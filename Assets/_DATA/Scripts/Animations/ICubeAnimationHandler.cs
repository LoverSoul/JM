using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using JM.Views;
using UnityEngine;

namespace JM.Animations
{
    public interface ICubeAnimationHandler
    {
        Task FallAsync(CubeView cub,Vector3 position);
        Task ExplodeAsync(CubeView cube);
        Task FallAndExplodeAsync(CubeView cub,Vector3 position);
        Task JumpAsync(CubeView cube);
        void Reset(CubeView cube);
    }
}
