using KidsGame.Scripts.View;
using UnityEngine;

namespace KidsGame.Scripts.Infrastructure.Services.Tower
{
    public interface ITowerQueryService
    {
        bool TryGetTopCube(RectTransform zone, Transform excluded, out CubeView topCube);
        float ResolveCubeHeight(RectTransform zone, CubeView currentCube);
    }
}

