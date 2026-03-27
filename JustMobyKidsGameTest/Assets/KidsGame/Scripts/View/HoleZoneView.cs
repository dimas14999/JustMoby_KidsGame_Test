using UnityEngine;

namespace KidsGame.Scripts.View
{
    public class HoleZoneView : MonoBehaviour
    {
        [SerializeField] private RectTransform _zone = null!;

        public RectTransform Zone => _zone;

        private void Awake()
        {
            if (_zone == null)
                _zone = GetComponent<RectTransform>();
        }

        private void OnValidate()
        {
            if (_zone == null)
                _zone = GetComponent<RectTransform>();
        }
    }
}

