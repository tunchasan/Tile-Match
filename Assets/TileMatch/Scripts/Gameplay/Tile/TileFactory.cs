using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using TileMatch.Scripts.Utils;

namespace TileMatch.Scripts.Gameplay.Tile
{
    public class TileContext
    {
        public bool Enabled;
        public StandardTile Entity;
    }

    public class TileFactory : Singleton<TileFactory>
    {
        [field: SerializeField] public StandardTile TilePrefab { get; private set; }
        [field: SerializeField] public TileConfig TileConfig { get; private set; }
        public TileTypeSelector TileTypeSelector { get; private set; } = new();
        
        private readonly Dictionary<int, TileContext> _tiles = new();

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        public StandardTile GetTile(List<TileType> tileFilter)
        {
            var tileType = TileTypeSelector.GetTile(tileFilter);
            var unusedTile = _tiles.FirstOrDefault(tile => !tile.Value.Enabled).Value;

            if (unusedTile != null)
            {
                unusedTile.Enabled = true;
                unusedTile.Entity.gameObject.SetActive(true);
                
                // Initialize the Tile with configs
                unusedTile.Entity.Init(tileType, TileConfig.Get(tileType).sprite);
                TileTypeSelector.AddTile(tileType);
                return unusedTile.Entity;
            }

            var newTileId = _tiles.Count;
            var newTile = Instantiate(TilePrefab, transform);
            
            _tiles.Add(newTileId , new TileContext
            {
                Enabled = true,
                Entity = newTile
            });
            
            // Initialize the Tile
            newTile.Init(newTileId, tileType, TileConfig.Get(tileType).sprite);
            TileTypeSelector.AddTile(tileType);
            return newTile;
        }

        public void DestroyTile(StandardTile standardTile)
        {
            var tileContext = _tiles[standardTile.Id];
            
            if (tileContext != null)
            {
                tileContext.Enabled = false;
                tileContext.Entity.SetParent(transform);
                tileContext.Entity.ResetTransform();
                tileContext.Entity.SetInteractionInEditMode(true);
                tileContext.Entity.gameObject.SetActive(false);
                TileTypeSelector.RemoveTile(tileContext.Entity.Type);
            }

            else
            {
                Debug.LogError($"TileFactory::DestroyTile {standardTile.name}_{standardTile.Id} not found in factory!");
            }
        }

        public void Clear()
        {
            foreach (var tilesContext in _tiles.Values)
            {
                DestroyTile(tilesContext.Entity);
            }
        }

        public StandardTile[] GetActiveTiles()
        {
            return _tiles
                .Where(t => t.Value.Enabled)
                .Select(t => t.Value.Entity).ToArray();
        }
    }
}