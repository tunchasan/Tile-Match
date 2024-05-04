using TatedrezGame.Utils;
using TileMatch.Scripts.Gameplay;
using UnityEngine;

namespace TileMatch.Scripts
{
    public class Main : Singleton<Main>
    {
        [field: SerializeField] public TileFactory TileFactory { get; private set; }
    }
}
