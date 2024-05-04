using EditorAttributes;
using UnityEngine;

namespace TileMatch.Scripts.Gameplay
{
    public class Tile : MonoBehaviour
    {
        [field: SerializeField, ReadOnly] public int Id { get; private set; }
        [field: SerializeField, ReadOnly] public int SortingOrder { get; private set; }
        [field: SerializeField, ReadOnly] public bool InteractionStatus { get; private set; } = true;
        [field: SerializeField, ReadOnly] public TileType Type { get; private set; }
        [field: SerializeField] private SpriteRenderer Renderer { get; set; }

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
            InteractionStatus = status;
            SetColor(status ? Color.white : Color.gray);
        }

        public void ResetTransform()
        {
            var tileTransform = transform;
            tileTransform.localScale = Vector3.one;
            tileTransform.localPosition = Vector3.zero;
        }
    }
}