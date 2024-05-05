#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace TileMatch.Scripts.Core.LevelSystem
{
    /// <summary>
    /// Custom editor for the LevelGenerator class, providing buttons for generating,
    /// clearing, and saving levels.
    /// </summary>
    [CustomEditor(typeof(LevelGenerator))]
    public class LevelEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var levelGenerator = (LevelGenerator)target;

            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("Generate Level"))
            {
                levelGenerator.GenerateLevel();
                Debug.Log("Level Generated!");
            }
            
            GUI.backgroundColor = Color.yellow;
            if (GUILayout.Button("Save Level"))
            {
                levelGenerator.SaveLevel();
                Debug.Log("Level Saved!");
            }
            
            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("Clear Level"))
            {
                levelGenerator.ClearLevel();
                Debug.Log("Level Cleared!");
            }
        }
    }
}

#endif