using UnityEngine;
using System;

namespace KidsGame.Scripts.Infrastructure.Services.Animation
{
    public interface ITowerCollapseAnimationService
    {
        void CollapseAfterRemoval(RectTransform zone, Transform removedTransform, float removedCenterY, float removedHeight, Action onComplete);
    }
}

