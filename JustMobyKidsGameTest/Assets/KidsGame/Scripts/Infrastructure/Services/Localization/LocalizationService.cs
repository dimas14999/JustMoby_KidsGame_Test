using System.Collections.Generic;

namespace KidsGame.Scripts.Infrastructure.Services.Localization
{
    public class LocalizationService : ILocalizationService
    {
        private readonly Dictionary<string, string> _ru = new()
        {
            [LocalizationKeys.CubePlaced] = "Установили кубик",
            [LocalizationKeys.CubeRemoved] = "Удалили кубик",
            [LocalizationKeys.CubeDestroyedInHole] = "Уничтожили кубик",
            [LocalizationKeys.HeightLimitReached] = "Лимит по высоте"
        };

        public string Get(string key)
        {
            return _ru.TryGetValue(key, out var value) ? value : key;
        }
    }
}

