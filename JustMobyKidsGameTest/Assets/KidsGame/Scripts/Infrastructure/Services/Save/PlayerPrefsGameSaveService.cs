using UnityEngine;

namespace KidsGame.Scripts.Infrastructure.Services.Save
{
    public class PlayerPrefsGameSaveService : IGameSaveService
    {
        private const string SaveKey = "kids_game_save";

        public void Save(GameSaveData data)
        {
            var json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(SaveKey, json);
            PlayerPrefs.Save();
        }

        public GameSaveData Load()
        {
            if (!PlayerPrefs.HasKey(SaveKey))
                return new GameSaveData();

            var json = PlayerPrefs.GetString(SaveKey);
            if (string.IsNullOrEmpty(json))
                return new GameSaveData();

            return JsonUtility.FromJson<GameSaveData>(json) ?? new GameSaveData();
        }
    }
}

