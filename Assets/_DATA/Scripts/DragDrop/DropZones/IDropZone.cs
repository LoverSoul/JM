using UnityEngine;

namespace JM.DragDrop
{
    public interface IDropZone
    {
        bool Contains(Vector2 screenPoint);
    }
}