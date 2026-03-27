using KidsGame.Scripts.Config;
using KidsGame.Scripts.View;
using UnityEngine;

namespace KidsGame.Scripts.Infrastructure.Factory
{
    public interface IUICubeFactory
    {
        UICubeView Create(Transform parent, BoxData data);
    }
}
