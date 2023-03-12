using System;
using EzySlice;
using UnityEngine;

namespace Logic.Vegetation
{
    public class Sliceable : MonoBehaviour
    {
        public Material defaultSliceMaterial;

        public Action Sliced;

        public void Slice(Vector3 planeWorldPosition, Material sliceMaterial = null)
        {
            if (sliceMaterial == null)
                sliceMaterial = defaultSliceMaterial;

            var slicedObjects = gameObject.SliceInstantiate(planeWorldPosition, transform.up, sliceMaterial);

            if (slicedObjects == null) return;

            foreach (var slice in slicedObjects)
            {
                slice.transform.SetParent(transform.parent);
                SetSliceLayer(slice);
                slice.transform.position = transform.position;
                if (slice.name == "Upper_Hull")
                {
                    var rigidbody = AddRigidbodyToSlice(slice);
                    AddBoxCollidersToSlice(slice, rigidbody);
                }

                DestroyParts(slice);
            }

            Sliced?.Invoke();
        }

        private void SetSliceLayer(GameObject slice)
        {
            slice.layer = LayerMask.NameToLayer("Sliceable");
        }

        private Rigidbody AddRigidbodyToSlice(GameObject slice)
        {
            slice.AddComponent<Rigidbody>();
            var component = slice.GetComponent<Rigidbody>();
            component.isKinematic = false;
            component.useGravity = true;
            return component;
        }

        private void AddBoxCollidersToSlice(GameObject slice, Rigidbody component)
        {
            slice.AddComponent<BoxCollider>();
            var boxCollider = slice.GetComponent<BoxCollider>();
            var boxColliderSize = boxCollider.size;
            boxCollider.size = new Vector3(boxColliderSize.x, boxColliderSize.y, 0);
            component.AddForce(Vector3.left);
        }

        private void DestroyParts(GameObject slice)
        {
            Destroy(gameObject);
            Destroy(slice, 10);
        }
    }
}