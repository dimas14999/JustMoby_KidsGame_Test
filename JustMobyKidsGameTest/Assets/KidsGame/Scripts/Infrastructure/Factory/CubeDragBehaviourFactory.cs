using System;
using KidsGame.Scripts.Core.Application.Drop;
using KidsGame.Scripts.Core.Controller;
using KidsGame.Scripts.Config;
using KidsGame.Scripts.Core.Domain.Placement;
using KidsGame.Scripts.Infrastructure.Services.Animation;
using KidsGame.Scripts.Infrastructure.Services.Coordinates;
using KidsGame.Scripts.Infrastructure.Services.Feedback;
using KidsGame.Scripts.Infrastructure.Services.Tower;
using KidsGame.Scripts.View;
using UnityEngine;

namespace KidsGame.Scripts.Infrastructure.Factory
{
    public class CubeDragBehaviourFactory : ICubeDragBehaviourFactory
    {
        private readonly IActionFeedbackService _feedbackService;
        private readonly IPlacementPolicy _placementPolicy;
        private readonly ITowerCollapseAnimationService _collapseAnimationService;
        private readonly IHoleFallAnimationService _holeFallAnimationService;
        private readonly ICubePlaceAnimationService _placeAnimationService;
        private readonly ICubeMissAnimationService _missAnimationService;
        private readonly IDragCoordinateService _dragCoordinateService;
        private readonly IZoneClampService _zoneClampService;
        private readonly ITowerQueryService _towerQueryService;
        private readonly IDropDecisionService _dropDecisionService;
        private readonly DragRulesConfig _dragRulesConfig;

        public CubeDragBehaviourFactory(
            IActionFeedbackService feedbackService,
            IPlacementPolicy placementPolicy,
            ITowerCollapseAnimationService collapseAnimationService,
            IHoleFallAnimationService holeFallAnimationService,
            ICubePlaceAnimationService placeAnimationService,
            ICubeMissAnimationService missAnimationService,
            IDragCoordinateService dragCoordinateService,
            IZoneClampService zoneClampService,
            ITowerQueryService towerQueryService,
            IDropDecisionService dropDecisionService,
            DragRulesConfig dragRulesConfig)
        {
            _feedbackService = feedbackService;
            _placementPolicy = placementPolicy;
            _collapseAnimationService = collapseAnimationService;
            _holeFallAnimationService = holeFallAnimationService;
            _placeAnimationService = placeAnimationService;
            _missAnimationService = missAnimationService;
            _dragCoordinateService = dragCoordinateService;
            _zoneClampService = zoneClampService;
            _towerQueryService = towerQueryService;
            _dropDecisionService = dropDecisionService;
            _dragRulesConfig = dragRulesConfig;
        }

        public CubeDragBehaviour CreateForNew(CubeView cube, RectTransform zone, HoleZoneView holeZone, Action onStateChanged) => 
            CreateInternal(cube, zone, holeZone, onStateChanged, true);

        public CubeDragBehaviour CreateForExisting(CubeView cube, RectTransform zone, HoleZoneView holeZone, Action onStateChanged) => 
            CreateInternal(cube, zone, holeZone, onStateChanged, false);

        private CubeDragBehaviour CreateInternal(CubeView cube, RectTransform zone, HoleZoneView holeZone, Action onStateChanged, bool createdFromPalette)
        {
            var drag = cube.GetComponent<CubeDragBehaviour>();
            if (drag == null)
                drag = cube.gameObject.AddComponent<CubeDragBehaviour>();

            drag.Init(cube, zone, holeZone, _feedbackService, _placementPolicy, _collapseAnimationService,
                _holeFallAnimationService, _placeAnimationService, _missAnimationService, _dragCoordinateService,
                _zoneClampService, _towerQueryService,
                _dropDecisionService,
                _dragRulesConfig,
                onStateChanged, createdFromPalette);
            return drag;
        }
    }
}

