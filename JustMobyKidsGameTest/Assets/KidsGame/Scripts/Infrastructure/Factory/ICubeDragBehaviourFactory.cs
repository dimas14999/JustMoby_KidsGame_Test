using System;
using KidsGame.Scripts.Core.Controller;
using KidsGame.Scripts.View;
using UnityEngine;

namespace KidsGame.Scripts.Infrastructure.Factory
{
    public interface ICubeDragBehaviourFactory
    {
        CubeDragBehaviour CreateForNew(CubeView cube, RectTransform zone, HoleZoneView holeZone, Action onStateChanged);
        CubeDragBehaviour CreateForExisting(CubeView cube, RectTransform zone, HoleZoneView holeZone, Action onStateChanged);
    }
}

