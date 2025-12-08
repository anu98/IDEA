using UnityEngine;

namespace AugmeNDT
{
    public static class DatasetPreviewFactory
    {
        public static void CreatePreview(PageData pageData, DataVisGroup group, float previewScale = 0.2f)
        {
            if (pageData == null || group == null) return;

            if (group.HasPolygonalDataset)
            {
                CreateFiberPreview(pageData, group, previewScale);
            }
            else if (group.HasAbstractDataset)
            {
                CreateMddGlyphPreview(pageData, group, previewScale);
            }
            else
            {
                Debug.LogWarning("DatasetPreviewFactory: No polygonal or abstract data to preview.");
            } 
        }

        static void CreateFiberPreview(PageData pageData, DataVisGroup group, float previewScale)
        {
            //var groupRoot = group.GetGroupContainer();
            //var polyObj = groupRoot.GetComponentInChildren<PolyFiberRenderedObject>(true);
            //if (polyObj == null)
            //{
            //    Debug.LogWarning("DatasetPreviewFactory: PolyFiberRenderedObject not found for fiber preview.");
            //    return;
            //}
            Debug.Log($"[DatasetPreviewFactory] Creating preview for page {pageData.gameObject.name}");
            Transform anchor = pageData.GetPreviewAnchor();

            // Find the actual root GameObject that contains the fibers
            var groupRoot = group.GetGroupContainer();
            Transform fiberRoot = groupRoot.transform; // or a more specific child if you have one

            GameObject previewInstance = Object.Instantiate(fiberRoot.gameObject, anchor);
            previewInstance.transform.localPosition = Vector3.zero;
            previewInstance.transform.localRotation = Quaternion.identity;
            previewInstance.transform.localScale = Vector3.one * previewScale;

            pageData.PreviewObject = previewInstance;
        }

        static void CreateMddGlyphPreview(PageData pageData, DataVisGroup group, float previewScale)
        {
            // Create a minimal MDDGlyphs vis as a tiny preview
            var groupRoot = group.GetGroupContainer();

            VisMDDGlyphs vis = new VisMDDGlyphs();
            vis.AppendData(group.GetAbstractCsvData());
            vis.SetDataVisGroup(group);

            // Create a dedicated small container under the page for the preview
            Transform anchor = pageData.GetPreviewAnchor();
            GameObject previewContainer = new GameObject("MDDGlyphsPreview");
            previewContainer.transform.SetParent(anchor, false);

            vis.CreateVis(previewContainer);
            previewContainer.transform.localScale = Vector3.one * previewScale;

            pageData.PreviewObject = previewContainer;
        }
    }
}
