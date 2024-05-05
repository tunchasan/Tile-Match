using System;
using System.Linq;
using UnityEngine;

namespace TileMatch.Scripts.Gameplay.Tile
{
    [Serializable]
    public class TileConfigContext
    {
        public string title;
        public TileType type;
        public Sprite sprite;
    }
    
    [CreateAssetMenu(menuName = "Tile", fileName = "TileConfig")]
    public class TileConfig : ScriptableObject
    {
        [field: SerializeField] public TileConfigContext[] Contexts { get; private set; }

        public TileConfigContext Get(TileType type)
        {
            return Contexts.First(t => t.type == type);
        }
    }
}