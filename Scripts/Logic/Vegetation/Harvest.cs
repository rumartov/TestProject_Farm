using System;
using DG.Tweening;
using UnityEngine;

namespace Logic.Vegetation
{
    public class Harvest : MonoBehaviour
    {
        private bool _pickedUp;
        public VegetationType VegetationType { get; set; }

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.CompareTag("Player")) PickUp(collider.GetComponentInChildren<Backpack>());
        }

        public void Construct(VegetationType vegetationType)
        {
            VegetationType = vegetationType;
            _pickedUp = false;
        }

        public void PlayPickUpAnimation(Transform endPosition, Action onComplete)
        {
            var target = gameObject;
            var targetTransform = target.transform;

            var height = targetTransform.position + Vector3.up * 2;
            Tweener doMoveUp = targetTransform.DOMove(height, Constants.PickUpStackTime);

            doMoveUp.OnComplete(() =>
            {
                Tweener doMoveFollow = targetTransform.DOMove(endPosition.position, Constants.FollowStackTime);

                doMoveFollow.OnUpdate(() =>
                {
                    if (Vector3.Distance(endPosition.position, targetTransform.position) > 0.5f)
                        doMoveFollow.ChangeEndValue(endPosition.position, true);
                });

                doMoveFollow.OnComplete(() =>
                {
                    target.SetActive(false);
                    onComplete?.Invoke();
                });
            });
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
            var harvestGameObject = transform.parent.gameObject;

            backpack.PackItem(gameObject);
            gameObject.transform.SetParent(backpack.transform);

            Destroy(harvestGameObject);
        }
    }
}