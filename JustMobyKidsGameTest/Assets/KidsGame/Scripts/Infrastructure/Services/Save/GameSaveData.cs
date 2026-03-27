using System;
using System.Collections.Generic;
using UnityEngine;

namespace KidsGame.Scripts.Infrastructure.Services.Save
{
    [Serializable]
    public class GameSaveData
    {
        public List<SavedCubeData> Cubes = new();
    }

    [Serializable]
    public class SavedCubeData
    {
        public string Id;
        public Color Color;
        public Vector2 Position;
    }
}

