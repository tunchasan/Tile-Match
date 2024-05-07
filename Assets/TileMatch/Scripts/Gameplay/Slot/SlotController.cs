using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using TileMatch.Scripts.Core.NotifySystem;

namespace TileMatch.Scripts.Gameplay.Slot
{
    public class SlotController : MonoBehaviour
    {
        [Header("References")] 
        [SerializeField] private Slot[] slots;
        [SerializeField] private List<Tile.Tile> attachedTiles;

        private void RequestFillSlot(Tile.Tile receivedTile)
        {
            if (attachedTiles.Count < slots.Length)
            {
                var matchedSlotIndex = GetMatchedSlotIndex(receivedTile);

                if (matchedSlotIndex != -1)
                {
                    attachedTiles.Insert(matchedSlotIndex + 1, receivedTile);
                    
                    NotificationCenter.PostNotification(NotificationTag.OnTilePlacedToSlot, receivedTile);
                    
                    ReorderSlotElements();
                }

                else
                {
                    FillSlot(receivedTile);
                }
            }
        }

        private int GetMatchedSlotIndex(Tile.Tile receivedTile)
        {
            for (var i = 0; i < slots.Length; i++)
            {
                if (slots[i].IsEmpty) return -1;
                
                if (slots[i].AttachedTile.Type.Equals(receivedTile.Type))
                {
                    return i;
                }   
            }

            return -1;
        }

        private async void ReorderSlotElements()
        {
            for (var i = 0; i < attachedTiles.Count; i++)
            {
                slots[i].Clear();
                slots[i].Fill(attachedTiles[i]);
            }

            await UniTask.Delay(300);

            ValidateSlots();
        }
        
        private void ValidateSlots()
        {
            if (attachedTiles.Count < 3)
                return;

            for (var i = 0; i <= attachedTiles.Count - 3; i++)
            {
                // Check the current tile against the next two tiles
                if (attachedTiles[i].Type == attachedTiles[i + 1].Type && 
                    attachedTiles[i].Type == attachedTiles[i + 2].Type)
                {
                    Debug.Log($"SlotController::ValidateSlots Three consecutive tiles of type {attachedTiles[i].Type} found starting at index {i}.");
                    NotificationCenter.PostNotification(NotificationTag.OnTilesMatched, new List<Tile.Tile> { attachedTiles[i], attachedTiles[i + 1], attachedTiles[i + 2] });
                    MatchSlots(i);
                }
            }
        }

        private void MatchSlots(int startingIndex)
        {
            for (var i = startingIndex + 2; i >= startingIndex; i--)
            {
                slots[i].Clear();
                var matchedTile = attachedTiles[i];
                attachedTiles.Remove(matchedTile);
                Destroy(matchedTile.gameObject);
            }
            
            ReorderSlotElements();
        }

        private void FillSlot(Tile.Tile receivedTile)
        {
            var availableSlot = slots[attachedTiles.Count];
            attachedTiles.Add(receivedTile);
            availableSlot.Fill(receivedTile);
            NotificationCenter.PostNotification(NotificationTag.OnTilePlacedToSlot, receivedTile);
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