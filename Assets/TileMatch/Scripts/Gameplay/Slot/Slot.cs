using UnityEngine;

namespace TileMatch.Scripts.Gameplay.Slot
{
    public class Slot : MonoBehaviour
    {
        public Tile.StandardTile AttachedTile { get; private set; }

        public bool IsEmpty => AttachedTile == null;
        
        public void Fill(Tile.StandardTile receivedStandardTile)
        {
            receivedStandardTile.SetParent(transform);
            receivedStandardTile.ResetTransform(true);
            AttachedTile = receivedStandardTile;
        }

        public void Clear()
        {
            AttachedTile = null;
        }
    }
}