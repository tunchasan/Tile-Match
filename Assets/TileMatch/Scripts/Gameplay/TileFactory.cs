using System.Linq;
using UnityEngine;
using System.Collections.Generic;

namespace TileMatch.Scripts.Gameplay
{
    public class TileContext
    {
        public bool Enabled;
        public Tile Entity;
    }

    public class TileFactory : MonoBehaviour
    {
        [field: SerializeField] public Tile TilePrefab { get; private set; }
        [field: SerializeField] public TileConfig TileConfig { get; private set; }
        public TileTypeSelector TileTypeSelector { get; private set; } = new();
        
        private readonly Dictionary<int, TileContext> _tiles = new();

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        public Tile GetTile(List<TileType> tileFilter)
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

        public void DestroyTile(Tile tile)
        {
            var tileContext = _tiles[tile.Id];
            
            if (tileContext != null)
            {
                tileContext.Enabled = false;
                tileContext.Entity.SetParent(transform);
                tileContext.Entity.ResetTransform();
                tileContext.Entity.SetInteraction(true);
                tileContext.Entity.gameObject.SetActive(false);
                TileTypeSelector.RemoveTile(tileContext.Entity.Type);
            }

            else
            {
                Debug.LogError($"TileFactory::DestroyTile {tile.name}_{tile.Id} not found in factory!");
            }
        }

        public void Clear()
        {
            foreach (var tilesContext in _tiles.Values)
            {
                DestroyTile(tilesContext.Entity);
            }
        }

        public Tile[] GetActiveTiles()
        {
            return _tiles
                .Where(t => t.Value.Enabled)
                .Select(t => t.Value.Entity).ToArray();
        }
    }
}