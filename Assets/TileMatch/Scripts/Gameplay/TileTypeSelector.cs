using System.Linq;
using UnityEngine;
using System.Collections.Generic;

namespace TileMatch.Scripts.Gameplay
{
    public class TileTypeContext
    {
        public int Quantity;
        public TileType Type;
    }
    
    public class TileTypeSelector
    {
        private readonly List<TileTypeContext> _tileTypeContexts = new();
        
        public TileType GetTile(List<TileType> tileTypes)
        {
            foreach (var t in _tileTypeContexts)
            {
                if (t.Quantity % 3 != 0)
                {
                    return t.Type;
                }
            }
            
            return tileTypes.Count == 0 ? 
                TileTypeExtensions.GetRandomTileType() : 
                tileTypes[Random.Range(0, tileTypes.Count)];
        }

        public void AddTile(TileType type)
        {
            var tileTypeContext = _tileTypeContexts.FirstOrDefault(t => t.Type.Equals(type));
            
            if (tileTypeContext != null)
            {
                tileTypeContext.Quantity++;
            }

            else
            {
                _tileTypeContexts.Add(new TileTypeContext
                {
                    Quantity = 1,
                    Type = type
                });
            }
        }
        
        public void RemoveTile(TileType type)
        {
            var tileTypeContext = _tileTypeContexts.FirstOrDefault(t => t.Type.Equals(type));
            
            if (tileTypeContext != null)
            {
                tileTypeContext.Quantity--;

                if (tileTypeContext.Quantity <= 0)
                {
                    _tileTypeContexts.Remove(tileTypeContext);
                }
            }
        }
    }
}