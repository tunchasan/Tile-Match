using System;
using UnityEngine;
using EditorAttributes;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TileMatch.Scripts.Gameplay.Grid;
using TileMatch.Scripts.Gameplay.Tile;
using TileMatch.Scripts.Core.NotifySystem;
using TileMatch.Scripts.Utils;

namespace TileMatch.Scripts.Gameplay.Level
{
    public enum LevelStateChangeReason
    {
        Add,
        Remove,
        Randomize
    }

    [Serializable]
    public class LevelElementContext
    {
        public int id;
        public StandardTile tile;
        public StandardGrid relatedGrid;
    }
    
    public class Level : MonoBehaviour
    {
        private Dictionary<int, LevelElementContext> LevelCache { get; set; } = new ();
        [field: SerializeField, ReadOnly] private List<StandardTile> Tiles { get; set; } = new ();
        [field: SerializeField, ReadOnly] private List<StandardGrid> Grids { get; set; } = new ();

        public int ActiveTiles => Tiles.Count;
        
        public void Init()
        {
            ProcessLevelState();
        }

        private void ProcessLevelState()
        {
            foreach (var grid in GetComponentsInChildren<StandardGrid>())
            {
                var relatedTile = grid.GetComponentInChildren<StandardTile>();
                if (relatedTile != null)
                {
                    grid.Fill(relatedTile, false);
                    
                    LevelCache.Add(relatedTile.Id, new LevelElementContext
                    {
                        id = relatedTile.Id,
                        tile = relatedTile,
                        relatedGrid = grid
                    });

                    Grids.Add(grid);
                    Tiles.Add(relatedTile);
                }
            }
            
            NotificationCenter.PostNotification(NotificationTag.OnLevelStateProcessed, ActiveTiles);
        }
        
        private async void RandomizeLevelState()
        {
            NotificationCenter.PostNotification(NotificationTag.OnActionProcess);
            
            foreach (var grid in Grids)
            {
                grid.Clear();
            }
            
            foreach (var tile in Tiles)
            {
                LevelCache.Remove(tile.Id);
                tile.SetInteraction(false);
            }
            
            Utilities.Shuffle(Grids);

            for (var i = 0; i < Grids.Count; i++)
            {
                if (i < Tiles.Count)
                {
                    Grids[i].Fill(Tiles[i]);
                    
                    LevelCache.Add(Tiles[i].Id, new LevelElementContext
                    {
                        id = Tiles[i].Id,
                        tile = Tiles[i],
                        relatedGrid = Grids[i]
                    });
                }
            }

            await UniTask.Delay(300);
            OnLevelStateChanged(LevelStateChangeReason.Randomize, null);
            NotificationCenter.PostNotification(NotificationTag.OnActionProcessComplete);
        }

        private void AddTile(StandardTile tile)
        {
            Tiles.Add(tile);
            var relatedGrid = LevelCache[tile.Id].relatedGrid;
            relatedGrid.Fill(tile);
            if(Grids.Contains(relatedGrid)) return;
            Grids.Add(relatedGrid);
        }

        private void RemoveTile(StandardTile tile)
        {
            Tiles.Remove(tile);
            var relatedGrid = LevelCache[tile.Id].relatedGrid;
            Grids.Remove(relatedGrid);
            relatedGrid.Clear();
        }
        
        private void RemoveRangeTile(List<StandardTile> tiles)
        {
            foreach (var tile in tiles)
            {
                RemoveTile(tile);
            }
        }

        private async void OnLevelStateChanged(LevelStateChangeReason reason, StandardTile standardTile)
        {
            switch (reason)
            {
                case LevelStateChangeReason.Add:
                    AddTile(standardTile);
                    await UniTask.Delay(250);
                    LevelUtility.RefreshLevel(Tiles.ToArray());
                    return;
                case LevelStateChangeReason.Remove:
                    RemoveTile(standardTile);
                    LevelUtility.RefreshLevel(Tiles.ToArray());
                    return;
                case LevelStateChangeReason.Randomize:
                    LevelUtility.RefreshLevel(Tiles.ToArray());
                    return;
                default:
                    throw new ArgumentOutOfRangeException(nameof(reason), reason, null);
            }
        }

        private void OnMatchReceived(List<StandardTile> matchedTiles)
        {
            RemoveRangeTile(matchedTiles);
        }
        
        private void OnTilePressed(StandardTile tile)
        {
            var relatedGrid = LevelCache[tile.Id].relatedGrid;
            if(relatedGrid.IsEmpty()) return;
            relatedGrid.Highlight();
        }

        private void OnTileReleased(StandardTile tile)
        {
            var relatedGrid = LevelCache[tile.Id].relatedGrid;
            if(relatedGrid.IsEmpty()) return;
            relatedGrid.ClearHighlight();
        }

        private void OnEnable()
        {
            NotificationCenter.AddObserver<StandardTile>(NotificationTag.OnTilePress, OnTilePressed);
            NotificationCenter.AddObserver<StandardTile>(NotificationTag.OnTileRelease, OnTileReleased);
            NotificationCenter.AddObserver(NotificationTag.OnRandomizeBoardAction, RandomizeLevelState);
            NotificationCenter.AddObserver<List<StandardTile>>(NotificationTag.OnTilesMatched, OnMatchReceived);
            NotificationCenter.AddObserver<LevelStateChangeReason, StandardTile>(NotificationTag.OnTilePlacedToSlot, OnLevelStateChanged);
            NotificationCenter.AddObserver<LevelStateChangeReason, StandardTile>(NotificationTag.OnReverseActionCompleted, OnLevelStateChanged);
        }

        private void OnDisable()
        {
            NotificationCenter.RemoveObserver<StandardTile>(NotificationTag.OnTilePress, OnTilePressed);
            NotificationCenter.RemoveObserver<StandardTile>(NotificationTag.OnTileRelease, OnTileReleased);
            NotificationCenter.RemoveObserver(NotificationTag.OnRandomizeBoardAction, RandomizeLevelState);
            NotificationCenter.RemoveObserver<List<StandardTile>>(NotificationTag.OnTilesMatched, OnMatchReceived);
            NotificationCenter.RemoveObserver<LevelStateChangeReason, StandardTile>(NotificationTag.OnTilePlacedToSlot, OnLevelStateChanged);
            NotificationCenter.RemoveObserver<LevelStateChangeReason, StandardTile>(NotificationTag.OnReverseActionCompleted, OnLevelStateChanged);
        }
    }
}