using UnityEngine;
using UnityEngine.Rendering;

namespace TileMatch.Scripts.Gameplay.Grid
{
    [RequireComponent(typeof(SortingGroup))]
    public class Grid : MonoBehaviour
    {
        public Tile AttachedTile { get; private set; }

        private SortingGroup _sortingGroup;

        public bool Fill(Tile tile)
        {
            if (_sortingGroup == null)
            {
                _sortingGroup = GetComponent<SortingGroup>();
            }
            
            if (!this.IsEmpty()) return false;
            AttachedTile = tile;
            AttachedTile.SetParent(transform);
            AttachedTile.ResetTransform();
            AttachedTile.SetSortingOrder(_sortingGroup.sortingOrder);
            return true;
        }

        public void Clear()
        {
            if(AttachedTile == null) return;
            Main.Instance.TileFactory.DestroyTile(AttachedTile);
            AttachedTile = null;
        }
    }
}