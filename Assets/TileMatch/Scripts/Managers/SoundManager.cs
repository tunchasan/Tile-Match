using UnityEngine;
using System.Collections.Generic;
using TileMatch.Scripts.Gameplay.Tile;
using TileMatch.Scripts.Core.NotifySystem;

namespace TileMatch.Scripts.Managers
{
    public class SoundManager : MonoBehaviour
    {
        [field: SerializeField] public AudioSource TileSfx { get; private set; }
        [field: SerializeField] public AudioSource MergeSfx { get; private set; }
        
        private void PlayTileSound(StandardTile _)
        {
            TileSfx.Play();
        }
        
        private void PlayMergeSound(List<StandardTile> _)
        {
            MergeSfx.Play();
        }
        
        private void OnEnable()
        {
            NotificationCenter.AddObserver<StandardTile>(NotificationTag.OnTilePress, PlayTileSound);
            NotificationCenter.AddObserver<List<StandardTile>>(NotificationTag.OnTilesMatched, PlayMergeSound);
        }

        private void OnDisable()
        {
            NotificationCenter.RemoveObserver<StandardTile>(NotificationTag.OnTilePress, PlayTileSound);
            NotificationCenter.RemoveObserver<List<StandardTile>>(NotificationTag.OnTilesMatched, PlayMergeSound);
        }
    }
}