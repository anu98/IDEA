using UnityEngine;
using MixedReality.Toolkit.UX;
using MixedReality.Toolkit.SpatialManipulation;



public class TrashCan : MonoBehaviour
{
  //  [SerializeField] private string deletableTag = "Deletable";
    private void OnTriggerEnter(Collider other)
    {
       // if (!other.CompareTag(deletableTag)) return;

        // Check if handle of a graph/plot
        bool isGraphHandle = other.name.Contains("HandleObject");

        if (isGraphHandle)
        {
            Destroy(other.transform.parent.parent.gameObject);  // deletes MDD Glyphs Chart etc.
        }
        else
        {
             Destroy(other.gameObject);

        }


    }
}

