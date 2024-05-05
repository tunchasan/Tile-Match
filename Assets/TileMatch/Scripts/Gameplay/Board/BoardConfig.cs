using UnityEngine;

namespace TileMatch.Scripts.Gameplay.Board
{
    [CreateAssetMenu(menuName = "Board", fileName = "Config")]
    public class BoardConfig : ScriptableObject
    {
        [field: SerializeField] public Grid.StandardGrid StandardGrid { get; private set; }
        [field: SerializeField] public Vector2 Dimension { get; private set; }
    }
}