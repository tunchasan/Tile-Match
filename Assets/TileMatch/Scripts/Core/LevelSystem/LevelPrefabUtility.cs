#if UNITY_EDITOR

using System.IO;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using TileMatch.Scripts.Gameplay.Grid;
using TileMatch.Scripts.Gameplay.Level;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.Animations;

namespace TileMatch.Scripts.Core.LevelSystem
{
    /// <summary>
    /// Provides utilities for saving GameObjects as prefabs with specific clean-up and preparation steps
    /// tailored to LevelGenerator requirements.
    /// </summary>
    public static class LevelPrefabUtility
    {
        private const string DirectoryPath = "Assets/Levels";
        
        /// <summary>
        /// Saves a GameObject as a prefab after performing necessary cleanups and directory checks.
        /// </summary>
        /// <param name="levelGenerator">The LevelGenerator instance that contains the data to save.</param>
        /// <param name="animatorController">The AnimatorController to assign to the new Animator component.</param>
        public static void SaveLevelPrefab(LevelGenerator levelGenerator, AnimatorController animatorController)
        {
            CreateDirectoryIfNeeded();
            var prefabInstance = PreparePrefabInstance(levelGenerator.gameObject, animatorController);
            var levelGeneratorComponentInstance = prefabInstance.GetComponent<LevelGenerator>();
            CleanUpPrefabInstance(levelGeneratorComponentInstance, levelGenerator);
            SaveAndLogPrefab(prefabInstance);
            DestroyPrefabInstance(prefabInstance);
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// Ensures that the specified directory exists, and creates it if it does not.
        /// </summary>
        private static void CreateDirectoryIfNeeded()
        {
            if (!Directory.Exists(DirectoryPath))
            {
                Directory.CreateDirectory(DirectoryPath);
            }
        }

        /// <summary>
        /// Duplicates a specified GameObject and sets it up for modifications and prefab creation. 
        /// This involves adding a Level component, setting up an Animator with a specified AnimatorController, 
        /// and resetting the transform properties to default values.
        /// </summary>
        /// <param name="original">The original GameObject to duplicate.</param>
        /// <param name="animatorController">The AnimatorController to assign to the new Animator component.</param>
        private static GameObject PreparePrefabInstance(GameObject original, RuntimeAnimatorController animatorController)
        {
            var instance = Object.Instantiate(original);
            
            // Adds required level component
            instance.AddComponent<Level>();

            // Adds animator component & sets references
            var animator = instance.AddComponent<Animator>();
            animator.runtimeAnimatorController = animatorController;
            
            // Adjusts transform data
            var transform = instance.transform;
            transform.position = Vector3.zero;
            transform.localScale = Vector3.one;
            
            return instance;
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
        /// Saves the provided GameObject as a prefab at the specified directory path and marks it as an addressable asset.
        /// Increments the level index pointer after successful operation.
        /// </summary>
        /// <param name="instance">The GameObject instance to save as a prefab.</param>
        private static void SaveAndLogPrefab(GameObject instance)
        {
            var localPath = $"{DirectoryPath}/Level_{GetLevelPointerIndex() + 1}.prefab";
            var savedPrefab = PrefabUtility.SaveAsPrefabAsset(instance, localPath);
    
            if (savedPrefab != null)
            {
                Debug.Log($"Prefab saved/updated at: {localPath}");
                MarkAsAddressable(savedPrefab);
            }
            else
            {
                Debug.LogError("Failed to save prefab.");
            }
        }

        /// <summary>
        /// Marks the specified Unity Object as an addressable asset within a designated group, setting its address.
        /// </summary>
        /// <param name="prefab">The Object to mark as addressable, typically a prefab.</param>
        private static void MarkAsAddressable(Object prefab)
        {
            // Get the Addressable Asset settings
            var settings = AddressableAssetSettingsDefaultObject.GetSettings(true);

            // Create or get existing group
            var group = settings.FindGroup("Levels") ?? settings.CreateGroup("Levels", false, false, true, null);
            var groupElementCount = group.entries.Count;

            // Create an entry in the Addressable Asset settings
            var guid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(prefab));
            var entry = settings.CreateOrMoveEntry(guid, group);
            entry.address = $"Level_{groupElementCount + 1}";

            // Save the settings and refresh
            settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entry, true);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// Retrieves the index for the next level by counting the number of entries in the "Levels" group of the Addressable Assets.
        /// If the "Levels" group does not exist, it returns 0, indicating no levels have been added yet.
        /// </summary>
        private static int GetLevelPointerIndex()
        {
            // Get the Addressable Asset settings
            var settings = AddressableAssetSettingsDefaultObject.GetSettings(true);
            var group = settings.FindGroup("Levels");
            return settings.FindGroup("Levels") == null ? 0 : group.entries.Count;
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