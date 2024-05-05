using UnityEngine;

namespace TileMatch.Scripts.Gameplay.Tile
{
    public struct TileColliderContext
    {
        public bool IsOverlap;
        public Tile UpperTile;
        public Tile LowerTile;
    }
    
    public static class TileCollider
    {
        private const float Threshold = .75f;

        public static bool IsOverlapped(Tile tile1, Tile tile2, out TileColliderContext tileColliderContext)
        {
            var position1 = tile1.transform.position;
            var position2 = tile2.transform.position;
            var isOverlap = Vector2.Distance(position1, position2) < Threshold;
            
            var sortingOrder1 = tile1.SortingOrder;
            var sortingOrder2 = tile2.SortingOrder;

            var upperTile = sortingOrder1 > sortingOrder2 ? tile1 : tile2;
            var lowerTile = sortingOrder1 > sortingOrder2 ? tile2 : tile1;

            tileColliderContext = new TileColliderContext
            {
                IsOverlap = isOverlap,
                LowerTile = lowerTile,
                UpperTile = upperTile
            };

            return tileColliderContext.IsOverlap;
        }
    }
}