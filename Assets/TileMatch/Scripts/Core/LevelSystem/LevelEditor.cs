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

            var boardTop = (LevelGenerator)target;

            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("Generate Level"))
            {
                boardTop.GenerateLevel();
                Debug.Log("Level Generated!");
            }
            
            GUI.backgroundColor = Color.yellow;
            if (GUILayout.Button("Save Level"))
            {
                Debug.Log("Level Saved!");
            }
            
            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("Clear Level"))
            {
                boardTop.ClearLevel();
                Debug.Log("Level Cleared!");
            }
        }
    }
}