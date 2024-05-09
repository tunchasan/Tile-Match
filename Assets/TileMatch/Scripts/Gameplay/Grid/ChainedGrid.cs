using TileMatch.Scripts.Gameplay.Tile;
using UnityEngine;

namespace TileMatch.Scripts.Gameplay.Grid
{
    public class ChainedGrid : MonoBehaviour
    {
        [field: SerializeField] public StandardGrid[] Grids { get; private set; }
        public int LastIndex { get; private set; }
        
        public void FillInEditMode(StandardTile standardTile)
        {
            if (!this.HasAvailableSlot()) return;
            Grids[LastIndex].FillInEditMode(standardTile);
            LastIndex++;
        }

        public void ClearInEditMode()
        {
            foreach (var t in Grids)
            {
                t.ClearInEditMode();
            }
            
            LastIndex = 0;
        }
    }
}