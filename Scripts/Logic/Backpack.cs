using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Logic.Vegetation;
using Services.Input;
using Services.PersistentProgress;
using UnityEngine;

namespace Logic
{
    public class Backpack : MonoBehaviour
    {
        public Transform itemLook;

        private GameObject _backpackVisualItem;

        private IInputService _inputService;

        private bool _isAnimationPlaying;
        private IPersistentProgressService _progressService;
        public int Size { get; private set; }
        public List<GameObject> Container { get; set; }

        private void Update()
        {
            RotateBackpack();

            PlayAnimation(_backpackVisualItem);
        }

        public void Construct(int size, IInputService inputService, IPersistentProgressService progressService)
        {
            Size = size;
            Container = new List<GameObject>(Size);
            _isAnimationPlaying = false;

            _inputService = inputService;
            _progressService = progressService;
        }

        private void PlayAnimation(GameObject target)
        {
            if (!_isAnimationPlaying && !IsBackpackVisualNull() && IsPlayerMoving())
            {
                _isAnimationPlaying = true;
                target.transform.DOShakePosition(0.1f, 0.04f)
                    .OnComplete(() => _isAnimationPlaying = false);
            }
        }

        private void RotateBackpack()
        {
            if (!IsBackpackVisualNull()) _backpackVisualItem.transform.LookAt(itemLook);
        }

        private bool IsPlayerMoving()
        {
            return _inputService.Axis.sqrMagnitude > Constants.Epsilon;
        }

        private bool IsBackpackVisualNull()
        {
            return _backpackVisualItem == null;
        }

        public void PackItem(GameObject item)
        {
            if (Container.Count == Size)
                return;

            Container.Add(item);

            if (_backpackVisualItem == null)
                EnableVisual(item);

            _progressService.Progress.WorldData.LootData.StackData.Add(1);
        }

        public void UnPackItem(GameObject item, Transform target)
        {
            DisableVisual();
            if (Container.Count > 0)
            {
                item.SetActive(true);
                item.GetComponent<Harvest>()
                    .PlayPickUpAnimation(target,
                        () =>
                        {
                            Container.Remove(item);
                            item.transform.SetParent(target);
                            if (Container.Count == 0)
                                DisableVisual();
                        });
            }

            _progressService.Progress.WorldData.LootData.StackData.Remove(1);
        }

        private IEnumerator Unpack(List<GameObject> container, Transform target, float decay)
        {
            foreach (var item in container)
            {
                yield return new WaitForSeconds(decay);
                UnPackItem(item, target);
            }
        }

        private void EnableVisual(GameObject item)
        {
            item.SetActive(true);
            _backpackVisualItem = item;
            _backpackVisualItem.GetComponent<Harvest>().enabled = false;
            _backpackVisualItem.transform.position = transform.position;
        }

        private void DisableVisual()
        {
            _backpackVisualItem = null;
        }

        public bool IsFull()
        {
            return Container.Count == Size;
        }

        public bool IsEmpty()
        {
            return Container.Count == 0;
        }
    }
}