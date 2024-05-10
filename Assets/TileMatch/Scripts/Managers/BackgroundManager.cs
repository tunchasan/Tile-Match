using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using EditorAttributes;
using Random = UnityEngine.Random;
using TileMatch.Scripts.Core.NotifySystem;

namespace TileMatch.Scripts.Managers
{
    public class BackgroundManager : MonoBehaviour
    {
        [field: SerializeField, ReadOnly] private GameObject CurrentBackground { get; set; }

        [field: SerializeField] public List<GameObject> backgrounds = new();

        private void OnLevelLoaded(int value)
        {
            if (value % 4 == 1)
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