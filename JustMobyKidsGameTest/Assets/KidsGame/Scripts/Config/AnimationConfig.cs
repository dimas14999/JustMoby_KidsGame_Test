using UnityEngine;
using UnityEngine.Serialization;

namespace KidsGame.Scripts.Config
{
    [CreateAssetMenu(fileName = "AnimationConfig", menuName = "Config/AnimationConfig")]
    public class AnimationConfig : ScriptableObject
    {
        [field: Header("Spawn Bounce")] 
        [field: SerializeField] public float SpawnBounceDuration { get; private set; }

        [field: Header("Place Animation")]
        [field: SerializeField] public float PlaceJumpHeight { get; private set; }
        [field: SerializeField] public float PlaceDropDuration { get; private set; }
        [field: SerializeField] public float PlaceReboundDuration { get; private set; }
        [field: SerializeField] public float PlaceTiltAngle { get; private set; }
        [field: SerializeField] public float PlaceReboundOffsetY { get; private set; }

        [field: Header("Miss Animation")]
        [field: SerializeField] public float MissBouncePopDuration { get; private set; }
        [field: SerializeField] public float MissBounceShrinkDuration { get; private set; }
        [field: SerializeField] public float MissBounceScale { get; private set; }

        [field: Header("Hole Fall Animation")]
        [field: SerializeField] public float HoleFallDuration { get; private set; }
        [field: SerializeField] public float HoleDuration { get; private set; }
        [field: SerializeField] public float HoleOffset { get; private set; }
        [field: SerializeField] public float HoleSpinMin { get; private set; }
        [field: SerializeField] public float HoleSpinMax { get; private set; }

        [field: Header("Tower Collapse Animation")]
        [field: SerializeField] public float CollapseDuration { get; private set; }
        [field: SerializeField] public float CollapseStepDelay { get; private set; }
        [field: SerializeField] public float CollapseBounceHeight { get; private set; }
        [field: SerializeField] public float CollapseBounceDuration { get; private set; }
    }
}

