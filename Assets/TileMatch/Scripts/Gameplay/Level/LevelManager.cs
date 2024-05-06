using UnityEngine;
using EditorAttributes;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace TileMatch.Scripts.Gameplay.Level
{
    public class LevelManager : MonoBehaviour
    {
        [field: SerializeField] public Transform LevelSocket { get; private set; }
        [field: SerializeField, ReadOnly] public Level CurrentLevel { get; private set; }
        [field: SerializeField, ReadOnly] public int LevelPointer { get; private set; }
    
        private AsyncOperationHandle<GameObject> _levelHandle;

        private void Start()
        {
            LoadLevelAsync();
        }

        private async void LoadLevelAsync()
        {
            _levelHandle = Addressables.LoadAssetAsync<GameObject>($"Level_{LevelPointer + 1}");
            await _levelHandle.Task;

            if (_levelHandle.Status == AsyncOperationStatus.Succeeded)
            {
                var instance = Instantiate(_levelHandle.Result, LevelSocket);
                CurrentLevel = instance.GetComponent<Level>();
            }

            else
            {
                Debug.LogError("LevelManager::LoadLevelAsync Failed to load addressable asset.");
            }
        }

        public void UnloadLevel()
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

            Debug.Log("Level unloaded and resources cleaned up.");
        }
    }
}