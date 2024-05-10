using System.Collections.Generic;
using UnityEngine;

namespace TileMatch.Scripts.Utils
{
    public static class Utilities
    {
        /// <summary>
        /// Shuffles the elements of the given list in place using the Fisher-Yates shuffle algorithm.
        /// </summary>
        /// <param name="array">The list of items to be shuffled.</param>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        public static void Shuffle<T>(IList<T> array)
        {
            for (var i = array.Count - 1; i > 0; i--)
            {
                var randomIndex = Random.Range(0, i + 1);
                (array[i], array[randomIndex]) = (array[randomIndex], array[i]);
            }
        }
    }
}