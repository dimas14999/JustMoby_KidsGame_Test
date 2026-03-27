using System;
using System.Collections.Generic;
using UnityEngine;

namespace KidsGame.Scripts.Config
{
    [CreateAssetMenu(fileName = "BoxConfig", menuName = "Config/BoxConfig")]
    public class BoxConfig : ScriptableObject
    {
        [field: SerializeField] public List<BoxData> BoxDatas { get; private set; }
    }

    [Serializable]
    public class BoxData
    {
        public string Id;
        public Color Color;
        public Sprite Sprite;
    }
}
