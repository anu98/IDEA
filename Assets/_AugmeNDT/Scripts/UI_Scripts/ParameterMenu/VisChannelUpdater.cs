using UnityEngine;
using TMPro;
using AugmeNDT;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class VisChannelUpdater : MonoBehaviour
{
    [Header("Dropdowns for Axis Encoding")]
    public TMP_Dropdown xDropdown;
    public TMP_Dropdown yDropdown;
    public TMP_Dropdown zDropdown;
    public TMP_Dropdown colorDropdown;
    public TMP_Dropdown sizeDropdown;

    [Header("Target Visualization")]
    public DataVisGroup currentGroup; // drag the DataVisGroup from scene
    [Header("Visualization Type")]
    public AugmeNDT.VisType visType = AugmeNDT.VisType.MDDGlyphs;

    // Called when "Apply" button is pressed
    public void OnApply()
    {

        Debug.Log("Apply pressed");

        if (currentGroup == null)
        {
            Debug.LogError("Current DataVisGroup is null!");
            return;
        }

        // ---- Clear old visualizations ----
        if (currentGroup.visualizations != null)
        {
            foreach (var vis in currentGroup.visualizations)
            {
                if (vis != null) vis.DeleteVis();
            }
            currentGroup.visualizations.Clear();
        }

        // ---- Prepare selected channels ----
        var channels = new Dictionary<AugmeNDT.VisChannel, AugmeNDT.Attribute>();

        var dataset = currentGroup.GetAbstractCsvData();
        if (dataset == null)
        {
            Debug.LogError("No abstract dataset in current group!");
            return;
        }

        // Helper to get Attribute from dropdown value
        AugmeNDT.Attribute GetAttributeFromDropdown(TMP_Dropdown dropdown)
        {
            string selectedName = dropdown.options[dropdown.value].text;
            int attrId = dataset.GetAttributeId(selectedName);
            return dataset.GetAttribute(attrId);
        }

        // ---- Assign channels depending on visType ----
        switch (visType)
        {
            case AugmeNDT.VisType.MDDGlyphs:
                channels[AugmeNDT.VisChannel.XPos] = GetAttributeFromDropdown(xDropdown);
                channels[AugmeNDT.VisChannel.YPos] = GetAttributeFromDropdown(yDropdown);
                channels[AugmeNDT.VisChannel.ZPos] = GetAttributeFromDropdown(zDropdown);
                //channels[AugmeNDT.VisChannel.XSize] = GetAttributeFromDropdown(sizeDropdown);
                //channels[AugmeNDT.VisChannel.Color] = GetAttributeFromDropdown(colorDropdown);
                break;

            case AugmeNDT.VisType.BarChart:
                channels[AugmeNDT.VisChannel.XPos] = GetAttributeFromDropdown(xDropdown);
                channels[AugmeNDT.VisChannel.YSize] = GetAttributeFromDropdown(yDropdown);
                channels[AugmeNDT.VisChannel.ZPos] = GetAttributeFromDropdown(zDropdown);
                //channels[AugmeNDT.VisChannel.Color] = GetAttributeFromDropdown(xDropdown);
                break;

            case AugmeNDT.VisType.Histogram:
                channels[AugmeNDT.VisChannel.XPos] = GetAttributeFromDropdown(xDropdown);
                channels[AugmeNDT.VisChannel.YSize] = GetAttributeFromDropdown(yDropdown);
                channels[AugmeNDT.VisChannel.Color] = GetAttributeFromDropdown(xDropdown);
                break;

            case AugmeNDT.VisType.Scatterplot:
                channels[AugmeNDT.VisChannel.XPos] = GetAttributeFromDropdown(xDropdown);
                channels[AugmeNDT.VisChannel.YPos] = GetAttributeFromDropdown(yDropdown);
                channels[AugmeNDT.VisChannel.ZPos] = GetAttributeFromDropdown(zDropdown);
                channels[AugmeNDT.VisChannel.Color] = GetAttributeFromDropdown(xDropdown);
                break;

            case AugmeNDT.VisType.ChronoBins:
                channels[AugmeNDT.VisChannel.XPos] = GetAttributeFromDropdown(xDropdown);
                channels[AugmeNDT.VisChannel.YPos] = GetAttributeFromDropdown(yDropdown);
                channels[AugmeNDT.VisChannel.ZPos] = GetAttributeFromDropdown(zDropdown);
                channels[AugmeNDT.VisChannel.Color] = GetAttributeFromDropdown(xDropdown);
                break;

            default:
                Debug.LogWarning("Unknown VisType, defaulting to MDDGlyphs");
                channels[AugmeNDT.VisChannel.XPos] = GetAttributeFromDropdown(xDropdown);
                channels[AugmeNDT.VisChannel.YPos] = GetAttributeFromDropdown(yDropdown);
                channels[AugmeNDT.VisChannel.ZPos] = GetAttributeFromDropdown(zDropdown);
                channels[AugmeNDT.VisChannel.XSize] = GetAttributeFromDropdown(xDropdown);
                channels[AugmeNDT.VisChannel.Color] = GetAttributeFromDropdown(xDropdown);
                break;
        }


        // ---- Render new visualization ----
        currentGroup.RenderAbstractVisObject(visType, channels);

        // Optional: arrange objects in space
        currentGroup.ArrangeObjectsSpatially();

        Debug.Log("Visualization updated with new parameters");
    }

    private int GetAttributeIdFromDropdown(TMP_Dropdown dropdown, AbstractDataset dataset)
    {
        if (dropdown == null) return -1;

        string attrName = dropdown.options[dropdown.value].text;
        return dataset.GetAttributeId(attrName);
    }
    public void ClearVisualizations(DataVisGroup group)
    {
        if (group.visualizations == null || group.visualizations.Count == 0) return;

        foreach (Vis vis in group.visualizations)
        {
            if (vis != null)
            {
                vis.DeleteVis();   // This destroys visContainerObject
            }
        }

        group.visualizations.Clear();
    }
    public void RenderNewVisualization(DataVisGroup group, VisType visType, Dictionary<VisChannel, Attribute> selectedChannels)
    {
        // Clear old visualizations first
        ClearVisualizations(group);

        // Render the new visualization with selected channels
        group.RenderAbstractVisObject(visType, selectedChannels);

        // Optional: re-arrange in space if needed
        group.ArrangeObjectsSpatially();
    }
    public void SetCurrentGroup(DataVisGroup group)
    {
        currentGroup = group;
        Debug.Log("Current DataVisGroup set in VisChannelUpdater: " + group);
    }


}

