using DG.Tweening;
using UnityEngine;
using EditorAttributes;
using TileMatch.Scripts.Core.NotifySystem;

namespace TileMatch.Scripts.Gameplay.Tile
{
    public class StandardTile : MonoBehaviour
    {
        [field: SerializeField, ReadOnly] public int Id { get; private set; }
        [field: SerializeField, ReadOnly] public int SortingOrder { get; private set; }
        [field: SerializeField, ReadOnly] public bool InteractionStatus { get; private set; } = true;
        [field: SerializeField, ReadOnly] public TileType Type { get; private set; }
        [field: SerializeField] private SpriteRenderer Renderer { get; set; }
        [field: SerializeField] private BoxCollider2D Collider { get; set; }
        [field: SerializeField] private ParticleSystem Particle { get; set; }
        
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

        private void SetColor(Color color)
        {
            Renderer.DOColor(color, .25F);
        }

        public void SetInteraction(bool status, bool updateVisual = true)
        {
            Collider.enabled = status;
            InteractionStatus = status;
            if(!updateVisual) return;
            SetColor(status ? Color.white : Color.gray);
        }
        
        public void SetInteractionInEditMode(bool status)
        {
            Collider.enabled = status;
            InteractionStatus = status;
            Renderer.color = status ? Color.white : Color.gray;
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

        public void Destroy()
        {
            Particle.Play();
            Particle.transform.SetParent(null);
            var mainModule = Particle.main;
            mainModule.startColor = Type.GetColorByType();
            
            transform.DOScale(Vector3.zero, .4F);
            Destroy(gameObject, .4F);
        }
        
        private void OnMouseDown()
        {
            transform.DOScale(Vector3.one * 1.2F, .25F);
            NotificationCenter.PostNotification(NotificationTag.OnTilePress, this);
        }

        private void OnMouseUp()
        {
            DOTween.Kill(transform);
            transform.DOScale(Vector3.one, .1F);
            NotificationCenter.PostNotification(NotificationTag.OnTileRelease, this);
            NotificationCenter.PostNotification(NotificationTag.OnTileSelect, this);
        }
    }
}