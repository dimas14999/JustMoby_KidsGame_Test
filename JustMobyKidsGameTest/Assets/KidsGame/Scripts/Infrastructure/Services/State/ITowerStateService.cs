using System;
using KidsGame.Scripts.Infrastructure.Factory;
using KidsGame.Scripts.View;
using UnityEngine;

namespace KidsGame.Scripts.Infrastructure.Services.State
{
    public interface ITowerStateService
    {
        void Save(RectTransform zone);
        void Restore(RectTransform zone, ICubeFactory cubeFactory, Action<CubeView> onCubeRestored);
    }
}

