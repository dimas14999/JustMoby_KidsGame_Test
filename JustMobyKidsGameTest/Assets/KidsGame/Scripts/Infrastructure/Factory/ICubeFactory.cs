using KidsGame.Scripts.Config;
using KidsGame.Scripts.View;
using UnityEngine;

namespace KidsGame.Scripts.Infrastructure.Factory
{
    public interface ICubeFactory
    {
        CubeView Create(Transform parent, BoxData data);
    }
}
