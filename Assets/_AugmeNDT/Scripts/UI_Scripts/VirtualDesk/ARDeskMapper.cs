using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARDeskMapper : MonoBehaviour
{
    [Header("AR Components")]
    public ARPlaneManager planeManager;  // will auto-find if null

    [Header("Virtual Desk")]
    public GameObject virtualDeskPrefab;

    private GameObject spawnedDesk;
    private bool deskPlaced = false;

    void Awake()
    {
        // Auto-assign planeManager if not set
        if (planeManager == null)
        {
            planeManager = FindObjectOfType<ARPlaneManager>();
            if (planeManager == null)
                Debug.LogError("No ARPlaneManager found in the scene!");
        }
    }

    void OnEnable()
    {
        if (planeManager != null)
            planeManager.planesChanged += OnPlanesChanged;
    }

    void OnDisable()
    {
        if (planeManager != null)
            planeManager.planesChanged -= OnPlanesChanged;
    }

    void OnPlanesChanged(ARPlanesChangedEventArgs args)
    {
        if (deskPlaced) return;

        foreach (ARPlane plane in args.added)
        {
            if (plane.alignment == PlaneAlignment.HorizontalUp)
            {
                PlaceDesk(plane);
                deskPlaced = true;
                break;
            }
        }
    }

    void PlaceDesk(ARPlane plane)
    {
        // Use plane's world position
        spawnedDesk = Instantiate(virtualDeskPrefab, plane.transform.position, plane.transform.rotation);

        // Optional: attach anchor to fix desk in AR space
        var anchor = plane.gameObject.AddComponent<ARAnchor>();
        spawnedDesk.transform.parent = anchor.transform;

        Debug.Log("Virtual desk placed on detected plane!");
    }
}