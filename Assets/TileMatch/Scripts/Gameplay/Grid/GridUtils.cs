namespace TileMatch.Scripts.Gameplay.Grid
{
    public static class GridUtils
    {
        public static bool IsEmpty(this Grid grid)
        {
            return grid.AttachedTile == null;
        }
        
        public static bool HasAvailableSlot(this ChainedGrid chainedGrid)
        {
            return chainedGrid.Grids.Length > chainedGrid.LastIndex;
        }
    }
}