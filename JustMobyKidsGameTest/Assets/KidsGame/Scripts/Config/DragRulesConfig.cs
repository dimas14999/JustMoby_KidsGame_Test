using UnityEngine;

namespace KidsGame.Scripts.Config
{
    [CreateAssetMenu(fileName = "DragRulesConfig", menuName = "Config/DragRulesConfig")]
    public class DragRulesConfig : ScriptableObject
    {
        [field: SerializeField] public float SpawnHoldDelaySeconds { get; private set; }
        [field: SerializeField] public float StackSnapRadius { get; private set; }
        [field: SerializeField] public float PlacementHeightFactor { get; private set; }
    }
}

