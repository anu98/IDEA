using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace AugmeNDT
{
    public class AttributePopulator : MonoBehaviour
    {
        public TMP_Dropdown xAxisDropdown;
        public TMP_Dropdown yAxisDropdown;
        public TMP_Dropdown zAxisDropdown;

        /// <summary>
        /// Call this after the dataset is loaded
        /// </summary>
        public void Populate(List<string> attributes)
        {
            //List<string> attributes = loader.GetAllAttributeNames();

            if (attributes == null || attributes.Count == 0)
            {
                Debug.LogWarning("No attributes found in CSV");
                return;
            }

            // Clear existing options
            xAxisDropdown.ClearOptions();
            yAxisDropdown.ClearOptions();
            zAxisDropdown.ClearOptions();

            // Convert string list to TMP_Dropdown.OptionData list
            List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
            foreach (var attr in attributes)
            {
                options.Add(new TMP_Dropdown.OptionData(attr));
            }

            // Add options to all three dropdowns
            xAxisDropdown.AddOptions(options);
            yAxisDropdown.AddOptions(options);
            zAxisDropdown.AddOptions(options);

            // Optionally set default selections
            xAxisDropdown.value = 0;
            yAxisDropdown.value = 1 < options.Count ? 1 : 0;
            zAxisDropdown.value = 2 < options.Count ? 2 : 0;
        }
    }
}
