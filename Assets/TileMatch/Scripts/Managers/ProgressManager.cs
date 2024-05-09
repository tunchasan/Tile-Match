using UnityEngine;
using System.Collections.Generic;
using TileMatch.Scripts.Gameplay.Tile;
using TileMatch.Scripts.Core.NotifySystem;

namespace TileMatch.Scripts.Managers
{
    public class ProgressManager : MonoBehaviour
    {
        private int _totalTileCount;

        private void Init(int initialTileCount)
        {
            _totalTileCount = initialTileCount;
        }

        private void OnProgressChanged(List<StandardTile> removedTiles)
        {
            var totalTilesInBoard = Main.Instance.LevelManager.CurrentLevel.ActiveTiles;
            var totalTilesInSlots = Main.Instance.SlotController.ActiveTiles;
            var progress = 1F - (float)(totalTilesInBoard + totalTilesInSlots - removedTiles.Count) / _totalTileCount;
            NotificationCenter.PostNotification(NotificationTag.OnLevelProgressChanged, progress);
        }

        private void OnEnable()
        {
            NotificationCenter.AddObserver<int>(NotificationTag.OnLevelStateProcessed, Init);
            NotificationCenter.AddObserver<List<StandardTile>>(NotificationTag.OnTilesMatched, OnProgressChanged);
        }

        private void OnDisable()
        {
            NotificationCenter.RemoveObserver<int>(NotificationTag.OnLevelStateProcessed, Init);
            NotificationCenter.RemoveObserver<List<StandardTile>>(NotificationTag.OnTilesMatched, OnProgressChanged);
        }
    }
}