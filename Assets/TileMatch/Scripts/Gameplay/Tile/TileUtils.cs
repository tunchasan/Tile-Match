using System;
using UnityEngine;
using Random = System.Random;

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

        public static Color GetColorByType(this TileType type)
        {
            switch (type)
            {
                case TileType.BluePentagon:
                case TileType.BlueSquare:
                case TileType.BlueStar:
                    return Color.blue;
                case TileType.BrownPentagon:
                case TileType.BrownSquare:
                case TileType.BrownStar:
                    return new Color(0.5961f, 0.2235f, 0.0f, 1.0f); // RGB for Brown
                case TileType.CreamPentagon:
                case TileType.CreamSquare:
                case TileType.CreamStar:
                    return new Color(1.0f, 0.4902f, 0.1843f, 1.0f); // RGB for Cream
                case TileType.GreenPentagon:
                case TileType.GreenSquare:
                case TileType.GreenStar:
                    return Color.green;
                case TileType.LightBluePentagon:
                case TileType.LightBlueSquare:
                case TileType.LightBlueStar:
                    return new Color(0.1255f, 0.6118f, 0.8784f, 1.0f); // RGB for Light Blue
                case TileType.OrangePentagon:
                case TileType.OrangeSquare:
                case TileType.OrangeStar:
                    return new Color(0.8431f, 0.4510f, 0.1176f, 1.0f); // RGB for Orange
                case TileType.PinkPentagon:
                case TileType.PinkSquare:
                case TileType.PinkStar:
                    return Color.magenta;
                case TileType.PurplePentagon:
                case TileType.PurpleSquare:
                case TileType.PurpleStar:
                    return new Color(0.4392f, 0.0588f, 0.7882f, 1.0f); // RGB for Purple
                case TileType.RedPentagon:
                case TileType.RedSquare:
                case TileType.RedStar:
                    return Color.red;
                case TileType.YellowPentagon:
                case TileType.YellowSquare:
                case TileType.YellowStar:
                    return Color.yellow;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}