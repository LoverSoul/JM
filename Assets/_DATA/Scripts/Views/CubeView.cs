using System.Collections;
using System.Collections.Generic;
using JM.Models;
using UnityEngine;
using UnityEngine.UI;

namespace JM.Views
{
    public class CubeView : MonoBehaviour, ICubeView
    {
        public CubeModel Model { get;  private set; }
        public RectTransform RectTransform => (RectTransform)transform;
        
        [SerializeField] private Image _image;
        
        public void SetModel(CubeModel model)
        {
            Model = model;
        }
        
        public void SetSprite(Sprite sprite)
        {
            _image.sprite = sprite;
        }

    }
}
