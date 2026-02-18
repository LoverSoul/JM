using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JM.Models
{
    public readonly struct CubeModel
    {
        public string NameID { get; }

        public CubeModel(string nameID)
        {
            NameID = nameID;
        }

    }
}
