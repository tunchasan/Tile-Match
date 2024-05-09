using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using TileMatch.Scripts.Core.NotifySystem;

namespace TileMatch.Scripts.Managers
{
    public class BackgroundSystem : MonoBehaviour
    {
        [field: SerializeField] public List<GameObject> backgrounds = new();

        public GameObject CurrentBackground { get; private set; }
        
        private void OnLevelLoaded(int value)
        {
            if (value % 3 == 0)
            {
                if (CurrentBackground == null)
                {
                    CurrentBackground = backgrounds[Random.Range(0, backgrounds.Count)];
                }

                else
                {
                    var remainedBackgrounds = backgrounds.Except(new[] { CurrentBackground }).ToArray();
                    CurrentBackground = remainedBackgrounds[Random.Range(0, remainedBackgrounds.Length)];
                }

                HideBackgroundsExceptCurrent();
                CurrentBackground.SetActive(true);
            }
        }

        private void HideBackgroundsExceptCurrent()
        {
            foreach (var background in backgrounds)
            {
                if(background == CurrentBackground) continue;
                background.SetActive(false);
            }
        }
        
        private void OnEnable()
        {
            NotificationCenter.AddObserver<int>(NotificationTag.OnLevelLoaded, OnLevelLoaded);
        }

        private void OnDisable()
        {
            NotificationCenter.RemoveObserver<int>(NotificationTag.OnLevelLoaded, OnLevelLoaded);
        }
    }
}