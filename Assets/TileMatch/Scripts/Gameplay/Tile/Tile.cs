using System;
using DG.Tweening;
using UnityEngine;
using EditorAttributes;
using TileMatch.Scripts.Core.LevelSystem;
using TileMatch.Scripts.Core.NotifySystem;
using TileMatch.Scripts.Gameplay.Grid;

namespace TileMatch.Scripts.Gameplay.Tile
{
    public class Tile : MonoBehaviour
    {
        [field: SerializeField, ReadOnly] public int Id { get; private set; }
        [field: SerializeField, ReadOnly] public int SortingOrder { get; private set; }
        [field: SerializeField, ReadOnly] public bool InteractionStatus { get; private set; } = true;
        [field: SerializeField, ReadOnly] public TileType Type { get; private set; }
        [field: SerializeField] private SpriteRenderer Renderer { get; set; }
        [field: SerializeField] private BoxCollider2D Collider { get; set; }

        private void Start()
        {
            Main.Instance.TileFactory.ReceiveTile(this);
        }

        public void Init(TileType type, Sprite sprite)
        {
            Type = type;
            Renderer.sprite = sprite;
        }
        
        public void Init(int id, TileType type, Sprite sprite)
        {
            Id = id;
            Type = type;
            Renderer.sprite = sprite;
        }

        public void SetParent(Transform parent)
        {
            transform.SetParent(parent);
        }

        public void SetSortingOrder(int order)
        {
            SortingOrder = order;
        }

        public void SetColor(Color color)
        {
            Renderer.color = color;
        }

        public void SetInteraction(bool status)
        {
            Collider.enabled = status;
            InteractionStatus = status;
            SetColor(status ? Color.white : Color.gray);
        }

        public void ResetTransform(bool animate = false, float duration = .25F)
        {
            var tileTransform = transform;

            if (animate)
            {
                tileTransform.DOScale(Vector3.one, duration);
                tileTransform.DOLocalMove(Vector3.zero, duration);
            }

            else
            {
                tileTransform.localScale = Vector3.one;
                tileTransform.localPosition = Vector3.zero;   
            }
        }
        
        private void OnMouseDown()
        {
            GetComponentInParent<StandardGrid>()?.Highlight();
            transform.DOScale(Vector3.one * 1.2F, .25F);
        }

        private void OnMouseUp()
        {
            DOTween.Kill(transform);
            transform.DOScale(Vector3.one, .1F).OnComplete(() =>
            {
                GetComponentInParent<StandardGrid>()?.ClearHighlight();
            });
            
            NotificationCenter.PostNotification(NotificationTag.OnTileSelect, this);
        }
    }
}