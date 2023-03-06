using CodeBase.Infrastructure;
using EzySlice;
using UnityEngine;

public class Sliceable : MonoBehaviour
{
    public GameObject plane;
    public Material sliceMaterial;
    private void OnTriggerEnter(Collider other)
    {
        /*if (other.CompareTag("Slicer"))
        {
            Debug.Log("Sliced");
            Slice(new Vector3(other.GetComponent<BoxCollider>().size.x, 0, 
                other.GetComponent<BoxCollider>().size.z), Vector3.left);
        }*/
    }

    public GameObject[] Slice(Vector3 planeWorldPosition, Vector3 planeWorldDirection) {
        GameObject[] slicedObjects = gameObject.SliceInstantiate(planeWorldPosition, planeWorldDirection, sliceMaterial);
        if (slicedObjects == null)
            return null;
        
        foreach (GameObject slice in slicedObjects)
        {
            slice.AddComponent<Rigidbody>();
            Rigidbody component = slice.GetComponent<Rigidbody>();
            component.isKinematic = false;
            component.useGravity = true;

            slice.AddComponent<BoxCollider>();
            BoxCollider boxCollider = slice.GetComponent<BoxCollider>();
            Vector3 boxColliderSize = boxCollider.size;
            boxCollider.size = new Vector3(boxColliderSize.x, boxColliderSize.y, 0);
            component.AddForce(planeWorldDirection);
            
            Destroy(gameObject);
            Destroy(slice, 5);
        }
        
        return slicedObjects;
    }
}
