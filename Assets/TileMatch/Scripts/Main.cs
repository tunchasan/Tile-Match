using TileMatch.Scripts.Gameplay.Tile;
using TileMatch.Scripts.Utils;
using UnityEngine;

namespace TileMatch.Scripts
{
    public class Main : Singleton<Main>
    {
        [field: SerializeField] public TileFactory TileFactory { get; private set; }
    }
}