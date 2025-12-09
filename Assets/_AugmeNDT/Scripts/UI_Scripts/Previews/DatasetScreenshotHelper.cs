using AugmeNDT;
using UnityEngine;

public class DatasetScreenshotHelper : MonoBehaviour
{
    public SnapshotSaver snapshotSaver;

    public void CapturePreview(DataVisGroup group, PageData pageData, string datasetPath)
    {
        if (group == null || pageData == null)
        {
            Debug.LogError("[ScreenshotHelper] Group or PageData is null.");
            return;
        }

        GameObject previewClone = null;

        // SPATIAL: fiber-only preview (this is what was working)
        if (group.HasPolygonalDataset)
        {
            GameObject fiberRoot = group.GetPolyFiberObject();   // returns the fiber bundle root
            if (fiberRoot == null)
            {
                Debug.LogWarning("[ScreenshotHelper] No fiber root for preview.");
                return;
            }

            Transform anchor = pageData.GetPreviewAnchor();      // small anchor on the page
            previewClone = Instantiate(fiberRoot, anchor, false);
        }
        // ABSTRACT: leave empty for now so it cannot break fibers
        else if (group.HasAbstractDataset)
        {
            Debug.LogWarning("[ScreenshotHelper] Abstract preview not implemented yet.");
            return;
        }
        else
        {
            Debug.LogWarning("[ScreenshotHelper] No polygonal or abstract data for preview.");
            return;
        }

        int previewLayer = LayerMask.NameToLayer("Preview");
        if (previewLayer != -1)
            SetLayerRecursively(previewClone, previewLayer);     // clone only

        snapshotSaver.CaptureObject(previewClone.transform, datasetPath);
    }

    void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
            SetLayerRecursively(child.gameObject, layer);
    }
}
