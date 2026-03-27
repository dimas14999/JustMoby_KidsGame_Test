using KidsGame.Scripts.Infrastructure.Services.Localization;
using KidsGame.Scripts.View;
using UnityEngine;
using Zenject;

namespace KidsGame.Scripts.Infrastructure.Services.Feedback
{
    public class TweenActionFeedbackService : IActionFeedbackService
    {
        private readonly ILocalizationService _localizationService;
        private ActionTextView _actionTextView;

        public TweenActionFeedbackService(ILocalizationService localizationService)
        {
            _localizationService = localizationService;
        }

        public void SetActionTextView(ActionTextView actionTextView)
        {
            _actionTextView = actionTextView;
        }

        public void Show(string localizationKey, Color color)
        {
            if (_actionTextView == null) return;
            _actionTextView.ShowAction(_localizationService.Get(localizationKey), color);
        }
    }
}

