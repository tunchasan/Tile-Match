using UnityEngine;

namespace TileMatch.Scripts.Utils
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static readonly object Lock = new();
        private static T _instance;

        public static T Instance
        {
            get
            {
                lock (Lock)
                {
                    if (_instance == null) _instance = (T) FindObjectOfType(typeof(T));

                    return _instance;
                }
            }
        }
    }
}