using UnityEngine;

namespace EditorAttributes
{
    public class SelectionButtonsAttribute : PropertyAttribute
    {
	    public string CollectionName { get; private set; }
	    public float ButtonsHeight { get; private set; }
        public bool ShowLabel { get; private set; }

		/// <summary>
		/// Attribute to display a bunch of values in toggleble buttons
		/// </summary>
		/// <param name="collectionName">The name of the collection</param>
		/// <param name="buttonsHeight">The height of the selection buttons in pixels</param>
		/// <param name="showLabel">Show the label of the field</param>
		public SelectionButtonsAttribute(string collectionName = "", float buttonsHeight = 18f, bool showLabel = true)
        {
            CollectionName = collectionName;
            ButtonsHeight = buttonsHeight;
            ShowLabel = showLabel;
        }
    }
}
