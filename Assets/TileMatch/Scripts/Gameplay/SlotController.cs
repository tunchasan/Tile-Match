using System.Collections.Generic;
using System.Linq;
using TileMatch.Scripts.Core.LevelSystem;
using UnityEngine;
using TileMatch.Scripts.Core.NotifySystem;

namespace TileMatch.Scripts.Gameplay
{
    public class SlotController : MonoBehaviour
    {
        [Header("References")] 
        [SerializeField] private Slot[] slots;

        private int _lastFilledIndex;
        
        private void RequestFillSlot(Tile.Tile receivedTile)
        {
            FillSlot(receivedTile);
        }

        private void FillSlot(Tile.Tile receivedTile)
        {
            if (_lastFilledIndex < slots.Length)
            {
                var availableSlot = slots[_lastFilledIndex];

                availableSlot.Fill(receivedTile);

                _lastFilledIndex++;
            }
        }
        
        private void OnEnable()
        {
            NotificationCenter.AddObserver<Tile.Tile>(NotificationTag.OnTileSelect, RequestFillSlot);
        }

        private void OnDisable()
        {
            NotificationCenter.RemoveObserver<Tile.Tile>(NotificationTag.OnTileSelect, RequestFillSlot);
        }
    }
}