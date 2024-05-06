using TileMatch.Scripts.Gameplay.Tile;

namespace TileMatch.Scripts.Gameplay.Level
{
    public static class LevelUtility
    {
        /// <summary>
        /// Validates the placement of tiles within the grids by checking for overlaps and disabling interactions
        /// on the lower tile of any overlapping pairs.
        /// </summary>
        public static void RefreshLevel(Tile.Tile[] activeTiles)
        {
            // Iterate through all pairs of active tiles to check for overlaps
            foreach (var t1 in activeTiles)
            {
                t1.SetInteraction(true);
                
                foreach (var t2 in activeTiles)
                {
                    if(t1.Equals(t2)) continue;
            
                    // Check for overlap and disable interaction if needed
                    if (TileCollider.IsOverlapped(t1, t2, out var result))
                    {
                        result.LowerTile.SetInteraction(false);
                    }
                }
            }
        }
    }
}