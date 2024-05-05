namespace TileMatch.Scripts.Gameplay.Grid
{
    public static class GridUtils
    {
        public static bool IsEmpty(this StandardGrid standardGrid)
        {
            return standardGrid.AttachedTile == null;
        }
        
        public static bool IsEmpty(this ChainedGrid chainedGrid)
        {
            return chainedGrid.LastIndex == 0;
        }
        
        public static bool HasAvailableSlot(this ChainedGrid chainedGrid)
        {
            return chainedGrid.Grids.Length > chainedGrid.LastIndex;
        }
    }
}