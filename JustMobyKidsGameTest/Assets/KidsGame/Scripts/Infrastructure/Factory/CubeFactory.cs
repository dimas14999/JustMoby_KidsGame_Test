using KidsGame.Scripts.Config;
using KidsGame.Scripts.View;
using UnityEngine;
using Zenject;

namespace KidsGame.Scripts.Infrastructure.Factory
{
    public class CubeFactory : ICubeFactory
    {
        private readonly DiContainer _container;
        private readonly CubeView _prefab;
        
        public CubeFactory(DiContainer container, CubeView prefab)
        {
            _container = container;
            _prefab = prefab;
        }
        
        public CubeView Create(Transform parent, BoxData data)
        {
            var cube = _container.InstantiatePrefabForComponent<CubeView>(_prefab, parent);
            cube.Init(data);
            return cube;
        }
    }
}
