using KidsGame.Scripts.Config;
using KidsGame.Scripts.View;
using UnityEngine;
using Zenject;

namespace KidsGame.Scripts.Infrastructure.Factory
{
    public class UiCubeFactory : IUICubeFactory
    {
        private readonly DiContainer _container;
        private readonly UICubeView _prefab;
        
        public UiCubeFactory(DiContainer container, UICubeView prefab)
        {
            _container = container;
            _prefab = prefab;
        }
        
        public UICubeView Create(Transform parent, BoxData data)
        {
            var cube = _container.InstantiatePrefabForComponent<UICubeView>(_prefab, parent);
            cube.Init(data);
            return cube;
        }
        
    }
}
