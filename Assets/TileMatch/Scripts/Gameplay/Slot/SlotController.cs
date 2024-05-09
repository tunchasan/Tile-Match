using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using TileMatch.Scripts.Gameplay.Tile;
using TileMatch.Scripts.Gameplay.Level;
using TileMatch.Scripts.Core.NotifySystem;

namespace TileMatch.Scripts.Gameplay.Slot
{
    public class SlotController : MonoBehaviour
    {
        [Header("References")] 
        [SerializeField] private Slot[] slots;
        [SerializeField] private Slot[] tempSlots;
        [SerializeField] private List<StandardTile> attachedTiles;
        [SerializeField] private List<StandardTile> attachedTilesInTempSlots;

        public int ActiveTiles => attachedTiles.Count + attachedTilesInTempSlots.Count;

        private void RequestFillSlot(StandardTile elem)
        {
            if (attachedTiles.Count < slots.Length)
            {
                elem.SetInteraction(false, false);
                var matchedSlotIndex = GetMatchedSlotIndex(elem);

                if (matchedSlotIndex != -1)
                {
                    attachedTiles.Insert(matchedSlotIndex + 1, elem);
                    
                    ReorderSlotElements();
                }

                else
                {
                    FillSlot(elem);
                }

                RemoveElementFromTempSlotIfNeeded(elem);
                
                NotificationCenter.PostNotification(NotificationTag.OnTilePlacedToSlot, LevelStateChangeReason.Remove, elem);

                // Checks, whether all slots filled or not
                if (attachedTiles.Count == slots.Length && !HasAnyValidMatch(out _))
                {
                    NotificationCenter.PostNotification(NotificationTag.AllSlotsFilled);
                }
            }
        }

        private int GetMatchedSlotIndex(StandardTile elem)
        {
            for (var i = 0; i < slots.Length; i++)
            {
                if (slots[i].IsEmpty) return -1;
                
                if (slots[i].AttachedTile.Type.Equals(elem.Type))
                {
                    return i;
                }   
            }

            return -1;
        }

        private async void ReorderSlotElements()
        {
            for (var i = 0; i < slots.Length; i++)
            {
                slots[i].Clear();

                if (i < attachedTiles.Count)
                {
                    slots[i].Fill(attachedTiles[i]);
                }
            }

            await UniTask.Delay(300);

            ValidateSlots();
        }
        
        private void ReorderTempSlotElements()
        {
            for (var i = 0; i < tempSlots.Length; i++)
            {
                tempSlots[i].Clear();

                if (i < attachedTilesInTempSlots.Count)
                {
                    tempSlots[i].Fill(attachedTilesInTempSlots[i]);
                }
            }
        }

        private bool HasAnyValidMatch(out int matchStartIndex)
        {
            if (attachedTiles.Count < 3)
            {
                matchStartIndex = -1;
                return false;
            }

            for (var i = 0; i <= attachedTiles.Count - 3; i++)
            {
                // Check the current tile against the next two tiles
                if (attachedTiles[i].Type == attachedTiles[i + 1].Type && attachedTiles[i].Type == attachedTiles[i + 2].Type)
                {
                    matchStartIndex = i;
                    return true;
                }
            }

            matchStartIndex = -1;
            return false;
        }
        
        private void ValidateSlots()
        {
            if (HasAnyValidMatch(out var matchStartIndex))
            {
                Debug.Log($"SlotController::ValidateSlots Three consecutive tiles of type {attachedTiles[matchStartIndex].Type} found starting at index {matchStartIndex}.");
                
                var matchingTiles = new List<StandardTile>
                {
                    attachedTiles[matchStartIndex], attachedTiles[matchStartIndex + 1],
                    attachedTiles[matchStartIndex + 2]
                };
                
                NotificationCenter.PostNotification(NotificationTag.OnTilesMatched, matchingTiles);
                MatchSlots(matchStartIndex);
            }
        }
        
        private async void MatchSlots(int startingIndex)
        {
            for (var i = startingIndex + 2; i >= startingIndex; i--)
            {
                var targetTile = attachedTiles[i];
                targetTile.Destroy();
                RemoveElementFromSlot(slots[i], targetTile);
            }

            await UniTask.Delay(350);
            ReorderSlotElements();
        }

        private void RemoveElementFromSlot(Slot targetSlot, StandardTile elem)
        {
            targetSlot.Clear();
            attachedTiles.Remove(elem);
        }
        
        private void RemoveElementFromTempSlotIfNeeded(StandardTile elem)
        {
            if(attachedTilesInTempSlots.Count == 0) return;
            
            foreach (var tempSlot in tempSlots)
            {
                if (!tempSlot.IsEmpty && tempSlot.AttachedTile.Id.Equals(elem.Id))
                {
                    tempSlot.Clear();
                    attachedTilesInTempSlots.Remove(elem);
                    ReorderTempSlotElements();
                    return;
                }
            }
        }

        private void ClearSlots()
        {
            foreach (var slot in slots)
            {
                if (slot.IsEmpty) continue;
                var tile = slot.AttachedTile;
                tile.DestroyImmediately();
                slot.Clear();
            }
            
            foreach (var tempSlot in tempSlots)
            {
                if (tempSlot.IsEmpty) continue;
                var tile = tempSlot.AttachedTile;
                tile.DestroyImmediately();
                tempSlot.Clear();
            }
            
            attachedTiles.Clear();
            attachedTilesInTempSlots.Clear();
        }
        
        private void FillSlot(StandardTile elem)
        {
            var availableSlot = slots[attachedTiles.Count];
            attachedTiles.Add(elem);
            availableSlot.Fill(elem);
        }
        
        private void FillTempSlot(StandardTile elem)
        {
            var availableSlot = tempSlots[attachedTilesInTempSlots.Count];
            attachedTilesInTempSlots.Add(elem);
            availableSlot.Fill(elem);
            elem.SetInteraction(true, false);
            NotificationCenter.PostNotification(NotificationTag.OnTilePlacedToSlot, LevelStateChangeReason.Remove, elem);
        }
        
        private void RequestReverseMove()
        {
            if (attachedTiles.Count == 0)
            {
                Debug.LogWarning("SlotController::RequestReverseMove Blocked!");
                return;
            }

            var element = attachedTiles[^1];
            var relatedSlot = slots[attachedTiles.Count - 1];
            RemoveElementFromSlot(relatedSlot, element);
            ReorderSlotElements();
            
            NotificationCenter.PostNotification(NotificationTag.OnReverseActionCompleted, LevelStateChangeReason.Add, element);
        }

        private void RequestDrawTiles()
        {
            if (attachedTiles.Count < 3 || attachedTilesInTempSlots.Count != 0)
            {
                Debug.LogWarning("SlotController::RequestDrawTiles Blocked!");
                return;
            }

            for (var i = 0; i < tempSlots.Length; i++)
            {
                var relatedSlot = slots[i];
                var element = attachedTiles[0];
                RemoveElementFromSlot(relatedSlot, element);
                FillTempSlot(element);
            }
            
            ReorderSlotElements();
        }
        
        private void OnEnable()
        {
            NotificationCenter.AddObserver(NotificationTag.OnLevelPreUnload, ClearSlots);
            NotificationCenter.AddObserver(NotificationTag.OnDrawAction, RequestDrawTiles);
            NotificationCenter.AddObserver(NotificationTag.OnReverseAction, RequestReverseMove);
            NotificationCenter.AddObserver<StandardTile>(NotificationTag.OnTileSelect, RequestFillSlot);
        }

        private void OnDisable()
        {
            NotificationCenter.RemoveObserver(NotificationTag.OnLevelPreUnload, ClearSlots);
            NotificationCenter.RemoveObserver(NotificationTag.OnDrawAction, RequestDrawTiles);
            NotificationCenter.RemoveObserver(NotificationTag.OnReverseAction, RequestReverseMove);
            NotificationCenter.RemoveObserver<StandardTile>(NotificationTag.OnTileSelect, RequestFillSlot);
        }
    }
}