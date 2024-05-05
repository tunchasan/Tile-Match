using System;

namespace TileMatch.Scripts.Gameplay.Tile
{
    public static class TileUtils
    {
        private static readonly Random Random = new();

        public static TileType GetRandomTileType()
        {
            // Get all values of the enum
            var values = Enum.GetValues(typeof(TileType));

            // Choose a random index
            var randomIndex = Random.Next(0, values.Length);

            // Return the random enum value
            return (TileType)values.GetValue(randomIndex);
        }
    }
}