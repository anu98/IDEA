using UnityEngine;

public class FixDataMarkLayers : MonoBehaviour
{
    void Start()
    {
        int defaultLayer = LayerMask.NameToLayer("Default");

        // Find the Data Marks parent at runtime
        var dataMarks = GameObject.Find("Data Marks");
        if (dataMarks == null)
        {
            Debug.LogWarning("[FixDataMarkLayers] 'Data Marks' object not found.");
            return;
        }

        SetLayerRecursively(dataMarks, defaultLayer);
        Debug.Log("[FixDataMarkLayers] Reset layers on Data Marks and children.");
    }

    void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
            SetLayerRecursively(child.gameObject, layer);
    }
}
