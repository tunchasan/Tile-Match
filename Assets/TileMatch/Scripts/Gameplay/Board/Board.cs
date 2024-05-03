using UnityEngine;

namespace TileMatch.Scripts.Gameplay.Board
{
    public class Board : MonoBehaviour
    {
        [field: SerializeField] public BoardConfig Config { get; private set; }

        private void Start()
        {
            CreateGrid();
        }

        private void CreateGrid()
        {
            for (var y = 0; y < Config.Dimension.y; y++)
            {
                for (var x = 0; x < Config.Dimension.x; x++)
                {
                    var position = new Vector2(x, -y); // Calculate position for each tile
                    var tile = Instantiate(Config.Grid, transform); // Instantiate tile
                    tile.transform.localPosition = position;
                    tile.name = $"Grid ({x},{y})";
                }
            }
        }
    }
}