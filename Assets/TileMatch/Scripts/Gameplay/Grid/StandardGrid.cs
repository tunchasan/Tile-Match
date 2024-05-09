using UnityEngine;
using UnityEngine.Rendering;
using TileMatch.Scripts.Gameplay.Tile;

namespace TileMatch.Scripts.Gameplay.Grid
{
    [RequireComponent(typeof(SortingGroup))]
    public class StandardGrid : MonoBehaviour
    {
        public StandardTile AttachedTile { get; private set; }

        private SortingGroup _sortingGroup;
        private int _originalSortingOrder;

        private void Awake()
        {
            _sortingGroup = GetComponent<SortingGroup>();
            _originalSortingOrder = _sortingGroup.sortingOrder;
        }

        public void Highlight()
        {
            _sortingGroup.sortingOrder = 10;
        }

        public void ClearHighlight()
        {
            _sortingGroup.sortingOrder = _originalSortingOrder;
        }

        public void Fill(StandardTile standardTile, bool animate = true)
        {
            if (!this.IsEmpty()) return;
            AttachedTile = standardTile;
            AttachedTile.SetParent(transform);
            AttachedTile.ResetTransform(animate);
            AttachedTile.SetSortingOrder(_sortingGroup.sortingOrder);
        }

        public void Clear()
        {
            if(AttachedTile == null) return;
            AttachedTile = null;
        }
        
        public void FillInEditMode(StandardTile standardTile)
        {
            if (!this.IsEmpty()) return;

            if (_sortingGroup == null)
            {
                _sortingGroup = GetComponent<SortingGroup>();
            }

            AttachedTile = standardTile;
            AttachedTile.SetParent(transform);
            AttachedTile.ResetTransform();
            AttachedTile.SetSortingOrder(_sortingGroup.sortingOrder);
        }
        
        public void ClearInEditMode()
        {
            if(AttachedTile == null) return;
            
            #if UNITY_EDITOR
            TileFactory.Instance.DestroyTile(AttachedTile);
            #endif

            AttachedTile = null;
        }
    }
}