using UnityEditor;
using UnityEngine;

namespace TileMatch.Scripts.Gameplay.Board
{
    [CustomEditor(typeof(BoardTop))]
    public class BoardTopEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var boardTop = (BoardTop)target;

            GUI.backgroundColor = Color.green;

            if (GUILayout.Button("Generate Level"))
            {
                boardTop.GenerateLevel();
            }
            
            GUI.backgroundColor = Color.yellow;

            if (GUILayout.Button("Clear Level"))
            {
                boardTop.ClearLevel();
                Debug.Log("Level Cleared!");
            }
            
            GUI.backgroundColor = Color.yellow;

            if (GUILayout.Button("Save Level"))
            {
                Debug.Log("Level Saved!");
            }
        }
    }
}