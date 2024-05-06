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

        public void Init()
        {
            tiles = GetComponentsInChildren<Tile.Tile>().ToList();
        }

        private void OnLevelStateChanged(Tile.Tile tile)
        {
            LevelUtility.RefreshLevel(tiles.Except(new []{tile}).ToArray());
        }

        private void OnEnable()
        {
            NotificationCenter.AddObserver<Tile.Tile>(NotificationTag.OnTilePlacedToSlot, OnLevelStateChanged);
        }

        private void OnDisable()
        {
            NotificationCenter.RemoveObserver<Tile.Tile>(NotificationTag.OnTilePlacedToSlot, OnLevelStateChanged);
        }
    }
}