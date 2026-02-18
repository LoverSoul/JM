using System.Collections;
using System.Collections.Generic;
using JM.Models;
using UnityEngine;

namespace JM.Views
{
    public interface ICubeView
    {
        CubeModel Model { get; }
        RectTransform RectTransform { get; }

        void SetSprite(Sprite sprite);
        void SetModel(CubeModel model);
    }
}
