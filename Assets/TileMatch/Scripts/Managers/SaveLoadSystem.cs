using UnityEngine;

namespace TileMatch.Scripts.Managers
{
    public static class SaveLoadSystem
    {
        private const string LevelPointer = "LevelPointer";

        public static int LoadLevelPointer()
        {
            return PlayerPrefs.GetInt(LevelPointer, 0);
        }
        
        public static int SaveLevelPointer(int value)
        {
            PlayerPrefs.SetInt(LevelPointer, value);
            return value;
        }
    }
}