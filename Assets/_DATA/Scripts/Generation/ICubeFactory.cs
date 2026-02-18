using System.Collections.Generic;
using JM.Models;
using JM.Views;

namespace JM.Generation
{
    public interface ICubeFactory
    {
        CubeView Create(CubeModel model);
    }
}