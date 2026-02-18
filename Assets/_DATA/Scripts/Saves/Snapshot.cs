using System.Collections.Generic;

namespace JM.Saves
{
    [System.Serializable]
    public class Snapshot
    {
        public List<CubeSnapshot> Cubes = new();
    }
}