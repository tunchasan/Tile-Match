using UnityEngine;

namespace TileMatch.Scripts.Gameplay.Slot
{
    public class Slot : MonoBehaviour
    {
        public Tile.Tile AttachedTile { get; private set; }

        public bool IsEmpty => AttachedTile == null;
        
        public void Fill(Tile.Tile receivedTile)
        {
            receivedTile.SetParent(transform);
            receivedTile.ResetTransform(true);
            AttachedTile = receivedTile;
        }

        public void Clear()
        {
            AttachedTile = null;
        }
    }
}