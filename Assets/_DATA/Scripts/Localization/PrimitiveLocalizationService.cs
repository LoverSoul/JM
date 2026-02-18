using System.Collections;
using System.Collections.Generic;
using JM.Localization;
using UnityEngine;

namespace JM.Localization
{
    public class PrimitiveLocalizationService : ILocalizationService
    {
        private readonly Dictionary<string, string> _data = new()
        {
            { "hole_missed", " Cube missed hole!" },
            { "hole_entered", "Cube falling to hole!" },
            { "height_limit", "Height limit reached" },
            { "drag_start", "Drag started!" },
            {"first_cube","First cube placed!" },
            {"too_low", "Cube too low!" },
            {"cube_missed", "Cube missed!" },
            {"cube_placed", "Cube placed!" },
        };

        public string Get(string key)
        {
            return _data.TryGetValue(key, out var value)
                ? value
                : key;
        }
    }
}
