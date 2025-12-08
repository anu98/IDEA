using UnityEngine;
using MixedReality.Toolkit.UX;
using MixedReality.Toolkit.SpatialManipulation;



public class TrashCan : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject); Destroy(other.gameObject);
     

    }
}

