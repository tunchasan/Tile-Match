using TileMatch.Scripts.Gameplay.Level;
using TileMatch.Scripts.Utils;
using UnityEngine;

namespace TileMatch.Scripts
{
    public class Main : Singleton<Main>
    {
        [field: SerializeField] public LevelManager LevelManager { get; private set; }
    }
}