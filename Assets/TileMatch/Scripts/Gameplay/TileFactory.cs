using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

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
        
        private readonly Dictionary<int, TileContext> _tiles = new();

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
        
        public Tile GetTile(List<TileType> tileTypes)
        {
            return tileTypes.Count == 0 ? GetTile() : GetTile(tileTypes[Random.Range(0, tileTypes.Count)]);
        }
        
        private Tile GetTile()
        {
            return GetTile(TileTypeExtensions.GetRandomTileType());
        }

        private Tile GetTile(TileType type)
        {
            var unusedTile = _tiles.FirstOrDefault(tile => !tile.Value.Enabled).Value;

            if (unusedTile != null)
            {
                unusedTile.Enabled = true;
                unusedTile.Entity.gameObject.SetActive(true);
                
                // Initialize the Tile with configs
                unusedTile.Entity.Init(type, TileConfig.Get(type).sprite);
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
            newTile.Init(newTileId, type, TileConfig.Get(type).sprite);
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