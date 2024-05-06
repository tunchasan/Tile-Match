using UnityEngine;

namespace TileMatch.Scripts.Gameplay.Slot
{
    public class Slot : MonoBehaviour
    {
        public void Fill(Tile.Tile receivedTile)
        {
            receivedTile.SetParent(transform);
            receivedTile.ResetTransform(true);
        }
    }
}