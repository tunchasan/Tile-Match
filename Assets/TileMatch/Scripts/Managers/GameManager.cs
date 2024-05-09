using UnityEngine;
using EditorAttributes;
using TileMatch.Scripts.Core.NotifySystem;

namespace TileMatch.Scripts.Managers
{
    public enum GameState
    {
        None,
        Start,
        Failed,
        Completed
    }
    
    public class GameManager : MonoBehaviour
    {
        [field: SerializeField, ReadOnly] public GameState GameState { get; private set; } = GameState.None;

        public void StartGame()
        {
            UpdateGameState(GameState.Start);
        }

        private void UpdateGameState(GameState state)
        {
            if (state == GameState) return;
            GameState = state;

            // Specific notification
            if (GameState == GameState.Start)
            {
                NotificationCenter.PostNotification(NotificationTag.OnGameStart);
            }
            
            // Broadcast the game state changes to all listeners
            NotificationCenter.PostNotification(NotificationTag.OnGameStateChanged, GameState);
        }

        private void ResetGameState(int _)
        {
            GameState = GameState.Start;
        }
        
        private void OnLevelProgressChanged(float value)
        {
            if ((int)value != 1) return;
            UpdateGameState(GameState.Completed);
            Debug.LogWarning("GameManager::OnLevelProgressChanged Level Completed!");
        }

        private void OnLevelFailReceived()
        {
            UpdateGameState(GameState.Failed);
            Debug.LogWarning("GameManager::OnLevelFailReceived Level Failed!");
        }
        
        private void OnEnable()
        {
            NotificationCenter.AddObserver<int>(NotificationTag.OnLevelLoaded, ResetGameState);
            NotificationCenter.AddObserver(NotificationTag.AllSlotsFilled, OnLevelFailReceived);
            NotificationCenter.AddObserver<float>(NotificationTag.OnLevelProgressChanged, OnLevelProgressChanged);
        }

        private void OnDisable()
        {
            NotificationCenter.RemoveObserver<int>(NotificationTag.OnLevelLoaded, ResetGameState);
            NotificationCenter.RemoveObserver(NotificationTag.AllSlotsFilled, OnLevelFailReceived);
            NotificationCenter.RemoveObserver<float>(NotificationTag.OnLevelProgressChanged, OnLevelProgressChanged);
        }
    }
}