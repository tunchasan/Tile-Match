using UnityEngine;

namespace TileMatch.Scripts.Gameplay.Tile
{
    public struct TileColliderContext
    {
        public bool IsOverlap;
        public StandardTile UpperStandardTile;
        public StandardTile LowerStandardTile;
    }
    
    public static class TileCollider
    {
        public static bool IsOverlapped(StandardTile tile1, StandardTile tile2, float threshold, out TileColliderContext tileColliderContext)
        {
            var position1 = tile1.transform.position;
            var position2 = tile2.transform.position;
            var isOverlap = Vector2.Distance(position1, position2) < threshold;
            
            var sortingOrder1 = tile1.SortingOrder;
            var sortingOrder2 = tile2.SortingOrder;

            var upperTile = sortingOrder1 > sortingOrder2 ? tile1 : tile2;
            var lowerTile = sortingOrder1 > sortingOrder2 ? tile2 : tile1;

            tileColliderContext = new TileColliderContext
            {
                IsOverlap = isOverlap,
                LowerStandardTile = lowerTile,
                UpperStandardTile = upperTile
            };

            return tileColliderContext.IsOverlap;
        }
    }
}