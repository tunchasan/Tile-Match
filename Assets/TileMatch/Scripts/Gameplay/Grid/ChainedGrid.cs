using UnityEngine;

namespace TileMatch.Scripts.Gameplay.Grid
{
    public class ChainedGrid : MonoBehaviour
    {
        [field: SerializeField] public StandardGrid[] Grids { get; private set; }
        public int LastIndex { get; private set; }
        
        public bool Fill(Tile.Tile tile)
        {
            if (!this.HasAvailableSlot()) return false;
            Grids[LastIndex].Fill(tile);
            LastIndex++;
            return true;
        }

        public void Clear()
        {
            foreach (var t in Grids)
            {
                t.Clear();
            }
            
            LastIndex = 0;
        }
    }
}