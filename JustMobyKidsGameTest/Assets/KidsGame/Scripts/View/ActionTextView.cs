using DG.Tweening;
using TMPro;
using UnityEngine;

namespace KidsGame.Scripts.View
{
    public class ActionTextView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text = null!;
        [SerializeField] private float _showDuration;
        [SerializeField] private float _holdDuration;
        [SerializeField] private float _hideDuration;

        private Sequence _sequence;

        public void ShowAction(string message, Color color)
        {
            if (_text == null) return;

            _sequence?.Kill();
            _text.text = message;
            _text.color = new Color(color.r, color.g, color.b, 0f);
            _text.rectTransform.localScale = Vector3.one * 0.85f;

            _sequence = DOTween.Sequence();
            _sequence.Append(_text.DOFade(1f, _showDuration).SetEase(Ease.OutQuad));
            _sequence.Join(_text.rectTransform.DOScale(Vector3.one, _showDuration).SetEase(Ease.OutBack));
            _sequence.AppendInterval(_holdDuration);
            _sequence.Append(_text.DOFade(0f, _hideDuration).SetEase(Ease.InQuad));
        }
    }
}

