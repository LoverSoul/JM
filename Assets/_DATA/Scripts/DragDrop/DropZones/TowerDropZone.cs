using System.Collections.Generic;
using JM.Views;
using UnityEngine;

namespace JM.DragDrop
{
    public class TowerDropZone : DropZone, IDropZone
    {
        public int CubeCount => _cubes?.Count ?? 0;
        public IReadOnlyList<CubeView> Cubes => _cubes;
        
        private List<CubeView> _cubes = new List<CubeView>();
        
        public List<CubeView> GetCubesOnTopOf(CubeView cube)
        {
            List<CubeView> result = new List<CubeView>();

            int index = _cubes.IndexOf(cube);
            if (index < 0)
                return result;

            for (int i = index + 1; i < _cubes.Count; i++)
            {
                result.Add(_cubes[i]);
            }

            return result;
        }


        public bool ContainsCube(CubeView cube)
        {
            return _cubes.Contains(cube);
        }

        public void AddCubeOnTop(CubeView cube)
        {
            _cubes.Add(cube);
        }

        public void RemoveCube(CubeView cube)
        {
            _cubes.Remove(cube);
        }

        public CubeView GetTopCube()
        {
            if (_cubes == null || _cubes.Count == 0)
                return null;

            return _cubes[_cubes.Count - 1];
        }
        
    }
}