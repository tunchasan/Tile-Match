#if UNITY_EDITOR

using System.IO;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using TileMatch.Scripts.Gameplay.Grid;

namespace TileMatch.Scripts.Core.LevelSystem
{
    /// <summary>
    /// Provides utilities for saving GameObjects as prefabs with specific clean-up and preparation steps
    /// tailored to LevelGenerator requirements.
    /// </summary>
    public static class LevelPrefabUtility
    {
        /// <summary>
        /// Saves a GameObject as a prefab after performing necessary cleanups and directory checks.
        /// </summary>
        /// <param name="levelGenerator">The LevelGenerator instance that contains the data to save.</param>
        /// <param name="saveLevelIndexPointer">Reference to the index pointer for saving unique prefab files.</param>
        /// <param name="saveLevelDirectoryPath">The directory path where the prefab should be saved.</param>
        public static void SaveLevelPrefab(LevelGenerator levelGenerator, ref int saveLevelIndexPointer, string saveLevelDirectoryPath)
        {
            CreateDirectoryIfNeeded(saveLevelDirectoryPath);
            var prefabInstance = PreparePrefabInstance(levelGenerator.gameObject);
            var levelGeneratorComponentInstance = prefabInstance.GetComponent<LevelGenerator>();
            CleanUpPrefabInstance(levelGeneratorComponentInstance, levelGenerator);
            SaveAndLogPrefab(prefabInstance, ref saveLevelIndexPointer, saveLevelDirectoryPath);
            DestroyPrefabInstance(prefabInstance);
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// Ensures that the specified directory exists, and creates it if it does not.
        /// </summary>
        /// <param name="path">The directory path to check and potentially create.</param>
        private static void CreateDirectoryIfNeeded(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        /// <summary>
        /// Creates a duplicate of the original GameObject for modifications and prefab saving.
        /// </summary>
        /// <param name="original">The original GameObject to duplicate.</param>
        /// <returns>The duplicated GameObject.</returns>
        private static GameObject PreparePrefabInstance(GameObject original)
        {
            return Object.Instantiate(original);
        }

        /// <summary>
        /// Performs cleanup on the prefab instance by removing unnecessary components and empty grids.
        /// </summary>
        /// <param name="levelGeneratorInstance">The prefab instance to clean up.</param>
        /// <param name="globalLevelGenerator">The associated LevelGenerator component to reference during cleanup.</param>
        private static void CleanUpPrefabInstance(LevelGenerator levelGeneratorInstance, LevelGenerator globalLevelGenerator)
        {
            RemoveEmptyGrids(levelGeneratorInstance, globalLevelGenerator.topLayerGrids);
            RemoveEmptyGrids(levelGeneratorInstance, globalLevelGenerator.bottomLayerGrids);
            Object.DestroyImmediate(levelGeneratorInstance, true);
        }

        /// <summary>
        /// Removes all empty grid components and their parent container if all are empty.
        /// </summary>
        private static void RemoveEmptyGrids(LevelGenerator levelGeneratorInstance, IReadOnlyList<StandardGrid> grids)
        {
            var container = levelGeneratorInstance.topLayerGrids[0].transform.parent;
            var hasNonEmptyGrid = false;

            for (var i = 0; i < grids.Count; i++)
            {
                if (grids[i].IsEmpty())
                {
                    Object.DestroyImmediate(levelGeneratorInstance.topLayerGrids[i].gameObject, true);
                }

                else
                {
                    hasNonEmptyGrid = true;
                }
            }

            if (!hasNonEmptyGrid && container != null)
            {
                Object.DestroyImmediate(container.gameObject, true);
            }
        }
        
        /// <summary>
        /// Removes all empty grid components and their parent container if all are empty.
        /// </summary>
        private static void RemoveEmptyGrids(LevelGenerator levelGeneratorInstance, IReadOnlyList<ChainedGrid> grids)
        {
            var container = levelGeneratorInstance.bottomLayerGrids[0].transform.parent;
            var hasNonEmptyGrid = false;

            for (var i = 0; i < grids.Count; i++)
            {
                if (grids[i].IsEmpty())
                {
                    Object.DestroyImmediate(levelGeneratorInstance.bottomLayerGrids[i].gameObject, true);
                }

                else
                {
                    hasNonEmptyGrid = true;
                }
            }

            if (!hasNonEmptyGrid && container != null)
            {
                Object.DestroyImmediate(container.gameObject, true);
            }
        }

        /// <summary>
        /// Saves the cleaned-up GameObject as a prefab at the specified path and logs the outcome.
        /// </summary>
        /// <param name="instance">The instance to save as a prefab.</param>
        /// <param name="saveLevelIndexPointer">Reference to the index pointer for unique naming.</param>
        /// <param name="directoryPath">The directory where the prefab should be saved.</param>
        private static void SaveAndLogPrefab(GameObject instance, ref int saveLevelIndexPointer, string directoryPath)
        {
            var localPath = $"{directoryPath}/Level_{saveLevelIndexPointer++}.prefab";
            var savedPrefab = PrefabUtility.SaveAsPrefabAsset(instance, localPath);
            Debug.Log(savedPrefab != null ? $"Prefab saved/updated at: {localPath}" : "Failed to save prefab.");
        }

        /// <summary>
        /// Destroys the temporary prefab instance after saving is complete.
        /// </summary>
        /// <param name="instance">The instance to destroy.</param>
        private static void DestroyPrefabInstance(Object instance)
        {
            Object.DestroyImmediate(instance);
        }
    }
}

#endif