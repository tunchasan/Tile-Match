namespace TileMatch.Scripts.Gameplay.Board
{
    using UnityEngine;

    public class Board : MonoBehaviour
    {
        [field: SerializeField] public BoardConfig Config { get; private set; }

        private void Start()
        {
            CreateGrid();
        }

        private void CreateGrid()
        {
            Debug.LogError("Board::CreateGrid");
        }
    }
}