using UnityEngine;
using KidsGame.Scripts.View;

namespace KidsGame.Scripts.Infrastructure.Services.Feedback
{
    public interface IActionFeedbackService
    {
        void Show(string localizationKey, Color color);
        void SetActionTextView(ActionTextView actionTextView);
    }
}

