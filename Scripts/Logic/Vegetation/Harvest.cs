using System;
using DG.Tweening;
using UnityEngine;

namespace Logic.Vegetation
{
    public class Harvest : MonoBehaviour
    {
        private VegetationType VegetationType { get; set; }

        private bool _pickedUp;

        public void Construct(VegetationType vegetationType)
        {
            VegetationType = vegetationType;
            _pickedUp = false;
        }

        public void PlayPickUpAnimation(Transform endPosition, Action onComplete)
        {
            GameObject target = gameObject;
            Transform targetTransform = target.transform;
            
            Vector3 height = targetTransform.position + Vector3.up * 2;
            Tweener doMoveUp = targetTransform.DOMove(height, 1);
            
            doMoveUp.OnComplete(() =>
            {
                Tweener doMoveFollow = targetTransform.DOMove(endPosition.position, 0.09f);
            
                doMoveFollow.OnUpdate(() => {
                    if(Vector3.Distance(endPosition.position, targetTransform.position) > 0.5f) {
                        doMoveFollow.ChangeEndValue(endPosition.position, true);
                    }
                });

                doMoveFollow.OnComplete(() =>
                {
                    target.SetActive(false);
                    onComplete?.Invoke();
                });
            });
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.CompareTag("Player"))
            {
                PickUp(collider.GetComponentInChildren<Backpack>());
            }
        }

        private void PickUp(Backpack backpack)
        {
            if (backpack.IsFull())
                return;
            if (_pickedUp)
                return;
            
            PlayPickUpAnimation(backpack.transform, () => AddItemToBackpack(backpack));
         
            _pickedUp = true;
        }

        private void AddItemToBackpack(Backpack backpack)
        {
            GameObject harvestGameObject = transform.parent.gameObject;

            backpack.PackItem(gameObject);
            gameObject.transform.SetParent(backpack.transform);
               
            Destroy(harvestGameObject);
        }
    }
}
