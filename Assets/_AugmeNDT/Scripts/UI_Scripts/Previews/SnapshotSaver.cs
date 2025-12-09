using System.Collections;
using System.IO;
using UnityEngine;

public class SnapshotSaver : MonoBehaviour
{
    public Camera previewCamera;
    public int width = 512;
    public int height = 512;

    public void CaptureObject(Transform target, string datasetPath)
    {
        if (target == null || previewCamera == null) return;
        StartCoroutine(CaptureAndSave(target, datasetPath));
    }

    IEnumerator CaptureAndSave(Transform target, string datasetPath)
    {
        // Frame object
        Bounds b = CalculateBounds(target);
        Vector3 center = b.center;
        float radius = b.extents.magnitude;
        float distance = radius * 1.5f;

        previewCamera.transform.position = center + new Vector3(1, 1, -distance);
        previewCamera.transform.LookAt(center);
        

        yield return new WaitForEndOfFrame();

        // Render to RT
        RenderTexture rt = new RenderTexture(width, height, 16);
        previewCamera.targetTexture = rt;
        previewCamera.Render();

        RenderTexture.active = rt;
        Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();

        previewCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);

        byte[] pngData = tex.EncodeToPNG();
        Destroy(tex);

        string filePath = PreviewPathHelper.GetPreviewPathForDataset(datasetPath);
        File.WriteAllBytes(filePath, pngData);

        Debug.Log("Saved dataset preview: " + filePath);
    }

    Bounds CalculateBounds(Transform root)
    {
        var renderers = root.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0)
            return new Bounds(root.position, Vector3.one * 0.1f);

        Bounds b = renderers[0].bounds;
        for (int i = 1; i < renderers.Length; i++)
            b.Encapsulate(renderers[i].bounds);
        return b;
    }
}
