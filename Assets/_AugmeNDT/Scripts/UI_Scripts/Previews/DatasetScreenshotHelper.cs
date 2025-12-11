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
        Debug.Log($"[ScreenshotHelper] CapturePreview for {datasetPath}, " +
             $"HasPolygonal={group.HasPolygonalDataset}, HasAbstract={group.HasAbstractDataset}");
        GameObject previewClone = null;

        // SPATIAL: fiber-only preview (this is what was working)
        if (group.HasPolygonalDataset)
        {
            GameObject fiberRoot = group.GetPolyFiberObject();   // returns the fiber bundle root
            if (fiberRoot == null)
            {
                Debug.Log("[ScreenshotHelper] No fiber root for preview.");
                return;
            }

            Transform anchor = pageData.GetPreviewAnchor();      // small anchor on the page
            previewClone = Instantiate(fiberRoot, anchor, false);
        }
        // ABSTRACT: leave empty for now so it cannot break fibers
        else if (group.HasAbstractDataset)
        {
            GameObject abstractRoot = group.GetLastAbstractVisObject();  // scatterplot / MDD glyphs
            if (abstractRoot == null) {
                Debug.Log("dataset preview is null");
            };

            Transform anchor = pageData.GetPreviewAnchor();
            previewClone = Instantiate(abstractRoot, anchor, false);
            Transform selectionBoxes = previewClone.transform.Find("VisContainer/SelectionBoxes");
            if (selectionBoxes != null)
                selectionBoxes.gameObject.SetActive(false);
            Debug.LogWarning("[ScreenshotHelper] Abstract preview is called.");
            // after Instantiate(abstractRoot, anchor, false);
            previewClone.transform.localPosition = Vector3.zero;
            previewClone.transform.localRotation = Quaternion.identity;
            previewClone.transform.localScale = Vector3.one * 0.5f;  // tune factor


        }
        else
        {
            Debug.LogWarning("[ScreenshotHelper] No polygonal or abstract data for preview.");
           
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
