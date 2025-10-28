using AugmeNDT;
using TMPro;
using UnityEngine;

public class VisTypeSelector : MonoBehaviour
{
    public TMP_Dropdown visTypeDropdown;
    public VisChannelUpdater updater; // assign the VisChannelUpdater in inspector

    void Start()
    {
        if (visTypeDropdown != null)
        {
            visTypeDropdown.onValueChanged.AddListener(OnDropdownChanged);
        }
    }

    void OnDropdownChanged(int index)
    {
        string selected = visTypeDropdown.options[index].text;
        switch (selected)
        {
            case "Bar Chart":
                updater.visType = VisType.BarChart;
                break;
            case "Scatterplot":
                updater.visType = VisType.Scatterplot;
                break;
            case "Histogram":
                updater.visType = VisType.Histogram;
                break;
            case "MDDGlyphs":
                updater.visType = VisType.MDDGlyphs;
                break;
            case "ChronoBins":
                updater.visType = VisType.ChronoBins;
                break;
            default:
                updater.visType = VisType.MDDGlyphs;
                break;
        }
    }
}

