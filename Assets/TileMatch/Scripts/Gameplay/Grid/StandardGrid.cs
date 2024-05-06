using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace TileMatch.Scripts.Gameplay.Grid
{
    [RequireComponent(typeof(SortingGroup))]
    public class StandardGrid : MonoBehaviour
    {
        public Tile.Tile AttachedTile { get; private set; }

        private SortingGroup _sortingGroup;
        private int _originalSortingOrder;

        private void Awake()
        {
            _sortingGroup = GetComponent<SortingGroup>();
        }

        public void Highlight()
        {
            _originalSortingOrder = _sortingGroup.sortingOrder;
            _sortingGroup.sortingOrder = 10;
        }

        public void ClearHighlight()
        {
            _sortingGroup.sortingOrder = _originalSortingOrder;
        }

        public bool Fill(Tile.Tile tile)
        {
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