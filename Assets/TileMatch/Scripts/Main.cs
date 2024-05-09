using TileMatch.Scripts.Gameplay.Level;
using TileMatch.Scripts.Gameplay.Slot;
using TileMatch.Scripts.Managers;
using TileMatch.Scripts.Utils;
using UnityEngine;

namespace TileMatch.Scripts
{
    public class Main : Singleton<Main>
    {
        [field: SerializeField] public LevelManager LevelManager { get; private set; }
        [field: SerializeField] public SlotController SlotController { get; private set; }
        [field: SerializeField] public GameManager GameManager { get; private set; }

        private void Start()
        {
            GameManager.StartGame();
        }
    }
}