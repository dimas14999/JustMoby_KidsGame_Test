using System;
using System.Linq;
using KidsGame.Scripts.Config;
using KidsGame.Scripts.Infrastructure.Factory;
using KidsGame.Scripts.Infrastructure.Services.Save;
using KidsGame.Scripts.View;
using UnityEngine;

namespace KidsGame.Scripts.Infrastructure.Services.State
{
    public class TowerStateService : ITowerStateService
    {
        private readonly BoxConfig _boxConfig;
        private readonly IGameSaveService _saveService;

        public TowerStateService(BoxConfig boxConfig, IGameSaveService saveService)
        {
            _boxConfig = boxConfig;
            _saveService = saveService;
        }

        public void Save(RectTransform zone)
        {
            var save = new GameSaveData();
            for (var i = 0; i < zone.childCount; i++)
            {
                var cube = zone.GetChild(i).GetComponent<CubeView>();
                if (cube == null) continue;

                save.Cubes.Add(new SavedCubeData
                {
                    Id = cube.Id,
                    Color = cube.CubeColor,
                    Position = cube.Rect.anchoredPosition
                });
            }

            _saveService.Save(save);
        }

        public void Restore(RectTransform zone, ICubeFactory cubeFactory, Action<CubeView> onCubeRestored)
        {
            var save = _saveService.Load();
            if (save == null || save.Cubes == null || save.Cubes.Count == 0) return;

            foreach (var saved in save.Cubes)
            {
                var data = _boxConfig.BoxDatas.FirstOrDefault(x => x.Id != null && x.Id == saved.Id)
                           ?? _boxConfig.BoxDatas.FirstOrDefault();
                if (data == null) continue;

                var cube = cubeFactory.Create(zone, data);
                cube.Rect.anchoredPosition = saved.Position;
                onCubeRestored?.Invoke(cube);
            }
        }
    }
}

