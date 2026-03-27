using KidsGame.Scripts.Config;
using UnityEngine;
using UnityEngine.UI;

namespace KidsGame.Scripts.View
{
    public class CubeView : MonoBehaviour
    {
        [SerializeField] private RectTransform _rect = null!;
        [SerializeField] private Image _image = null!;
        
        private Color _cubeColor = Color.white;
        private string _id;

        public RectTransform Rect => _rect;
        public Color CubeColor => _cubeColor;
        public string Id => _id;

        public void Init(BoxData data)
        {
            _id = data.Id;
            _image.sprite = data.Sprite;
            _cubeColor = data.Color;

            if (_rect != null)
            {
                _rect.anchorMin = _rect.anchorMax = new Vector2(0.5f, 0.5f);
                _rect.pivot = new Vector2(0.5f, 0.5f);
            }

            if (data.Sprite != null && (_rect.rect.width <= 0.001f || _rect.rect.height <= 0.001f))
            {
                var spriteSize = new Vector2(data.Sprite.rect.width, data.Sprite.rect.height);
                _rect.sizeDelta = spriteSize;
                if (_image != null && _image.rectTransform != null)
                {
                    _image.rectTransform.anchorMin = _image.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                    _image.rectTransform.pivot = new Vector2(0.5f, 0.5f);
                    _image.rectTransform.sizeDelta = spriteSize;
                }
            }

            _image.raycastTarget = true;
        }
    }
}
