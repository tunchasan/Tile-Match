using System.Threading.Tasks;
using TileMatch.Scripts.Gameplay.Tile;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace TileMatch.Scripts.Gameplay.Level
{
    public static class LevelUtility
    {
        /// <summary>
        /// Validates the placement of tiles within the grids by checking for overlaps and disabling interactions
        /// on the lower tile of any overlapping pairs.
        /// </summary>
        public static void RefreshLevel(StandardTile[] activeTiles)
        {
            // Iterate through all pairs of active tiles to check for overlaps
            foreach (var t1 in activeTiles)
            {
                t1.SetInteraction(true);
                
                foreach (var t2 in activeTiles)
                {
                    if(t1.Equals(t2)) continue;
            
                    // Check for overlap and disable interaction if needed
                    if (TileCollider.IsOverlapped(t1, t2, .65F, out var result))
                    {
                        result.LowerStandardTile.SetInteraction(false);
                    }
                }
            }
        }
        
        public static async Task<bool> IsLevelAddressValid(string address)
        {
            var handle = Addressables.LoadResourceLocationsAsync(address);

            await handle.Task;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                var isValid = handle.Result != null && handle.Result.Count > 0;
                Addressables.Release(handle);
                return isValid;
            }

            Debug.LogError($"Failed to check addressable asset: {address}");
            return false;
        }
    }
}