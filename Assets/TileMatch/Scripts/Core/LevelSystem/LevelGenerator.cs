#if UNITY_EDITOR

using System;
using System.Linq;
using UnityEngine;
using EditorAttributes;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using TileMatch.Scripts.Gameplay.Grid;
using TileMatch.Scripts.Gameplay.Tile;

namespace TileMatch.Scripts.Core.LevelSystem
{
    /// <summary>
    /// Manages the generation and setup of levels in a tile-matching game. This class handles the distribution of tiles across standard and chained grids,
    /// enforces grid configurations based on intensity settings, and ensures valid tile interactions. It includes mechanisms to shuffle grid arrangements,
    /// clear previous level configurations, and validate grid setups to prevent gameplay issues such as overlapping tiles.
    /// </summary>
    public class LevelGenerator : MonoBehaviour
    {
        [Header("Config")]
        [Range(21, 120)] 
        [SerializeField] 
        // Total number of tiles to be used in the level, adjusted to always be a multiple of 3 in the editor.
        private int tileCount;

        [Range(0F, 1F)] 
        [SerializeField] 
        // Proportion of the total tile count that will be used for the top layer of the grid.
        private float topTileIntensity;

        [Range(0F, 1F)] 
        [field: SerializeField, ReadOnly] 
        // Automatically calculated to represent the remaining tile intensity for the bottom layer, complementary to topTileIntensity.
        private float bottomTileIntensity;

        [SerializeField] 
        // List of allowed tile types for the level.
        private List<TileType> typeFilter;

        [Header("References")]
        [SerializeField]
        // Represents top layer of the board.
        // Array of standard Grid objects in the level, where tiles are assigned during level generation.
        public StandardGrid[] topLayerGrids;

        [SerializeField]
        // Represents bottom layer of the board.
        // Array of ChainedGrid objects for more complex tile arrangements that may have linked behavior or properties.
        public ChainedGrid[] bottomLayerGrids;

        [Header("Others")] 
        [SerializeField] private int levelSaveIndexPointer;
        [SerializeField] private string levelSaveDirectoryPath;
        
        /// <summary>
        /// Ensures tileCount is a multiple of 3 and calculates the bottomTileIntensity based on topTileIntensity.
        /// </summary>
        private void OnValidate()
        {
            // Adjust tileCount to ensure it is always a multiple of 3
            tileCount = Mathf.FloorToInt((float)tileCount / 3) * 3;
            // Automatically set bottomTileIntensity as the complement of topTileIntensity
            bottomTileIntensity = 1F - topTileIntensity;
        }
       
        /// <summary>
        /// Generates a new level by clearing previous grid states, distributing tiles based on intensity settings,
        /// and validating overlapping among tiles in the grid.
        /// </summary>
        public void GenerateLevel()
        {
            // Clear last level's caches
            ClearGrids();

            // Calculate the number of tiles for normal and chained grids based on intensity
            var normalGridTiles = (int)(tileCount * topTileIntensity);
            var chainedGridTiles = tileCount - normalGridTiles;
    
            // Distribute tiles across normal and chained grids
            FillGrids(normalGridTiles);
            FillChainedGrids(chainedGridTiles);
    
            // Check and resolve any improper tile interactions
            ValidateGrids();
        }

        /// <summary>
        ///  Initiates the process to save the current GameObject state as a prefab (new level)
        /// </summary>
        public void SaveLevel()
        {
            LevelPrefabUtility.SaveLevelPrefab(this, ref levelSaveIndexPointer, levelSaveDirectoryPath);
        }

        /// <summary>
        /// This method is used to reset the tile setup before generating a new level or when cleaning up the current level.
        /// </summary>
        public void ClearLevel()
        {
            // Clear all tiles managed by the TileFactory
            Main.Instance.TileFactory.Clear();
        }

        /// <summary>
        /// Fills the standard grids with tiles. The grids are first shuffled for random tile distribution,
        /// then filled up to the specified quantity or the total number of grids available, whichever is smaller.
        /// </summary>
        /// <param name="quantity">The number of tiles to distribute among the grids.</param>
        private void FillGrids(int quantity)
        {
            // Shuffle grids to ensure random tile distribution
            Shuffle(topLayerGrids);

            // Loop through each grid and assign a tile from the tile factory based on the type filter
            for (var i = 0; i < Mathf.Min(quantity, topLayerGrids.Length); i++)
            {
                topLayerGrids[i].Fill(Main.Instance.TileFactory.GetTile(typeFilter));
            }
        }

        /// <summary>
        /// Fills the chained grids with tiles based on a specified quantity. It first shuffles the chained grids for randomness,
        /// then fills them proportionally based on the provided quantity, ensuring no grid is overfilled.
        /// </summary>
        /// <param name="quantity">The number of tiles to distribute among the chained grids.</param>
        private void FillChainedGrids(int quantity)
        {
            // Shuffle chained grids to ensure random tile distribution
            Shuffle(bottomLayerGrids);

            var filledGrids = 0;
            var selectionCount = (int)Math.Ceiling(quantity / 7F); // Calculate number of chained grids needed based on desired tile count
            var selectedChainedGrids = bottomLayerGrids.Take(selectionCount); // Select the first few grids after shuffling

            // Loop through each selected chained grid
            foreach (var t in selectedChainedGrids)
            {
                // Loop through each individual grid within a chained grid and assign tiles
                foreach (var unused in t.Grids)
                {
                    filledGrids++;
                    t.Fill(Main.Instance.TileFactory.GetTile(typeFilter)); // Fill the grid with a tile from the factory based on the type filter
                    if (filledGrids >= quantity) return; // Exit if the desired quantity of tiles has been placed
                }
            }
        }
        
        /// <summary>
        /// Validates the placement of tiles within the grids by checking for overlaps and disabling interactions
        /// on the lower tile of any overlapping pairs.
        /// </summary>
        private void ValidateGrids()
        {
            var activeTiles = Main.Instance.TileFactory.GetActiveTiles();

            // Iterate through all pairs of active tiles to check for overlaps
            foreach (var t1 in activeTiles)
            {
                foreach (var t2 in activeTiles)
                {
                    if(t1.Equals(t2)) continue;
            
                    // Check for overlap and disable interaction if needed
                    if (TileCollider.IsOverlapped(t1, t2, out var result))
                    {
                        result.LowerTile.SetInteraction(false);
                    }
                }
            }
        }
        
        /// <summary>
        /// Clears all tiles from both the normal and chained grids.
        /// </summary>
        private void ClearGrids()
        {
            // Clear all tiles in normal grids
            foreach (var t in topLayerGrids)
            {
                t.Clear();
            }

            // Clear all tiles in chained grids
            foreach (var t in bottomLayerGrids)
            {
                t.Clear();
            }
        }
        
        /// <summary>
        /// Shuffles the elements of the given list in place using the Fisher-Yates shuffle algorithm.
        /// </summary>
        /// <param name="array">The list of items to be shuffled.</param>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        private static void Shuffle<T>(IList<T> array)
        {
            for (var i = array.Count - 1; i > 0; i--)
            {
                var randomIndex = Random.Range(0, i + 1);
                (array[i], array[randomIndex]) = (array[randomIndex], array[i]);
            }
        }
    }
}

#endif