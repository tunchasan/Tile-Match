using DG.Tweening;
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

        private void Awake()
        {
            // Initialize DOTween with recycling and capacity settings enabled, and only log errors.
            DOTween.Init(true, false, LogBehaviour.ErrorsOnly);
            
            // Set the target frame rate for mobile devices
            Application.targetFrameRate = 60;
        }
        
        private void Start()
        {
            GameManager.StartGame();
        }
    }
}