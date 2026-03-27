using KidsGame.Scripts.Config;
using KidsGame.Scripts.Infrastructure.Factory;
using UnityEngine;
using Zenject;

namespace KidsGame.Scripts.Core.Controller
{
    public class UiBoxController : MonoBehaviour
    {
        [SerializeField] private RectTransform _boxParent = null!;

        [Inject] private BoxConfig _boxConfig;
        [Inject] private IUICubeFactory _cubeFactory;

        private void Start()
        {
            if (_boxParent == null) return;
            if (_boxConfig == null || _boxConfig.BoxDatas == null) return;

            foreach (var box in _boxConfig.BoxDatas)
            {
                _cubeFactory.Create(_boxParent, box);
            }
        }
    }
}