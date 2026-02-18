using System.Collections;
using System.Collections.Generic;
using JM.Saves;
using UnityEngine;

namespace JM.Saves
{
    public class PlayerPrefsSaveService : ISaveService
    {
        private const string KEY = "GAME_PROGRESS";

        public void Save(GameProgressDTO progress)
        {
            var json = JsonUtility.ToJson(progress);
            PlayerPrefs.SetString(KEY, json);
            PlayerPrefs.Save();
        }

        public GameProgressDTO Load()
        {
            if (!PlayerPrefs.HasKey(KEY))
                return null;

            var json = PlayerPrefs.GetString(KEY);
            return JsonUtility.FromJson<GameProgressDTO>(json);
        }
    }
}
