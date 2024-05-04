using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace TileMatch.Scripts.Gameplay.Board
{
    public class BoardTop : MonoBehaviour
    {
        public Gameplay.Grid.Grid[] Grids;
        public Grid.ChainedGrid[] ChainedGrids;
        public List<TileType> TypeFilter;
        
        [Range(21, 108)] // Adjust the range as needed
        public int count;

        private void OnValidate()
        {
            // Ensure count is a multiple of 3
            count = Mathf.FloorToInt((float)count / 3) * 3;
        }

        public void GenerateLevel()
        {
            // Clear last level's caches
            ClearGrids();

            var normalGridTiles = (int)(count * .7F);
            var chainedGridTiles = count - normalGridTiles;
            
            // Fill the slots
            FillGrids(normalGridTiles);
            FillChainedGrids(chainedGridTiles);
            
            // Validates grid's interactions
            ValidateGrids();
        }

        public void ClearLevel()
        {
            Main.Instance.TileFactory.Clear();
        }

        private void FillGrids(int quantity)
        {
            // Shuffle sprites array to randomize selection
            Shuffle(Grids);
        
            // Loop through each grid and assign a sprite
            for (var i = 0; i < Mathf.Min(quantity, Grids.Length); i++)
            {
                Grids[i].Fill(Main.Instance.TileFactory.GetTile(TypeFilter));
            }
        }

        private void FillChainedGrids(int quantity)
        {
            // Shuffle sprites array to randomize selection
            Shuffle(ChainedGrids);

            var filledGrids = 0;
            var selectionCount = (int)Math.Ceiling(quantity / 7F);
            var selectedChainedGrids = ChainedGrids.Take(selectionCount);

            foreach (var t in selectedChainedGrids)
            {
                // Loop through each grid and assign a sprite
                foreach (var t1 in t.Grids)
                {
                    filledGrids++;
                    t1.Fill(Main.Instance.TileFactory.GetTile(TypeFilter));
                    if(filledGrids >= quantity) return;
                }
            }
        }
        
        private void ValidateGrids()
        {
            var activeTiles = Main.Instance.TileFactory.GetActiveTiles();

            foreach (var t1 in activeTiles)
            {
                foreach (var t2 in activeTiles)
                {
                    if(t1.Equals(t2)) continue;
                    
                    if (TileCollider.IsOverlapped(t1, t2, out var result))
                    {
                        result.LowerTile.SetInteraction(false);
                    }
                }
            }
        }
        
        private void ClearGrids()
        {
            foreach (var t in Grids)
            {
                t.Clear();
            }
            
            foreach (var t in ChainedGrids)
            {
                t.Clear();
            }
        }
        
        // Method to shuffle an array
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