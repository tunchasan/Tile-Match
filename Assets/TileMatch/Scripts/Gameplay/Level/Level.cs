using UnityEngine;
using System.Linq;
using EditorAttributes;
using System.Collections.Generic;
using TileMatch.Scripts.Core.NotifySystem;

namespace TileMatch.Scripts.Gameplay.Level
{
    public class Level : MonoBehaviour
    {
        [field: SerializeField, ReadOnly] private List<Tile.Tile> tiles;

        #if UNITY_EDITOR
        public void Init()
        {
            tiles = GetComponentsInChildren<Tile.Tile>().ToList();
        }
        #endif

        private void OnLevelStateChanged(Tile.Tile tile)
        {
            LevelUtility.RefreshLevel(tiles.Except(new []{tile}).ToArray());
        }

        private void OnMatchReceived(List<Tile.Tile> matchedTiles)
        {
            tiles = tiles.Except(matchedTiles).ToList();
        }

        private void OnEnable()
        {
            NotificationCenter.AddObserver<Tile.Tile>(NotificationTag.OnTilePlacedToSlot, OnLevelStateChanged);
            NotificationCenter.AddObserver<List<Tile.Tile>>(NotificationTag.OnTilesMatched, OnMatchReceived);
        }

        private void OnDisable()
        {
            NotificationCenter.RemoveObserver<Tile.Tile>(NotificationTag.OnTilePlacedToSlot, OnLevelStateChanged);
            NotificationCenter.RemoveObserver<List<Tile.Tile>>(NotificationTag.OnTilesMatched, OnMatchReceived);
        }
    }
}