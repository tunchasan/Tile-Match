using System;
using UnityEngine;
using EditorAttributes;
using TileMatch.Scripts.Managers;
using UnityEngine.AddressableAssets;
using TileMatch.Scripts.Core.NotifySystem;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace TileMatch.Scripts.Gameplay.Level
{
    public class LevelManager : MonoBehaviour
    {
        [field: SerializeField] public Transform LevelSocket { get; private set; }
        [field: SerializeField, ReadOnly] public Level CurrentLevel { get; private set; }
        [field: SerializeField, ReadOnly] public int LevelPointer { get; private set; }
    
        private AsyncOperationHandle<GameObject> _levelHandle;


        private void Init()
        {
            // Loads current level index
            LevelPointer = SaveLoadSystem.LoadLevelPointer();
            LoadLevelAsync(LevelPointer);
        }
        
        private void LoadNextLevelAsync()
        {
            // Iterates level to next
            LevelPointer = SaveLoadSystem.SaveLevelPointer(LevelPointer + 1);
            
            // Starts level loading process
            UnloadLevel();
            LoadLevelAsync(LevelPointer);
        }

        private void RetryLevelAsync()
        {
            UnloadLevel();
            LoadLevelAsync(LevelPointer);
        }
        
        private async void LoadLevelAsync(int levelIndex)
        {
            if (levelIndex < 0)
            {
                Debug.LogError("LevelManager::LoadLevelAsync Failed to load any level. Check Addressable 'Level' Group.");
                return;
            }

            var targetLevelIndex = levelIndex + 1;
            var levelAddress = $"Level_{targetLevelIndex}";        
            
            try
            {
                Debug.Log($"LevelManager::LoadLevelAsync Attempting to load {levelAddress}");

                // Checks whether level address is valid or not
                var result = await LevelUtility.IsLevelAddressValid(levelAddress);

                if (!result)
                {
                    Debug.Log($"LevelManager::LoadLevelAsync Addressable Asset not Found! Level_{targetLevelIndex}");
                    Debug.Log($"LevelManager::LoadLevelAsync Load Failed! {levelAddress}");
                    Debug.Log("LevelManager::LoadLevelAsync Retry level load process...");
                    
                    // Retry for loading level
                    LoadLevelAsync(levelIndex - 1);
                    
                    return;
                }
                
                // Attempting to load level
                _levelHandle = Addressables.LoadAssetAsync<GameObject>(levelAddress);
                
                await _levelHandle.Task;
                
                // Checks result and continue to the level loading process
                if (_levelHandle.Status == AsyncOperationStatus.Succeeded)
                {
                    var instance = Instantiate(_levelHandle.Result, LevelSocket);
                    CurrentLevel = instance.GetComponent<Level>();
                    CurrentLevel.Init();
                    
                    NotificationCenter.PostNotification(NotificationTag.OnLevelLoaded, LevelPointer + 1);
                    Debug.Log($"LevelManager::LoadLevelAsync Level_{targetLevelIndex} Loaded!");
                }
            }
            
            catch (Exception ex)
            {
                Debug.LogError($"LevelManager::LoadLevelAsync Something went wrong during level loading process. {ex.GetType()}: {ex.Message}");
                throw;
            }
        }

        private void UnloadLevel()
        {
            if (CurrentLevel != null)
            {
                Destroy(CurrentLevel.gameObject);
                CurrentLevel = null;
            }

            if (_levelHandle.IsValid())
            {
                Addressables.Release(_levelHandle);
            }

            Debug.Log("LevelManager::UnloadLevel Level unloaded and resources cleaned up.");
        }

        private void OnEnable()
        {
            NotificationCenter.AddObserver(NotificationTag.OnGameStart, Init);
            NotificationCenter.AddObserver(NotificationTag.OnRequestReloadLevel, RetryLevelAsync);
            NotificationCenter.AddObserver(NotificationTag.OnRequestLoadNextLevel, LoadNextLevelAsync);
        }

        private void OnDisable()
        {
            NotificationCenter.RemoveObserver(NotificationTag.OnGameStart, Init);
            NotificationCenter.RemoveObserver(NotificationTag.OnRequestReloadLevel, RetryLevelAsync);
            NotificationCenter.RemoveObserver(NotificationTag.OnRequestLoadNextLevel, LoadNextLevelAsync);
        }
    }
}